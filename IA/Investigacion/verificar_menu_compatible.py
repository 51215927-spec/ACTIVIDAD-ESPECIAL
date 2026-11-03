"""Sesiones de caja negra del menú compatible; todo el material auxiliar queda en IA."""
import hashlib
import json
import shutil
import subprocess
import sys
from pathlib import Path

import comparar_interfaz as captura

BASE = Path(__file__).resolve().parent
WORK = captura.ROOT / 'IA/Verificacion/MenuCompatible'
captura.OUT = BASE / 'MenuCompatible-20261103'
captura.OUT.mkdir(exist_ok=True)
WORK.mkdir(parents=True, exist_ok=True)
SONDA = WORK / 'CompilacionSonda/bin/SondaMenuCompatible/debug/SondaMenuCompatible.dll'


def cells(snapshot):
    rows = {}
    for run in snapshot['segmentos']:
        fg, bg, _, reverse = run['estilo']
        fg = 'white' if fg == 'default' else fg
        bg = 'black' if bg == 'default' else bg
        if reverse:
            fg, bg = bg, fg
        for dx, char in enumerate(run['texto']):
            rows[run['fila'], run['columna'] + dx] = (char, fg if char.strip() else '', bg)
    return rows


def baseline(file, name):
    data = json.loads((BASE / 'Interfaz-20261103' / file).read_text(encoding='utf-8'))
    return next(s for s in data['pantallas'] if s['nombre'] == name)


def equal_screens(expected, actual):
    a, b = cells(expected), cells(actual)
    different = [(pos, a.get(pos), b.get(pos)) for pos in a.keys() | b.keys() if a.get(pos) != b.get(pos)]
    assert not different, f'{actual["nombre"]}: {len(different)} celdas diferentes; {different[:6]}'


def original_numeric():
    target = WORK / 'Original'
    if target.exists():
        raise RuntimeError('La copia de trabajo ya existe; no se sobrescribe.')
    source = captura.ROOT / 'Ejecutable'
    before = {str(p.relative_to(source)): hashlib.sha256(p.read_bytes()).hexdigest()
              for p in source.rglob('*') if p.is_file()}
    shutil.copytree(source, target)
    s = captura.Capture('original-numerico', [str(captura.DOTNET8), str(target / 'StreamORT.dll')], target)
    try:
        s.wait('Competencia:')
        s.send('\r')
        s.line('Nombre de la competencia:', 'Visual')
        s.line('Patrocinador:', 'Sponsor')
        s.line('Año', '2027')
        s.line('Máximo de equipos', '4')
        s.wait('Presioná cualquier tecla para volver')
        s.send('x')
        s.wait('Competencia:')
        s.send('\x1b[B\r')
        s.line('Nombre del equipo:', 'Alpha')
        s.line('Fecha de creación', '01/01/2000')
        s.wait('Opción (1 a 4):')
        s.snapshot('categoria-inicial')
        for name, value in [('cero', '0'), ('cinco', '5'), ('letra', 'x'), ('vacio', '')]:
            s.send(value + '\r')
            s.wait('Opción (1 a 4):')
            s.snapshot('categoria-' + name)
        s.send('1\r')
        s.wait('Presioná cualquier tecla para volver')
        s.send('x')
        s.wait('Competencia:')
        s.send('\x1b[B\r')
        s.line('Nombre del equipo:', 'Alpha')
        s.line('Nombre del creador:', 'Ana')
        s.line('Alias:', 'ana')
        s.line('Edad', '20')
        s.line('Seguidores', '500')
        s.line('Valoración', '8,5')
        s.wait('Opción (1 a 4):')
        s.snapshot('especialidad-inicial')
        for name, value in [('cero', '0'), ('letra', 'x')]:
            s.send(value + '\r')
            s.wait('Opción (1 a 4):')
            s.snapshot('especialidad-' + name)
        s.send('1\r')
        s.wait('Creador registrado.')
    finally:
        s.close()
    after = {str(p.relative_to(source)): hashlib.sha256(p.read_bytes()).hexdigest()
             for p in source.rglob('*') if p.is_file()}
    assert before == after
    (captura.OUT / 'integridad-original.json').write_text(json.dumps(before, indent=2), encoding='utf-8')


def probe():
    results = {}
    for width, height, file, name in [
        (120, 40, 'original.json', 'original-01-menu-inicial'),
        (80, 24, 'original-80x24.json', 'original-80x24-menu'),
        (160, 40, 'original-160x40.json', 'original-160x40-menu')]:
        s = captura.Capture(f'sonda-{width}x{height}', [str(captura.DOTNET8), str(SONDA), 'flechas'], WORK, width, height)
        try:
            s.wait('Competencia:')
            s.snapshot('inicial')
            equal_screens(baseline(file, name), s.snapshots[-1])
            if width == 120:
                s.send('\x1b[A')
                s.snapshot('arriba-desde-primera')
                equal_screens(baseline('original.json', 'original-02-arriba-desde-primera'), s.snapshots[-1])
                s.send('\x1b[B')
                s.snapshot('abajo-desde-ultima')
                equal_screens(baseline('original.json', 'original-03-abajo-desde-ultima'), s.snapshots[-1])
                s.send('\x1b[B' * 4)
                s.snapshot('quinta')
                selected = cells(s.snapshots[-1])
                for key in ['\x1b[H', '\x1b[F', '\x1b[5~', '\x1b[6~', '5', '\x1b']:
                    s.send(key)
                    # No es necesario guardar seis imágenes iguales.
                    current = dict(segmentos=[dict(fila=y, columna=x, estilo=[c.fg, c.bg, c.bold, c.reverse], texto=c.data)
                                              for y, row in s.screen.buffer.items() for x, c in row.items()])
                    assert cells(current) == selected
                s.send('\r')
                s.wait('RESULTADO=4;REGISTRO=True')
                s.send('x')
                s.wait('Competencia:')
                s.snapshot('seleccion-conservada')
                equal_screens(s.snapshots[-2], s.snapshots[-1])
            results[f'menu-{width}x{height}'] = 'Celdas visibles y colores coinciden con el original.'
        finally:
            s.close()

    original_data = json.loads((captura.OUT / 'original-numerico.json').read_text(encoding='utf-8'))
    for mode in ['categoria', 'especialidad']:
        s = captura.Capture('sonda-' + mode, [str(captura.DOTNET8), str(SONDA), mode], WORK)
        try:
            s.wait('Opción (1 a 4):')
            s.snapshot('inicial')
            cases = [('cero', '0'), ('cinco', '5'), ('letra', 'x'), ('vacio', '')] if mode == 'categoria' else [('cero', '0'), ('letra', 'x')]
            for name, value in cases:
                s.send(value + '\r')
                s.wait('Opción (1 a 4):')
                s.snapshot(name)
            for shot in s.snapshots:
                suffix = shot['nombre'].removeprefix('sonda-')
                reference = next(p for p in original_data['pantallas'] if p['nombre'] == 'original-numerico-' + suffix)
                title = 'Categoría:' if mode == 'categoria' else 'Especialidad:'
                def section(screen):
                    matrix = cells(screen)
                    line = next(r['fila'] for r in screen['segmentos'] if title in r['texto'])
                    last = max(y for (y, x), (ch, fg, bg) in matrix.items() if ch.strip())
                    return {(y-line, x): c for (y, x), c in matrix.items() if line <= y <= last}
                assert section(reference) == section(shot), suffix
            s.send('1\r')
            s.wait('RESULTADO=0;REGISTRO=True')
            results[mode] = 'Lista, prompt y repreguntas coinciden; el contenido anterior se conserva.'
        finally:
            s.close()

    for mode in ['largo', 'sin-ciclo', 'sin-numeros', 'sin-registro']:
        s = captura.Capture('sonda-' + mode, [str(captura.DOTNET8), str(SONDA), mode], WORK, 80, 24)
        try:
            s.wait('Competencia:')
            if mode == 'largo':
                s.send('\x1b[A')
                s.snapshot('ultima-visible')
                assert any('80. Opción larga 80' in run['texto'] for run in s.snapshots[-1]['segmentos'])
                expected = 79
            elif mode == 'sin-ciclo':
                s.send('\x1b[A')
                expected = 0
            else:
                expected = 0
            s.send('\r')
            s.wait(f'RESULTADO={expected};REGISTRO={"False" if mode == "sin-registro" else "True"}')
            s.snapshot('resultado')
            results[mode] = 'Correcto.'
        finally:
            s.close()

    for mode, data in [('validaciones', ''), ('categoria', 'x\n5\n2\n')]:
        result = subprocess.run([str(captura.DOTNET8), str(SONDA), mode], input=data, text=True,
                                encoding='utf-8', capture_output=True, timeout=10)
        assert result.returncode == 0, result.stderr
        expected = 'VALIDACIONES=5' if mode == 'validaciones' else 'RESULTADO=1;REGISTRO=True'
        assert expected in result.stdout
        results['redirigido-' + mode] = result.stdout
    eof = subprocess.run([str(captura.DOTNET8), str(SONDA), 'categoria'], input='', text=True,
                         encoding='utf-8', capture_output=True, timeout=10)
    assert eof.returncode != 0 and 'EndOfStreamException' in eof.stderr
    results['entrada-cerrada'] = 'Finaliza con EndOfStreamException; no repite indefinidamente.'
    (captura.OUT / 'resultado-sonda.json').write_text(json.dumps(results, ensure_ascii=False, indent=2), encoding='utf-8')


def application():
    dll = WORK / 'CompilacionAplicacion/bin/ACTIVIDAD ESPECIAL/debug/ACTIVIDAD ESPECIAL.dll'
    results = {}
    for width, height, file, name in [
        (120, 40, 'original.json', 'original-01-menu-inicial'),
        (80, 24, 'original-80x24.json', 'original-80x24-menu'),
        (160, 40, 'original-160x40.json', 'original-160x40-menu')]:
        s = captura.Capture(f'aplicacion-{width}x{height}', [str(captura.DOTNET8), str(dll)], WORK, width, height)
        try:
            s.wait('Competencia:')
            s.snapshot('inicial')
            equal_screens(baseline(file, name), s.snapshots[-1])
            if width == 120:
                s.send('\x1b[A')
                s.snapshot('arriba-desde-primera')
                equal_screens(baseline('original.json', 'original-02-arriba-desde-primera'), s.snapshots[-1])
                s.send('\x1b[B')
                s.snapshot('abajo-desde-ultima')
                equal_screens(baseline('original.json', 'original-03-abajo-desde-ultima'), s.snapshots[-1])
                s.send('\x1b[B' * 4)
                s.snapshot('quinta')
                s.send('\r')
                s.wait('Primero tenés')
                s.snapshot('operacion-sin-menu-residual')
                assert not any('BATALLA DE CREADORES' in r['texto'] for r in s.snapshots[-1]['segmentos'])
                s.send('x')
                s.wait('Competencia:')
                s.snapshot('seleccion-conservada')
                equal_screens(s.snapshots[-3], s.snapshots[-1])
            results[f'aplicacion-{width}x{height}'] = 'Menú idéntico al original en texto y colores lógicos.'
        finally:
            s.close()
    (captura.OUT / 'resultado-aplicacion.json').write_text(json.dumps(results, ensure_ascii=False, indent=2), encoding='utf-8')


if __name__ == '__main__':
    sys.stdout.reconfigure(encoding='utf-8')
    if '--original' in sys.argv:
        original_numeric()
    elif '--sonda' in sys.argv:
        probe()
    elif '--aplicacion' in sys.argv:
        application()
