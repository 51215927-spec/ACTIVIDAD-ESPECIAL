"""Capturas de caja negra para etapa 3.5; no modifica la aplicación ni sus bibliotecas.

Requiere herramientas locales: pywinpty 3.1.2, pyte 0.8.2, Pillow 12.0.0.
Las imágenes reconstruyen las celdas ANSI; la paleta y la fuente se indican en el informe.
Solo escribe en test/resultados/local/ComparacionInterfaz y en test/Interfaz-20261103.
"""
import hashlib
import json
import select
import shutil
import sys
import time
from pathlib import Path

BASE = Path(__file__).resolve().parent
ROOT = BASE.parent
sys.path.insert(0, str(BASE / 'herramientas'))
from winpty import PtyProcess
import pyte
from PIL import Image, ImageDraw, ImageFont

WORK = ROOT / 'test/resultados/local/ComparacionInterfaz'
OUT = BASE / 'resultados/Interfaz-20261103'
DOTNET8 = BASE / 'herramientas/dotnet8/dotnet.exe'
COLS, ROWS = 120, 40
PALETTE = dict(zip(
    ['black', 'red', 'green', 'brown', 'blue', 'magenta', 'cyan', 'white',
     'brightblack', 'brightred', 'brightgreen', 'brightbrown', 'brightblue',
     'brightmagenta', 'brightcyan', 'brightwhite'],
    ['#000000', '#800000', '#008000', '#808000', '#000080', '#800080', '#008080', '#c0c0c0',
     '#808080', '#ff0000', '#00ff00', '#ffff00', '#0000ff', '#ff00ff', '#00ffff', '#ffffff']))


class Capture:
    def __init__(self, name, command, cwd, columns=COLS, rows=ROWS):
        self.name = name
        self.columns, self.rows = columns, rows
        self.screen = pyte.Screen(columns, rows)
        self.stream = pyte.Stream(self.screen)
        self.raw = ''
        self.pending = ''
        self.inputs = []
        self.snapshots = []
        self.proc = PtyProcess.spawn(command, cwd=str(cwd), dimensions=(rows, columns))

    def drain(self, duration=.20):
        end = time.monotonic() + duration
        while time.monotonic() < end:
            if select.select([self.proc.fileobj], [], [], .02)[0]:
                try:
                    value = self.proc.read(65536)
                except EOFError:
                    break
                self.raw += value
                self.pending += value
                self.stream.feed(value)

    def wait(self, text):
        end = time.monotonic() + 8
        while text not in self.pending:
            self.drain(.05)
            if time.monotonic() > end:
                raise TimeoutError(f'{self.name}: falta {text!r}; {self.pending[-600:]!r}')
        self.drain()

    def send(self, value):
        self.pending = ''
        self.inputs.append(value)
        self.proc.write(value)
        self.drain()

    def line(self, prompt, value):
        self.wait(prompt)
        self.send(str(value) + '\r')

    def snapshot(self, name):
        self.drain()
        key = f'{self.name}-{name}'
        lines = [line.rstrip() for line in self.screen.display]
        (OUT / f'{key}.txt').write_text('\n'.join(lines).rstrip() + '\n', encoding='utf-8')
        runs = []
        font = ImageFont.truetype('C:/Windows/Fonts/consola.ttf', 16)
        image = Image.new('RGB', (self.columns * 10, self.rows * 20), '#000000')
        draw = ImageDraw.Draw(image)
        for y in range(self.rows):
            for x in range(self.columns):
                cell = self.screen.buffer[y][x]
                fg, bg = cell.fg, cell.bg
                if cell.reverse:
                    fg, bg = bg, fg
                fgcolor = PALETTE.get(fg, '#c0c0c0' if fg == 'default' else '#' + fg)
                bgcolor = PALETTE.get(bg, '#000000' if bg == 'default' else '#' + bg)
                draw.rectangle((x*10, y*20, x*10+9, y*20+19), fill=bgcolor)
                draw.text((x*10, y*20-1), cell.data, font=font, fill=fgcolor)
                signature = [cell.fg, cell.bg, cell.bold, cell.reverse]
                if runs and runs[-1]['fila'] == y and runs[-1]['estilo'] == signature:
                    runs[-1]['texto'] += cell.data
                else:
                    runs.append(dict(fila=y, columna=x, estilo=signature, texto=cell.data))
        image.save(OUT / f'{key}.png')
        snapshot = dict(nombre=key, cursor=[self.screen.cursor.x, self.screen.cursor.y],
                        cursorOculto=self.screen.cursor.hidden, segmentos=runs)
        self.snapshots.append(snapshot)
        print(key, flush=True)

    def close(self):
        self.drain()
        self.proc.close(force=True)
        (OUT / f'{self.name}.ansi.txt').write_text(self.raw, encoding='utf-8')
        (OUT / f'{self.name}.json').write_text(json.dumps(dict(
            dimensiones=[self.columns, self.rows], entradas=self.inputs, pantallas=self.snapshots),
            ensure_ascii=False, indent=2), encoding='utf-8')


def original():
    copy = WORK / 'Original'
    if copy.exists():
        raise RuntimeError('La copia Original ya existe; conservar evidencia y usar otra carpeta.')
    shutil.copytree(ROOT / 'Ejecutable', copy)
    s = Capture('original', [str(DOTNET8), str(copy / 'StreamORT.dll')], copy)
    selected = 1

    def option(number):
        nonlocal selected
        s.send('\x1b[B' * ((number - selected) % 12) + '\r')
        selected = number

    def back():
        s.wait('Presioná cualquier tecla para volver')
        s.send('x')
        s.wait('Competencia:')

    try:
        s.wait('Competencia:')
        s.snapshot('01-menu-inicial')
        s.send('\x1b[A')
        s.snapshot('02-arriba-desde-primera')
        s.send('\x1b[B')
        s.snapshot('03-abajo-desde-ultima')
        for name, key in [('numero', '5'), ('home', '\x1b[H'), ('end', '\x1b[F'), ('escape', '\x1b')]:
            s.send(key)
            s.snapshot('tecla-' + name)
        s.send('\r')
        s.wait('Nombre de la competencia:')
        s.snapshot('04-crear-competencia')
        s.line('Nombre de la competencia:', 'Visual')
        s.line('Patrocinador:', 'Sponsor')
        s.line('Año', '1999')
        s.snapshot('05-error-anio')
        s.line('Año', '2027')
        s.line('Máximo de equipos', '4')
        s.wait('Presioná cualquier tecla para volver')
        s.snapshot('06-competencia-creada')
        back()
        s.snapshot('07-menu-con-competencia')
        option(2)
        s.line('Nombre del equipo:', 'Alpha')
        s.line('Fecha de creación', '01/01/2000')
        s.wait('Opción (1 a 4):')
        s.snapshot('08-categorias')
        s.send('1\r')
        back()
        s.snapshot('09-menu-conserva-seleccion')
        option(3)
        s.line('Nombre del equipo:', 'Alpha')
        s.line('Nombre del creador:', 'Ana')
        s.line('Alias:', 'ana')
        s.line('Edad', '20')
        s.line('Seguidores', '500')
        s.line('Valoración', '8,5')
        s.wait('Opción (1 a 4):')
        s.snapshot('10-especialidades')
        s.send('1\r')
        back()
        for number, name, prompts in [
            (4, '11-competencia', []), (5, '12-equipo', [('Nombre del equipo:', 'Alpha')]),
            (6, '13-creador', [('Nombre del equipo:', 'Alpha'), ('Alias', 'ana')]),
            (7, '14-estadisticas', [('Nombre del equipo:', 'Alpha')]),
            (9, '15-mas-seguidores', []), (10, '16-guardar', [])]:
            option(number)
            for prompt, value in prompts:
                s.line(prompt, value)
            s.wait('Presioná cualquier tecla para volver')
            s.snapshot(name)
            back()
        option(11)
        s.wait('(S/N):')
        s.snapshot('17-confirmar-lectura')
        s.send('n')
        back()
        option(1)
        s.wait('(S/N):')
        s.snapshot('18-confirmar-reemplazo')
        s.send('n')
        back()
        option(12)
        s.wait('TOP')
        s.snapshot('19-top3')
    finally:
        s.close()


def current_app():
    dll = WORK / 'CompilacionAplicacion/bin/ACTIVIDAD ESPECIAL/debug/ACTIVIDAD ESPECIAL.dll'
    s = Capture('actual', [str(DOTNET8), str(dll)], WORK)
    try:
        s.wait('Para desplazarse:')
        s.snapshot('01-menu-inicial')
        s.send('\x1b[A')
        s.snapshot('02-arriba-desde-primera')
        s.send('\x1b[B')
        s.snapshot('03-segunda')
        s.send('\r')
        s.wait('Primero tenés')
        s.send('x')
        s.wait('Para desplazarse:')
        s.snapshot('04-menu-tras-volver')
        s.send('\x1b[F')
        s.snapshot('05-end')
        s.send('\x1b[B')
        s.snapshot('06-abajo-desde-ultima')
    finally:
        s.close()


def library_probes():
    dll = WORK / 'CompilacionSonda/bin/SondaInterfaz/debug/SondaInterfaz.dll'
    for mode, expected in [('categoria', 'Para desplazarse:'), ('tabla', 'Gaming'), ('mensajes', 'Equipo registrado.')]:
        s = Capture('utilidades-' + mode, [str(DOTNET8), str(dll), mode], WORK)
        try:
            s.wait(expected)
            s.snapshot('01')
        finally:
            s.close()


def extra_original():
    copy = WORK / 'OriginalExtra'
    if copy.exists():
        raise RuntimeError('OriginalExtra ya existe; no se sobrescribe.')
    shutil.copytree(ROOT / 'Ejecutable', copy)
    for width, height in [(80, 24), (160, 40)]:
        s = Capture(f'original-{width}x{height}', [str(DOTNET8), str(copy / 'StreamORT.dll')], copy, width, height)
        try:
            s.wait('Competencia:')
            s.snapshot('menu')
        finally:
            s.close()
    s = Capture('original-extra', [str(DOTNET8), str(copy / 'StreamORT.dll')], copy)
    try:
        s.wait('Competencia:')
        s.send('\x1b[B' * 4)
        s.snapshot('01-quinta')
        for name, key in [('home', '\x1b[H'), ('end', '\x1b[F'), ('pageup', '\x1b[5~'), ('pagedown', '\x1b[6~')]:
            s.send(key)
            s.snapshot(name)
        s.send('\r')
        s.wait('Primero tenés')
        s.snapshot('02-sin-competencia-azul')
        s.send('x')
        s.wait('Competencia:')
        s.send('\x1b[B' * 3 + '\r')
        s.wait('Primero tenés')
        s.snapshot('03-comparar-azul')
        s.send('x')
        s.wait('Competencia:')
        s.snapshot('04-vuelta-fondo-negro')
        s.send('\x1b[B' * 4 + '\r')
        s.wait('TOP')
        s.snapshot('05-top-vacio')
    finally:
        s.close()


def summarize():
    all_screens = {}
    for path in OUT.glob('*.json'):
        if path.name in ('integridad.json', 'resumen.json'):
            continue
        data = json.loads(path.read_text(encoding='utf-8'))
        all_screens.update({s['nombre']: s for s in data['pantallas']})

    def highlights(name):
        return [dict(fila=r['fila'], columna=r['columna'], ancho=len(r['texto']), texto=r['texto'].strip(), estilo=r['estilo'])
                for r in all_screens[name]['segmentos'] if r['estilo'][1] in ('white', 'brightwhite')]

    observations = {name: highlights(name) for name in all_screens
                    if 'menu' in name or 'desde' in name or 'tecla' in name or 'extra' in name or 'end' in name}
    assert '12. Salir' in highlights('original-02-arriba-desde-primera')[0]['texto']
    assert '1. Crear competencia' in highlights('original-03-abajo-desde-ultima')[0]['texto']
    assert '2. Registrar equipo' in highlights('original-09-menu-conserva-seleccion')[0]['texto']
    for name in ['home', 'end', 'pageup', 'pagedown']:
        assert '5. Mostrar' in highlights('original-extra-' + name)[0]['texto']
    for name in ['original-01-menu-inicial', 'original-80x24-menu', 'original-160x40-menu']:
        assert highlights(name)[0]['ancho'] == 55
    assert len(highlights('actual-04-menu-tras-volver')) == 2
    for name, bg in [('original-08-categorias', 'blue'), ('original-10-especialidades', 'magenta'),
                     ('original-19-top3', 'green'), ('original-extra-05-top-vacio', 'green')]:
        assert all_screens[name]['segmentos'][0]['estilo'][1] == bg
    (OUT / 'resumen.json').write_text(json.dumps(dict(
        pantallas=len(all_screens), observaciones=observations,
        comprobaciones='Circularidad, selección, teclas auxiliares, ancho, historial y fondos comprobados.'),
        ensure_ascii=False, indent=2), encoding='utf-8')
    print(f'{len(all_screens)} pantallas; comprobaciones de evidencia correctas.')


if __name__ == '__main__':
    sys.stdout.reconfigure(encoding='utf-8')
    WORK.mkdir(parents=True, exist_ok=True)
    OUT.mkdir(parents=True, exist_ok=True)
    before = {str(p.relative_to(ROOT)): hashlib.sha256(p.read_bytes()).hexdigest()
              for p in (ROOT / 'Ejecutable').rglob('*') if p.is_file()}
    if '--resumen' in sys.argv:
        summarize()
    elif '--actual' in sys.argv:
        current_app()
    elif '--sonda' in sys.argv:
        library_probes()
    elif '--extra' in sys.argv:
        extra_original()
    else:
        original()
        current_app()
    after = {str(p.relative_to(ROOT)): hashlib.sha256(p.read_bytes()).hexdigest()
             for p in (ROOT / 'Ejecutable').rglob('*') if p.is_file()}
    assert before == after, 'El ejecutable original o sus datos cambiaron.'
    (OUT / 'integridad.json').write_text(json.dumps(before, indent=2), encoding='utf-8')
