"""Prueba la opción 11 en pseudoconsola; CSV/capturas bajo test/resultados/local (ignorado)."""
import json
import subprocess
import tempfile
from pathlib import Path

import comparar_interfaz as captura


def ejecutar():
    (captura.ROOT / 'test/resultados/local').mkdir(parents=True, exist_ok=True)
    raiz = Path(tempfile.mkdtemp(prefix='Confirmacion-', dir=captura.ROOT / 'test/resultados/local'))
    captura.OUT = raiz / 'capturas'
    captura.OUT.mkdir()
    aplicacion = captura.ROOT / 'test/resultados/local/Confirmacion/bin/ACTIVIDAD ESPECIAL/debug/ACTIVIDAD ESPECIAL.dll'
    sonda = captura.ROOT / 'test/VerificacionGuardado/bin/Debug/net8.0/VerificacionGuardado.dll'
    resultados = []
    for nombre, respuesta in [('sin-previa', None), ('minuscula', 's'), ('mayuscula', 'S'),
                               ('no', 'n'), ('otra', 'x'), ('enter', '\r')]:
        trabajo = raiz / nombre
        subprocess.run(['dotnet', '--roll-forward', 'Major', str(sonda),
                        'guardar-prueba', str(trabajo / 'datos'), 'completa'], check=True)
        sesion = captura.Capture(nombre, ['dotnet', '--roll-forward', 'Major', str(aplicacion)], trabajo)
        seleccion = 1

        def opcion(numero):
            nonlocal seleccion
            sesion.send('\x1b[B' * ((numero - seleccion) % 12) + '\r')
            seleccion = numero

        def volver():
            sesion.wait('Presioná cualquier tecla para volver')
            sesion.send('x')
            sesion.wait('Competencia:')

        try:
            sesion.wait('Competencia:')
            if respuesta is not None:
                opcion(1)
                sesion.line('Nombre de la competencia:', 'Memoria')
                sesion.line('Patrocinador:', 'Sponsor anterior')
                sesion.line('Año', '2026')
                sesion.line('Máximo de equipos', '2')
                volver()
                opcion(2)
                sesion.line('Nombre del equipo:', 'Anterior')
                sesion.line('Fecha de creación', '01/01/2020')
                sesion.line('Opción (1 a 4):', '1')
                volver()
            opcion(11)
            if respuesta is not None:
                sesion.wait('(S/N):')
                sesion.snapshot('confirmacion')
                sesion.send(respuesta)
            aceptada = respuesta in (None, 's', 'S')
            sesion.wait('Datos recuperados: 4 equipos.' if aceptada else 'Lectura cancelada.')
            if respuesta is None and '(S/N):' in sesion.pending:
                raise AssertionError('Se pidió confirmación sin competencia previa.')
            sesion.snapshot('resultado')
            volver()
            opcion(4)
            sesion.wait('Presioná cualquier tecla para volver')
            sesion.snapshot('estado')
            texto = '\n'.join(sesion.screen.display)
            if aceptada:
                assert 'Batalla ñ, "2026"' in texto and 'Vacío' in texto, texto
                assert 'Sponsor anterior' not in texto and 'Anterior' not in texto, texto
            else:
                assert 'Memoria' in texto and 'Sponsor anterior' in texto and 'Anterior' in texto, texto
                assert 'Batalla ñ, "2026"' not in texto and 'Vacío' not in texto, texto
            volver()
            opcion(12)
            sesion.wait('Presioná una tecla para cerrar.')
            sesion.send('x')
            resultados.append({'caso': nombre, 'reemplazo': aceptada, 'resultado': 'correcto'})
        finally:
            sesion.close()
    (raiz / 'resultados.json').write_text(json.dumps(resultados, ensure_ascii=False, indent=2), encoding='utf-8')
    print(json.dumps({'resultados': resultados, 'evidencia': str(raiz)}, ensure_ascii=False), flush=True)


if __name__ == '__main__':
    ejecutar()
