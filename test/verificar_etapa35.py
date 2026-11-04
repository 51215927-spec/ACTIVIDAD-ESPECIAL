"""Repite el contraste del código actual sin sobrescribir la evidencia anterior."""
import json
import shutil
import sys

import verificar_menu_compatible as menu

sys.stdout.reconfigure(encoding='utf-8')
menu.WORK = menu.captura.ROOT / 'test/resultados/local/VerificacionEtapa35'
menu.SONDA = menu.WORK / 'CompilacionSonda/bin/SondaMenuCompatible/debug/SondaMenuCompatible.dll'
menu.captura.OUT = menu.BASE / 'resultados/VerificacionEtapa35-20261103'
menu.captura.OUT.mkdir(exist_ok=True)

# Referencia ya capturada del original: solo se lee, no se reejecutan ni modifican sus datos.
shutil.copyfile(menu.BASE / 'resultados/MenuCompatible-20261103/original-numerico.json',
                menu.captura.OUT / 'original-numerico.json')
menu.probe()
menu.application()

result = {
    'referencia': 'Capturas previas del original; no se ejecutó otra vez el original.',
    'sonda': json.loads((menu.captura.OUT / 'resultado-sonda.json').read_text(encoding='utf-8')),
    'aplicacion': json.loads((menu.captura.OUT / 'resultado-aplicacion.json').read_text(encoding='utf-8')),
    'alcance': 'Menú principal y capacidades de biblioteca ya implementados; flujos de etapa 4 pendientes.'
}
(menu.captura.OUT / 'resultado.json').write_text(json.dumps(result, ensure_ascii=False, indent=2), encoding='utf-8')
print('Verificación de etapa 3.5 correcta en el alcance implementado.')
