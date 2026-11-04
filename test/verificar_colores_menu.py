"""Comparación interactiva de ambas sobrecargas; no reemplaza las pruebas unitarias finales."""
import json
from pathlib import Path

import comparar_interfaz as captura

BASE = Path(__file__).resolve().parent
captura.OUT = BASE / 'resultados/ColoresMenu-20261103'
captura.OUT.mkdir(exist_ok=True)
DLL = captura.ROOT / 'test/resultados/local/ColoresMenu/CompilacionSonda/bin/SondaColoresMenu/debug/SondaColoresMenu.dll'
results = {}

for palette in ['predeterminados', 'personalizados']:
    screens = {}
    for mode in ['anterior', 'nuevo']:
        name = f'{mode}-{palette}'
        session = captura.Capture(name, [str(captura.DOTNET8), str(DLL), mode, palette], captura.ROOT)
        try:
            session.wait('Comparación de colores')
            session.snapshot('inicio')
            session.send('\x1b[B')
            session.snapshot('segunda')
            session.send('\r')
            session.wait('RESULTADO=1;COLORES_RESTAURADOS=True')
            session.snapshot('confirmado')
            screens[mode] = [{key: value for key, value in screen.items() if key != 'nombre'}
                             for screen in session.snapshots]
        finally:
            session.close()
    assert screens['anterior'] == screens['nuevo'], f'Diferencia visual: {palette}'
    results[palette] = 'Tres pantallas iguales, índice 1 y colores restaurados en ambas formas.'

(captura.OUT / 'resultado.json').write_text(json.dumps(results, ensure_ascii=False, indent=2), encoding='utf-8')
print('Ambas sobrecargas equivalentes en las dos paletas; selección y restauración correctas.')
