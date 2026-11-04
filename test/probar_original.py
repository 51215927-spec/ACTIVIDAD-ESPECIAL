"""Pruebas de caja negra del original; no forma parte de la aplicación C#.

Dependencia local: python -m pip install --target herramientas pywinpty==3.1.2
Ejecutar desde la raíz: python test/probar_original.py
Solo escribe datos en la copia test/StreamORT.
"""
import base64
import json
import re
import select
import sys
import time
from pathlib import Path

BASE = Path(__file__).resolve().parent
sys.path.insert(0, str(BASE / 'herramientas'))
from winpty import PtyProcess

COPIA = BASE / 'StreamORT'
DATOS = COPIA / 'Datos'
DOTNET = BASE / 'herramientas/dotnet8/dotnet.exe'
ANSI = re.compile(r'\x1b\].*?(?:\x07|\x1b\\)|\x1b\[[0-?]*[ -/]*[@-~]|\x1b[78]', re.DOTALL)
RESULTADOS = json.loads((BASE / 'resultados/resultados.json').read_text(encoding='utf-8')) if (BASE / 'resultados/resultados.json').exists() else []


def limpiar(texto):
    texto = '\n'.join(linea.strip() for linea in ANSI.sub('', texto).splitlines() if linea.strip())
    return re.sub(r'╔[^\n]*\n.*?Competencia:[^\n]*\n?', '', texto, flags=re.DOTALL)


def b64(texto):
    return base64.b64encode(texto.encode('utf-8')).decode('ascii')


def escribir(archivo, filas):
    DATOS.mkdir(exist_ok=True)
    (DATOS / archivo).write_text('\n'.join(filas) + ('\n' if filas else ''), encoding='utf-8')


def preparar(equipos=None, creadores=None):
    escribir('competencia.txt', [f'{b64("Guardada")};{b64("Sponsor")};2027;10000'])
    escribir('equipos.txt', [f'{b64(n)};2000-01-01;{b64("Gaming")}' for n in (['Alpha', 'Vacio'] if equipos is None else equipos)])
    escribir('creadores.txt', creadores if creadores is not None else [creador('Alpha', 'Uno', 500, 8)])


def creador(equipo, alias, seguidores=500, valoracion=8):
    return f'{b64(equipo)};{b64("Nombre " + alias)};{b64(alias)};14;{seguidores};{valoracion};{b64("Streamer")}'


class Sesion:
    def __init__(self, nombre, runtime=None):
        self.nombre = nombre
        self.raw = ''
        self.pendiente = ''
        self.entradas = []
        self.seleccion = 1
        comando = [str(runtime or DOTNET)]
        if runtime:
            comando += ['--roll-forward', 'Major']
        self.proc = PtyProcess.spawn(comando + [str(COPIA / 'StreamORT.dll')], cwd=str(COPIA), dimensions=(40, 160))
        self.esperar('Competencia:')
        self.drenar()

    def drenar(self, segundos=0.12):
        limite = time.monotonic() + segundos
        while time.monotonic() < limite:
            if select.select([self.proc.fileobj], [], [], 0.02)[0]:
                try:
                    texto = self.proc.read(65536)
                except EOFError:
                    break
                self.raw += texto
                self.pendiente += ANSI.sub('', texto)
        return self.pendiente

    def esperar(self, texto, timeout=8):
        limite = time.monotonic() + timeout
        while texto not in self.pendiente:
            self.drenar(0.04)
            if time.monotonic() > limite:
                raise TimeoutError(f'{self.nombre}: esperaba {texto!r}; final: {self.pendiente[-1500:]}')
        return self.pendiente

    def enviar(self, texto):
        self.entradas.append(texto)
        self.proc.write(texto)

    def linea(self, prompt, texto):
        self.esperar(prompt)
        self.pendiente = ''
        self.enviar(str(texto) + '\r')

    def opcion(self, numero):
        self.pendiente = ''
        pasos = (numero - self.seleccion) % 12
        self.enviar('\x1b[B' * pasos + '\r')
        self.seleccion = numero
        self.drenar()

    def volver(self):
        texto = self.esperar('Presioná cualquier tecla para volver')
        self.pendiente = ''
        self.enviar('x')
        self.esperar('Competencia:')
        self.drenar()
        return limpiar(texto)

    def cargar(self, confirmar=None):
        self.opcion(11)
        if confirmar is not None:
            self.esperar('(S/N):')
            self.enviar(confirmar)
        return self.volver()

    def info(self):
        self.opcion(4)
        return self.volver()

    def crear_competencia(self, nombre='Memoria', confirmar=None, invalido=False):
        self.opcion(1)
        if confirmar is not None:
            self.esperar('(S/N):')
            self.enviar(confirmar)
        if invalido:
            self.linea('Nombre de la competencia:', '   ')
            self.esperar('El dato no puede estar vacío.')
        self.linea('Nombre de la competencia:', nombre)
        self.linea('Patrocinador:', 'Sponsor')
        self.linea('Año', 2027)
        self.linea('Máximo de equipos', 10000)
        return self.volver()

    def cerrar(self):
        self.drenar()
        try:
            self.proc.close(force=True)
        finally:
            (BASE / 'resultados' / f'{self.nombre}.ansi.txt').write_text(self.raw, encoding='utf-8')
            limpio = limpiar(self.raw)
            (BASE / 'resultados' / f'{self.nombre}.txt').write_text(limpio, encoding='utf-8')
            (BASE / 'resultados' / f'{self.nombre}.entradas.json').write_text(json.dumps(self.entradas, ensure_ascii=False, indent=2), encoding='utf-8')


def caso(nombre, funcion, runtime=None):
    sesion = Sesion(nombre, runtime)
    try:
        resultado = funcion(sesion)
        RESULTADOS[:] = [r for r in RESULTADOS if r['caso'] != nombre]
        RESULTADOS.append({'caso': nombre, 'resultado': resultado})
        print(nombre, 'completado', flush=True)
    finally:
        sesion.cerrar()
        (BASE / 'resultados/resultados.json').write_text(json.dumps(RESULTADOS, ensure_ascii=False, indent=2), encoding='utf-8')


def basicas(s):
    salida = {}
    for n in range(2, 11):
        s.opcion(n)
        salida[f'sin_competencia_{n}'] = s.volver()
    salida['lectura_inicial'] = s.cargar()
    s.opcion(8)
    s.linea('Primer equipo:', 'Vacio')
    s.linea('Segundo equipo:', 'Alpha')
    salida['vacio_con_lleno'] = s.volver()
    s.opcion(8)
    s.linea('Primer equipo:', 'Alpha')
    s.linea('Segundo equipo:', 'Vacio')
    salida['lleno_con_vacio'] = s.volver()
    for tecla in ['n', 'x', '\r', 'S']:
        s.opcion(1)
        s.esperar('(S/N):')
        s.enviar(tecla)
        if tecla == 'S':
            s.linea('Nombre de la competencia:', 'Nueva')
            s.linea('Patrocinador:', 'Patrocinador')
            s.linea('Año', 2027)
            s.linea('Máximo de equipos', 3)
        salida[f'recrear_{repr(tecla)}'] = s.volver()
        salida[f'info_recrear_{repr(tecla)}'] = s.info()
    for tecla in ['n', 'x', '\r', 'S']:
        salida[f'leer_{repr(tecla)}'] = s.cargar(tecla)
        salida[f'info_leer_{repr(tecla)}'] = s.info()
    return salida


if __name__ == '__main__':
    sys.stdout.reconfigure(encoding='utf-8')
    preparar()
    caso('01-basicas-net8', basicas)
