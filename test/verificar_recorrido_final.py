"""Recorrido supervisado por pseudoconsola Windows, con entradas y capturas ANSI.

No modifica los datos originales. Cada ejecución conserva su evidencia en una carpeta nueva.
Las PNG reconstruyen las celdas ANSI con Consolas; no son fotografías del escritorio.
"""
import json
import hashlib
import shutil
import sys
import tempfile
from datetime import date, timedelta
from pathlib import Path

import comparar_interfaz as captura


class Recorrido:
    def __init__(self, salida):
        self.salida = salida
        self.trabajo = Path(tempfile.mkdtemp(prefix='Recorrido-', dir=captura.ROOT / 'test/resultados/local'))
        captura.OUT = salida
        self.resultados = []
        self.numero = 0
        self.seleccion = 1
        dll = captura.ROOT / 'src/ACTIVIDAD ESPECIAL/bin/Debug/net8.0/ACTIVIDAD ESPECIAL.dll'
        self.s = captura.Capture('actual', ['dotnet', str(dll)], self.trabajo)

    def texto(self):
        return '\n'.join(self.s.screen.display)

    def comprobar(self, nombre, condicion, detalle=''):
        self.resultados.append(dict(caso=nombre, correcto=bool(condicion), detalle=detalle))

    def foto(self, nombre):
        self.numero += 1
        self.s.snapshot(f'{self.numero:03}-{nombre}')

    def opcion(self, numero):
        self.s.send('\x1b[B' * ((numero - self.seleccion) % 12) + '\r')
        self.seleccion = numero

    def volver(self):
        self.s.send('x')
        self.s.wait('Competencia:')
        lineas = [i for i, linea in enumerate(self.s.screen.display) if '►' in linea]
        self.comprobar(f'seleccion-conservada-{self.numero}',
                       len(lineas) == 1 and ' '.join(self.s.screen.display[lineas[0]].split()).startswith(
                           f'► {self.seleccion}.'), self.texto())

    def resultado(self, nombre, esperados=(), volver=True):
        self.s.wait('Presioná cualquier tecla para volver')
        texto = self.texto()
        self.foto(nombre)
        self.comprobar(nombre, all(valor in texto for valor in esperados), texto)
        fondo = 'blue' if self.seleccion in (2, 5, 7, 8, 9) else (
            'magenta' if self.seleccion in (3, 6) else 'black')
        fondo_actual = self.s.screen.buffer[0][0].bg
        self.comprobar(nombre + '-fondo', fondo_actual == fondo or (fondo == 'black' and fondo_actual == 'default'),
                       str(self.s.screen.buffer[0][0].bg))
        if volver:
            self.volver()

    def equipo(self, nombre, categoria='1'):
        self.opcion(2)
        self.s.line('Nombre del equipo:', nombre)
        self.s.line('Fecha de creación', '29/02/2024')
        self.s.line('Opción (1 a 4):', categoria)
        self.resultado('alta-' + nombre, ('Equipo registrado.',))

    def creador(self, equipo, alias, seguidores, valoracion, especialidad='1'):
        self.opcion(3)
        for prompt, valor in [('Nombre del equipo:', equipo), ('Nombre del creador:', 'Nombre ' + alias),
                              ('Alias:', alias), ('Edad', '20'), ('Seguidores', seguidores),
                              ('Valoración', valoracion), ('Opción (1 a 4):', especialidad)]:
            self.s.line(prompt, valor)
        self.resultado('alta-creador-' + alias, ('Creador registrado.',))

    def consulta(self, opcion, nombre, entradas, esperados):
        self.opcion(opcion)
        for prompt, valor in entradas:
            self.s.line(prompt, valor)
        self.resultado(nombre, esperados)

    def ejecutar(self):
        s = self.s
        s.wait('Competencia:')
        self.foto('menu-inicial')
        s.send('\x1b[A')
        self.foto('arriba-circular')
        self.comprobar('arriba-primera-a-ultima', '► 12. Salir' in self.texto(), self.texto())
        s.send('\x1b[B')
        self.foto('abajo-circular')
        self.comprobar('abajo-ultima-a-primera', '►  1. Crear competencia' in self.texto()
                       or '► 1. Crear competencia' in self.texto(), self.texto())
        for tecla in ['5', '\x1b', '\x1b[H', '\x1b[F']:
            s.send(tecla)
        self.foto('teclas-ajenas')
        self.comprobar('teclas-ajenas-no-seleccionan', 'Crear competencia' in self.texto(), self.texto())
        fila = next(i for i, linea in enumerate(s.screen.display) if '►' in linea)
        resaltadas = [x for x in range(s.columns) if s.screen.buffer[fila][x].bg == 'white']
        self.comprobar('resaltado-55-columnas', resaltadas == list(range(55)), str(resaltadas))
        for opcion in range(2, 11):
            self.opcion(opcion)
            self.resultado(f'sin-competencia-{opcion}', ('Primero tenés que crear o leer una competencia.',))
        self.consulta(11, 'leer-sin-archivos', [], ('No se pudieron leer los datos:',))

        self.opcion(1)
        s.line('Nombre de la competencia:', '   ')
        s.wait('El dato no puede estar vacío.')
        self.foto('nombre-invalido')
        s.line('Nombre de la competencia:', 'Final')
        s.line('Patrocinador:', 'Sponsor')
        for valor in ['abc', '1999', '2028']:
            s.line('Año', valor)
            self.foto('anio-invalido-' + valor)
        s.line('Año', '2026')
        for valor in ['0', '10001']:
            s.line('Máximo de equipos', valor)
            self.foto('cupo-invalido-' + valor)
        s.line('Máximo de equipos', '4')
        self.resultado('competencia-creada', ('Competencia creada.',))
        for respuesta, nombre in [('n', 'n'), ('x', 'x'), ('\r', 'enter')]:
            self.opcion(1)
            s.wait('(S/N):')
            s.send(respuesta)
            self.resultado('cancelar-crear-' + nombre)
            self.comprobar('estado-tras-cancelar-' + nombre, 'Competencia: Final' in self.texto())

        self.consulta(9, 'seguidores-sin-equipos', [], ('Todavía no hay equipos con creadores.',))
        self.consulta(5, 'equipo-inexistente', [('Nombre del equipo:', 'Ausente')], ('No se encontró el equipo.',))
        self.consulta(3, 'creador-sin-equipo', [('Nombre del equipo:', 'Ausente')], ('No se encontró el equipo.',))
        self.opcion(2)
        s.line('Nombre del equipo:', 'Alfa')
        for valor in ['1/1/2020', '31/02/2024', (date.today() + timedelta(days=1)).strftime('%d/%m/%Y')]:
            s.line('Fecha de creación', valor)
            s.wait('Fecha inválida o futura.')
            self.foto('fecha-invalida-' + valor.replace('/', '-'))
        s.line('Fecha de creación', '29/02/2024')
        for valor in ['x', '0', '5']:
            s.line('Opción (1 a 4):', valor)
            self.foto('categoria-invalida-' + valor)
        s.line('Opción (1 a 4):', '1')
        self.resultado('alta-Alfa', ('Equipo registrado.',))
        self.consulta(7, 'estadisticas-equipo-vacio', [('Nombre del equipo:', 'Alfa')],
                      ('Valoración promedio:', '0', 'Cantidad de streamers: 0', 'Total seguidores: 0', 'Sin creadores'))
        self.equipo('Beta', '2')
        self.equipo('Vacio', '3')
        self.opcion(2)
        for prompt, valor in [('Nombre del equipo:', ' ALFA '), ('Fecha de creación', '01/01/2020'),
                              ('Opción (1 a 4):', '4')]:
            s.line(prompt, valor)
        self.resultado('equipo-duplicado', ('Ya existe un equipo con ese nombre.',))
        self.equipo('Delta', '4')
        self.opcion(2)
        self.resultado('cupo-agotado', ('No quedan cupos.',))

        self.opcion(3)
        for prompt, valor in [('Nombre del equipo:', 'Alfa'), ('Nombre del creador:', 'Ana'), ('Alias:', 'ana')]:
            s.line(prompt, valor)
        for valor in ['abc', '13', '31']:
            s.line('Edad', valor)
            self.foto('edad-invalida-' + valor)
        s.line('Edad', '14')
        for valor in ['-1', '2147483648']:
            s.line('Seguidores', valor)
            self.foto('seguidores-invalidos-' + valor)
        s.line('Seguidores', '2147483647')
        for valor in ['abc', '0', '10.001']:
            s.line('Valoración', valor)
            self.foto('valoracion-invalida-' + valor)
        s.line('Valoración', '8,004')
        for valor in ['x', '0', '5']:
            s.line('Opción (1 a 4):', valor)
            self.foto('especialidad-invalida-' + valor)
        s.line('Opción (1 a 4):', '1')
        self.resultado('alta-ana', ('Creador registrado.',))
        self.creador('Alfa', 'luis', '1', '8.001', '2')
        self.creador('Beta', 'ana', '500', '8.0025', '3')
        self.creador('Delta', 'sol', '0', '8.5', '4')
        self.opcion(3)
        for prompt, valor in [('Nombre del equipo:', 'Alfa'), ('Nombre del creador:', 'Duplicado'),
                              ('Alias:', ' ANA '), ('Edad', '20'), ('Seguidores', '0'),
                              ('Valoración', '8'), ('Opción (1 a 4):', '1')]:
            s.line(prompt, valor)
        self.resultado('alias-duplicado', ('Ya existe un creador con ese alias en el equipo.',))

        self.consulta(4, 'ficha-competencia', [], ('Final', 'Sponsor', 'Cupos: 4/4', 'Alfa', 'Beta', 'Vacio', 'Delta'))
        self.consulta(5, 'ficha-equipo', [('Nombre del equipo:', ' alfa ')], ('Alfa', '29/02/2024', 'Gaming', 'ana', 'luis'))
        self.consulta(6, 'ficha-creador', [('Nombre del equipo:', 'ALFA'), ('Alias del creador:', ' ANA ')],
                      ('Ana', '2147483647', 'Streamer', 'Estrella: Sí'))
        self.consulta(6, 'alias-inexistente', [('Nombre del equipo:', 'Alfa'), ('Alias del creador:', 'ausente')],
                      ('No se encontró el creador.',))
        self.consulta(7, 'estadisticas-lleno', [('Nombre del equipo:', 'Alfa')],
                      ('Valoración promedio:', 'Cantidad de streamers: 1', 'Total seguidores: 2147483648 mil', 'Ana'))
        for primero, segundo, nombre, esperado in [
            ('Alfa', 'Beta', 'empate', '¡Empate!'), ('Alfa', 'Delta', 'gana-segundo', 'Ganador: Delta'),
            ('Delta', 'Alfa', 'gana-primero', 'Ganador: Delta'),
            ('Alfa', 'ALFA', 'mismo-equipo', 'Elegí dos equipos diferentes.'),
            ('Alfa', 'Vacio', 'lleno-vacio', 'Ambos equipos deben tener creadores.'),
            ('Vacio', 'Alfa', 'vacio-lleno', 'Ambos equipos deben tener creadores.')]:
            self.consulta(8, 'comparacion-' + nombre, [('Primer equipo:', primero), ('Segundo equipo:', segundo)], (esperado,))
        self.consulta(9, 'mas-seguidores', [], ('Alfa: 2147483648 mil seguidores.',))
        self.consulta(10, 'guardar', [], ('Datos guardados correctamente.',))
        self.comprobar('tres-csv', all((self.trabajo / 'datos/actual' / a).exists()
                       for a in ['competencia.csv', 'equipos.csv', 'creadores.csv']))
        for respuesta, nombre in [('n', 'n'), ('x', 'x'), ('\r', 'enter'), ('s', 's'), ('S', 'S')]:
            self.opcion(11)
            s.wait('(S/N):')
            self.foto('confirmacion-lectura-' + nombre)
            s.send(respuesta)
            self.resultado('lectura-' + nombre, ('Datos recuperados: 4 equipos.' if respuesta in ('s', 'S')
                                                 else 'Lectura cancelada.',))
        for respuesta in ['s', 'S']:
            self.opcion(1)
            s.wait('(S/N):')
            s.send(respuesta)
            for prompt, valor in [('Nombre de la competencia:', 'Reemplazo'), ('Patrocinador:', 'Otro'),
                                  ('Año', '2027'), ('Máximo de equipos', '1')]:
                s.line(prompt, valor)
            self.resultado('crear-reemplazo-' + respuesta, ('Competencia creada.',))
            self.consulta(4, 'estado-reemplazo-' + respuesta, [], ('Reemplazo', 'Cupos: 0/1'))
            self.opcion(11)
            s.wait('(S/N):')
            s.send(respuesta)
            self.resultado('restaurar-' + respuesta, ('Datos recuperados: 4 equipos.',))
        archivo = self.trabajo / 'datos/actual/creadores.csv'
        contenido = archivo.read_bytes()
        archivo.write_text('cabecera-invalida\n', encoding='utf-8')
        self.consulta(11, 'lectura-corrupta', [], ('No se pudieron leer los datos:',))
        archivo.write_bytes(contenido)
        self.consulta(4, 'memoria-tras-error', [], ('Final', 'Cupos: 4/4', 'Alfa', 'Beta'))
        actual = self.trabajo / 'datos/actual'
        apartado = self.trabajo / 'datos/apartado'
        actual.rename(apartado)
        actual.write_text('obstaculo', encoding='utf-8')
        self.consulta(10, 'error-guardado', [], ('No se pudieron guardar los datos:',))
        self.comprobar('obstaculo-intacto', actual.read_text(encoding='utf-8') == 'obstaculo')
        actual.unlink()
        apartado.rename(actual)
        antes = {p.name: hashlib.sha256(p.read_bytes()).hexdigest() for p in actual.glob('*.csv')}
        self.opcion(12)
        s.wait('Presioná una tecla para cerrar.')
        self.foto('top3')
        texto = self.texto()
        self.comprobar('top3-orden', all(v in texto for v in ['1. Delta', '2. Alfa', '3. Beta'])
                       and 'Vacio' not in texto, texto)
        self.comprobar('top3-fondo', s.screen.buffer[0][0].bg == 'green')
        s.send('x')
        s.drain()
        self.comprobar('cierre-proceso', not s.proc.isalive(), str(s.proc.exitstatus))
        despues = {p.name: hashlib.sha256(p.read_bytes()).hexdigest() for p in actual.glob('*.csv')}
        self.comprobar('salida-sin-guardado-automatico', antes == despues)

    def reinicio(self):
        s = captura.Capture('reinicio', ['dotnet', str(captura.ROOT /
                            'src/ACTIVIDAD ESPECIAL/bin/Debug/net8.0/ACTIVIDAD ESPECIAL.dll')], self.trabajo)
        try:
            s.wait('Competencia:')
            self.comprobar('reinicio-sin-carga-automatica', 'Competencia: Sin crear' in '\n'.join(s.screen.display))
            s.send('\x1b[A\x1b[A\r')
            s.wait('Datos recuperados: 4 equipos.')
            self.comprobar('lectura-inicial-sin-confirmar', '(S/N):' not in s.pending)
            s.snapshot('lectura-inicial')
            s.send('x')
            s.wait('Competencia: Final')
            s.send('\x1b[B\r')
            s.wait('Presioná una tecla para cerrar.')
            s.snapshot('top3-recuperado')
            self.comprobar('top3-tras-reinicio', all(v in '\n'.join(s.screen.display)
                           for v in ['1. Delta', '2. Alfa', '3. Beta']))
            s.send('x')
        finally:
            s.close()

    def referencia_original(self):
        """Contraste fresco del original, ejecutado en una copia aislada."""
        trabajo = self.trabajo / 'original'
        shutil.copytree(captura.ROOT / 'Ejecutable', trabajo)
        original = captura.Capture('original', ['dotnet', str(trabajo / 'StreamORT.dll')], trabajo)
        seleccion = 1

        def opcion(numero):
            nonlocal seleccion
            original.send('\x1b[B' * ((numero - seleccion) % 12) + '\r')
            seleccion = numero

        def guardar(nombre):
            original.wait('Presioná cualquier tecla para volver')
            original.snapshot(nombre)
            original.send('x')
            original.wait('Competencia:')

        try:
            original.wait('Competencia:')
            original.snapshot('menu-inicial')
            opcion(1)
            for prompt, valor in [('Nombre de la competencia:', 'Final'), ('Patrocinador:', 'Sponsor'),
                                  ('Año', '2026'), ('Máximo de equipos', '4')]:
                original.line(prompt, valor)
            guardar('competencia-creada')
            for nombre, categoria in [('Alfa', '1'), ('Beta', '2'), ('Vacio', '3'), ('Delta', '4')]:
                opcion(2)
                original.line('Nombre del equipo:', nombre)
                original.line('Fecha de creación', '29/02/2024')
                original.line('Opción (1 a 4):', categoria)
                guardar('alta-' + nombre)
            opcion(7)
            original.line('Nombre del equipo:', 'Alfa')
            guardar('estadisticas-equipo-vacio')
            for equipo, nombre, alias, edad, seguidores, valoracion, especialidad in [
                ('Alfa', 'Ana', 'ana', '14', '2147483647', '8,004', '1'),
                ('Alfa', 'Nombre luis', 'luis', '20', '1', '8.001', '2'),
                ('Beta', 'Nombre ana', 'ana', '20', '500', '8.0025', '3'),
                ('Delta', 'Nombre sol', 'sol', '20', '0', '8.5', '4')]:
                opcion(3)
                for prompt, valor in [('Nombre del equipo:', equipo), ('Nombre del creador:', nombre),
                                      ('Alias:', alias), ('Edad', edad), ('Seguidores', seguidores),
                                      ('Valoración', valoracion), ('Opción (1 a 4):', especialidad)]:
                    original.line(prompt, valor)
                guardar('creador-' + equipo + '-' + alias)
            for numero, nombre, entradas in [
                (4, 'ficha-competencia', []), (5, 'ficha-equipo', [('Nombre del equipo:', 'Alfa')]),
                (6, 'ficha-creador', [('Nombre del equipo:', 'Alfa'), ('Alias', 'ana')]),
                (7, 'estadisticas-lleno', [('Nombre del equipo:', 'Alfa')]),
                (8, 'comparacion-mismo-equipo', [('Primer equipo:', 'Alfa'), ('Segundo equipo:', 'ALFA')]),
                (8, 'comparacion-empate', [('Primer equipo:', 'Alfa'), ('Segundo equipo:', 'Beta')]),
                (9, 'mas-seguidores', [])]:
                opcion(numero)
                for prompt, valor in entradas:
                    original.line(prompt, valor)
                guardar(nombre)
            opcion(12)
            original.wait('Presioná una tecla para cerrar.')
            original.snapshot('top3')
            original.send('x')
        finally:
            original.close()

    def contrastar(self):
        referencia = json.loads((self.salida / 'original.json').read_text(encoding='utf-8'))['pantallas']
        # El eco de búsquedas con espacios/caja diferentes es una entrada distinta deliberada.
        def normalizar(texto):
            return '\n'.join(linea.split(':')[0] + ':' if linea.startswith(
                ('Nombre del equipo:', 'Alias del creador:')) else linea
                for linea in texto.splitlines()).rstrip()

        def estilos(pantalla):
            celdas = []
            for segmento in pantalla['segmentos']:
                fg, bg, negrita, reverso = segmento['estilo']
                estilo = [fg if fg != 'default' else 'white', bg if bg != 'default' else 'black', negrita, reverso]
                celdas.extend([estilo] * len(segmento['texto']))
            return celdas

        for nombre in ['menu-inicial', 'estadisticas-equipo-vacio', 'ficha-competencia', 'ficha-equipo',
                       'ficha-creador', 'estadisticas-lleno', 'comparacion-mismo-equipo',
                       'comparacion-empate', 'mas-seguidores', 'top3']:
            actual = next(p for p in self.s.snapshots if p['nombre'].endswith('-' + nombre))
            original = next(p for p in referencia if p['nombre'] == 'original-' + nombre)
            texto_actual = (self.salida / (actual['nombre'] + '.txt')).read_text(encoding='utf-8')
            texto_original = (self.salida / (original['nombre'] + '.txt')).read_text(encoding='utf-8')
            self.comprobar('contraste-texto-' + nombre, normalizar(texto_actual) == normalizar(texto_original))
            self.comprobar('contraste-estilos-' + nombre, estilos(actual) == estilos(original))

    def cerrar(self):
        self.s.close()
        resumen = dict(trabajo=str(self.trabajo), resultados=self.resultados,
                       correctos=sum(r['correcto'] for r in self.resultados),
                       fallos=sum(not r['correcto'] for r in self.resultados))
        (self.salida / 'resultados.json').write_text(json.dumps(resumen, ensure_ascii=False, indent=2), encoding='utf-8')
        print(json.dumps({k: v for k, v in resumen.items() if k != 'resultados'}, ensure_ascii=False), flush=True)


if __name__ == '__main__':
    sys.stdout.reconfigure(encoding='utf-8')
    (captura.ROOT / 'test/resultados/local').mkdir(parents=True, exist_ok=True)
    salida = Path(sys.argv[1]) if len(sys.argv) > 1 else captura.BASE / 'resultados/RecorridoFinal-20261104'
    salida.mkdir(parents=True, exist_ok=False)
    recorrido = Recorrido(salida)
    try:
        recorrido.ejecutar()
        recorrido.reinicio()
        recorrido.referencia_original()
        recorrido.contrastar()
    finally:
        recorrido.cerrar()
