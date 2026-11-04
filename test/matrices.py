"""Matrices adicionales; usa exclusivamente la copia local del ejecutable."""
import sys
import stat
from datetime import date, timedelta
from pathlib import Path
import probar_original as p


def rechazos(s, prompt, valores, error):
    for valor in valores:
        s.linea(prompt, valor)
        s.esperar(error)


def registrar_equipo(s, nombre, fechas=None, categoria='1'):
    s.opcion(2)
    s.linea('Nombre del equipo:', nombre)
    if fechas:
        rechazos(s, 'Fecha de creación', fechas[:-1], 'Fecha inválida o futura.')
        s.linea('Fecha de creación', fechas[-1])
    else:
        s.linea('Fecha de creación', '01/01/2000')
    s.linea('Opción (1 a 4):', categoria)
    return s.volver()


def entradas(s):
    salida = {'hoy': str(date.today())}
    s.opcion(1)
    rechazos(s, 'Nombre de la competencia:', ['', '   ', '\t'], 'El dato no puede estar vacío.')
    s.linea('Nombre de la competencia:', '  Copa  con  espacios  ')
    rechazos(s, 'Patrocinador:', ['', '   '], 'El dato no puede estar vacío.')
    s.linea('Patrocinador:', '  Sponsor  doble  ')
    rechazos(s, 'Año', ['abc', '1999', '2028', '2147483648', '2.000'], 'Ingresá un número entero válido.')
    s.linea('Año', 2000)
    rechazos(s, 'Máximo de equipos', ['0', '10001', '-1', 'abc'], 'Ingresá un número entero válido.')
    s.linea('Máximo de equipos', 10000)
    salida['competencia'] = s.volver()
    s.opcion(2)
    rechazos(s, 'Nombre del equipo:', ['', '   '], 'El dato no puede estar vacío.')
    s.linea('Nombre del equipo:', '  Águila  Azul  ')
    rechazos(s, 'Fecha de creación', ['31/02/2020', '29/02/2023', '1/1/2000', '2000-01-01', (date.today() + timedelta(days=1)).strftime('%d/%m/%Y')], 'Fecha inválida o futura.')
    s.linea('Fecha de creación', '29/02/2024')
    rechazos(s, 'Opción (1 a 4):', ['0', '5', 'x'], 'Ingresá un número entero válido.')
    s.linea('Opción (1 a 4):', '3')
    salida['equipo_bisiesto'] = s.volver()
    salida['equipo_hoy'] = registrar_equipo(s, 'Hoy', [date.today().strftime('%d/%m/%Y')])
    s.opcion(3)
    s.linea('Nombre del equipo:', '  águila  azul  ')
    rechazos(s, 'Nombre del creador:', ['', '   '], 'El dato no puede estar vacío.')
    s.linea('Nombre del creador:', '  Nombre  doble  ')
    rechazos(s, 'Alias:', ['', '   '], 'El dato no puede estar vacío.')
    s.linea('Alias:', '  Álias  doble  ')
    rechazos(s, 'Edad', ['13', '31', 'abc'], 'Ingresá un número entero válido.')
    s.linea('Edad', 30)
    rechazos(s, 'Seguidores', ['-1', '2147483648', '1,5'], 'Ingresá un número entero válido.')
    s.linea('Seguidores', 2147483647)
    rechazos(s, 'Valoración', ['0', '11', 'abc'], 'Ingresá un decimal válido')
    s.linea('Valoración', '8,5')
    rechazos(s, 'Opción (1 a 4):', ['0', '5'], 'Ingresá un número entero válido.')
    s.linea('Opción (1 a 4):', 4)
    salida['creador'] = s.volver()
    salida['info'] = s.info()
    s.opcion(10)
    salida['guardar_decimal'] = s.volver()
    salida['archivo_decimal'] = (p.DATOS / 'creadores.txt').read_text(encoding='utf-8')
    for equipo, alias in [('ÁGUILA  AZUL', 'áLIAS  DOBLE'), ('Aguila  Azul', None), ('A\u0301guila  Azul', None), ('Águila Azul', None), ('Águila  Azul', 'Alias  doble'), ('Águila  Azul', 'A\u0301lias  doble')]:
        s.opcion(6)
        s.linea('Nombre del equipo:', equipo)
        if alias is not None:
            s.linea('Alias del creador:', alias)
        salida[f'buscar_{equipo}_{alias}'] = s.volver()
    salida['equipo_largo_256'] = registrar_equipo(s, 'L' * 256)
    s.opcion(3)
    s.linea('Nombre del equipo:', 'L' * 256)
    s.linea('Nombre del creador:', 'N' * 256)
    s.linea('Alias:', 'A' * 256)
    s.linea('Edad', 14)
    s.linea('Seguidores', 0)
    s.linea('Valoración', 1)
    s.linea('Opción (1 a 4):', 1)
    salida['creador_largo_256'] = s.volver()
    s.crear_competencia('C' * 256, 's')
    salida['competencia_larga_256'] = s.info()
    return salida


def persistencia(s):
    salida = {}
    alteraciones = {
        'competencia_vacia': lambda: p.escribir('competencia.txt', []),
        'equipos_vacio': lambda: p.escribir('equipos.txt', []),
        'creadores_vacio': lambda: p.escribir('creadores.txt', []),
        'creadores_ausente': lambda: (p.DATOS / 'creadores.txt').unlink(),
        'competencia_columnas': lambda: p.escribir('competencia.txt', [p.b64('Incompleta')]),
        'equipo_columnas': lambda: p.escribir('equipos.txt', [p.b64('Incompleto')]),
        'creador_columnas': lambda: p.escribir('creadores.txt', [p.b64('Alpha')]),
        'anio_no_numero': lambda: p.escribir('competencia.txt', [f'{p.b64("Otra")};{p.b64("S")};abc;10']),
        'anio_fuera_rango': lambda: p.escribir('competencia.txt', [f'{p.b64("Otra")};{p.b64("S")};1999;10']),
        'cupo_menor_que_equipos': lambda: p.escribir('competencia.txt', [f'{p.b64("Otra")};{p.b64("S")};2027;1']),
        'equipo_duplicado': lambda: p.escribir('equipos.txt', [f'{p.b64(n)};2000-01-01;{p.b64("Gaming")}' for n in ['Alpha', 'alpha']]),
        'alias_duplicado': lambda: p.escribir('creadores.txt', [p.creador('Alpha', 'Uno'), p.creador('Alpha', 'uno')]),
        'equipo_inexistente': lambda: p.escribir('creadores.txt', [p.creador('Fantasma', 'Uno')]),
        'fecha_invalida': lambda: p.escribir('equipos.txt', [f'{p.b64("Alpha")};2023-02-29;{p.b64("Gaming")}']),
        'categoria_invalida': lambda: p.escribir('equipos.txt', [f'{p.b64("Alpha")};2000-01-01;{p.b64("Otra")}']),
        'especialidad_invalida': lambda: p.escribir('creadores.txt', [p.creador('Alpha', 'Uno').replace(p.b64('Streamer'), p.b64('Otra'))]),
        'seguidores_negativos': lambda: p.escribir('creadores.txt', [p.creador('Alpha', 'Uno', -1)]),
        'valoracion_fuera_rango': lambda: p.escribir('creadores.txt', [p.creador('Alpha', 'Uno', 0, 11)]),
        'numero_creador_invalido': lambda: p.escribir('creadores.txt', [p.creador('Alpha', 'Uno').replace(';14;', ';abc;')]),
        'creador_valido_y_corrupto': lambda: p.escribir('creadores.txt', [p.creador('Alpha', 'Nuevo'), '!!!;!!!;!!!;14;0;1;!!!']),
        'equipo_valido_y_corrupto': lambda: p.escribir('equipos.txt', [f'{p.b64("Nuevo")};2000-01-01;{p.b64("Gaming")}', '!!!;2000-01-01;!!!']),
        'lineas_en_blanco': lambda: p.escribir('creadores.txt', ['', p.creador('Alpha', 'Uno'), '']),
    }
    s.cargar()
    for nombre, alterar in alteraciones.items():
        p.preparar()
        s.cargar('s')
        alterar()
        salida[nombre] = {'lectura': s.cargar('s'), 'memoria': s.info()}
        print('  persistencia:', nombre, flush=True)
    p.preparar(['Alpha', 'Vacio', 'Tercero'], [p.creador('Alpha', 'Uno'), p.creador('Tercero', 'Dos')])
    s.cargar('s')
    s.opcion(10)
    salida['guardado_mayor'] = s.volver()
    s.crear_competencia('Pequena', 's')
    s.opcion(10)
    salida['guardado_menor'] = s.volver()
    salida['filas_tras_reemplazo'] = {f.name: f.read_text(encoding='utf-8') for f in p.DATOS.iterdir() if f.is_file()}
    salida['relectura_menor'] = s.cargar('s')
    salida['memoria_menor'] = s.info()
    p.preparar()
    s.cargar('s')
    archivo = p.DATOS / 'creadores.txt'
    archivo.unlink()
    archivo.mkdir()
    try:
        s.opcion(10)
        salida['escritura_bloqueada'] = s.volver()
        salida['memoria_tras_error_escritura'] = s.info()
    finally:
        archivo.rmdir()
        p.preparar()
    return salida


def totales(s):
    salida = {'lectura': s.cargar()}
    s.opcion(7)
    s.linea('Nombre del equipo:', 'Alpha')
    salida['estadisticas'] = s.volver()
    s.opcion(9)
    salida['mas_seguidores'] = s.volver()
    return salida


def ranking(s):
    salida = {'lectura': s.cargar(), 'info': s.info()}
    s.opcion(8)
    s.linea('Primer equipo:', 'Zeta')
    s.linea('Segundo equipo:', 'Alfa')
    salida['comparacion'] = s.volver()
    s.opcion(12)
    salida['ranking'] = p.limpiar(s.esperar('Presioná una tecla para cerrar'))
    s.enviar('x')
    return salida


def decimales(s):
    salida = {'lectura': s.cargar()}
    for entrada in ['8.5', '8,001', '8.004', '7,999']:
        s.opcion(3)
        s.linea('Nombre del equipo:', 'Zeta')
        s.linea('Nombre del creador:', 'Prueba')
        s.linea('Alias:', entrada)
        s.linea('Edad', 14)
        s.linea('Seguidores', 500)
        s.linea('Valoración', entrada)
        s.esperar('Especialidad:')
        s.linea('Opción (1 a 4):', 1)
        salida[f'alta_{entrada}'] = s.volver()
    salida['info'] = s.info()
    s.opcion(10)
    salida['guardar'] = s.volver()
    salida['archivo'] = (p.DATOS / 'creadores.txt').read_text(encoding='utf-8')
    salida['releer'] = s.cargar('s')
    salida['info_relectura'] = s.info()
    return salida


def estados_vacios(s):
    salida = {'lectura': s.cargar()}
    for opcion in [3, 5, 6, 7, 8]:
        s.opcion(opcion)
        s.linea('Primer equipo:' if opcion == 8 else 'Nombre del equipo:', 'NoExiste')
        salida[f'sin_equipos_{opcion}'] = s.volver()
    s.opcion(9)
    salida['sin_equipos_9'] = s.volver()
    s.opcion(2)
    s.linea('Nombre del equipo:', 'Unico')
    s.linea('Fecha de creación', '01/01/2000')
    s.linea('Opción (1 a 4):', 1)
    salida['primer_equipo'] = s.volver()
    s.opcion(2)
    salida['cupo_agotado'] = s.volver()
    s.opcion(6)
    s.linea('Nombre del equipo:', 'Unico')
    s.linea('Alias del creador:', 'NoExiste')
    salida['sin_creadores_6'] = s.volver()
    for opcion in [5, 7]:
        s.opcion(opcion)
        s.linea('Nombre del equipo:', 'Unico')
        salida[f'sin_creadores_{opcion}'] = s.volver()
    s.opcion(12)
    salida['ranking_vacio'] = p.limpiar(s.esperar('Presioná una tecla para cerrar'))
    s.enviar('x')
    return salida


def escritura_parcial(s):
    salida = {'lectura': s.cargar()}
    s.crear_competencia('SoloMemoria', 's')
    archivo = p.DATOS / 'creadores.txt'
    archivo.unlink()
    archivo.mkdir()
    try:
        s.opcion(10)
        salida['error'] = s.volver()
        salida['archivos_despues_del_error'] = {f.name: f.read_text(encoding='utf-8-sig') for f in p.DATOS.iterdir() if f.is_file()}
        salida['memoria'] = s.info()
    finally:
        archivo.rmdir()
        p.preparar()
    return salida


def solo_lectura(s):
    salida = {'lectura': s.cargar()}
    s.crear_competencia('SoloMemoria', 's')
    archivo = p.DATOS / 'creadores.txt'
    previo = archivo.read_bytes()
    archivo.chmod(stat.S_IREAD)
    try:
        s.opcion(10)
        salida['error'] = s.volver()
        salida['archivos_despues_del_error'] = {f.name: f.read_text(encoding='utf-8-sig') for f in p.DATOS.iterdir() if f.is_file()}
        salida['creadores_conservado'] = archivo.read_bytes() == previo
        salida['memoria'] = s.info()
    finally:
        archivo.chmod(stat.S_IWRITE | stat.S_IREAD)
        p.preparar()
    return salida


def regresion(s):
    salida = {'lectura': s.cargar(), 'info': s.info()}
    for equipo in ['Alpha', 'Beta', 'Gamma']:
        s.opcion(7)
        s.linea('Nombre del equipo:', equipo)
        salida[f'estadisticas_{equipo}'] = s.volver()
    s.opcion(9)
    salida['mas_seguidores'] = s.volver()
    s.opcion(12)
    salida['ranking'] = p.limpiar(s.esperar('Presioná una tecla para cerrar'))
    s.enviar('x')
    return salida


if __name__ == '__main__':
    sys.stdout.reconfigure(encoding='utf-8')
    grupo = sys.argv[1] if len(sys.argv) > 1 else 'entradas'
    if grupo == 'entradas':
        p.preparar()
        p.caso('02-entradas-net8', entradas)
    elif grupo == 'persistencia':
        p.preparar()
        p.caso('03-persistencia-net8', persistencia)
    elif grupo == 'totales':
        p.preparar(creadores=[p.creador('Alpha', 'Max', 2147483647), p.creador('Alpha', 'Extra', 1)])
        p.caso('04-totales-net8', totales)
    elif grupo == 'ranking':
        nombres = ['Zeta', 'Alfa', 'águila', 'Aguila', 'beta', 'A\u0301guila']
        p.preparar(nombres, [p.creador(n, 'Uno') for n in nombres])
        p.caso('05-ranking-net8', ranking)
    elif grupo == 'redondeo':
        p.preparar(['Zeta', 'Alfa'], [p.creador('Zeta', 'Uno', 500, 8.004), p.creador('Alfa', 'Uno', 500, 8.001)])
        p.caso('06-redondeo-net8', ranking)
    elif grupo == 'decimales':
        p.preparar(['Zeta'], [])
        p.caso('07-decimales-net8', decimales)
    elif grupo == 'vacios':
        p.preparar([], [])
        p.escribir('equipos.txt', [])
        p.escribir('competencia.txt', [f'{p.b64("SinEquipos")};{p.b64("Sponsor")};2027;1'])
        p.caso('08-vacios-net8', estados_vacios)
    elif grupo == 'escritura':
        p.preparar()
        p.caso('09-escritura-parcial-net8', escritura_parcial)
    elif grupo == 'solo-lectura':
        p.preparar()
        p.caso('10-solo-lectura-net8', solo_lectura)
    elif grupo in ['regresion8', 'regresion10']:
        valores = [(0, 8, 'Streamer'), (500, 8, 'Tiktoker'), (1000, 7, 'Influencer'), (1000, 8, 'Streamer'), (500, 9, 'Youtuber'), (499, 8, 'Streamer'), (500, 7, 'Streamer'), (499, 10, 'Influencer')]
        filas = [p.creador('Alpha', str(i), seg, val).replace(p.b64('Streamer'), p.b64(esp)) for i, (seg, val, esp) in enumerate(valores)]
        filas += [p.creador('Beta', 'Uno', 4498, 8)]
        p.preparar(['Alpha', 'Beta', 'Gamma'], filas)
        p.caso('11-regresion-net8' if grupo == 'regresion8' else '12-regresion-net10', regresion, None if grupo == 'regresion8' else 'C:/Program Files/dotnet/dotnet.exe')
