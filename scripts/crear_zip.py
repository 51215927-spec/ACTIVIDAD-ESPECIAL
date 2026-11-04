"""Paquete de fuentes del proyecto y del commit fijado de Mis Librerias.

Herramienta auxiliar de distribución, no forma parte de la aplicación C#.
Requiere Python 3 y Git; no necesita paquetes adicionales.
"""
import argparse
import hashlib
import io
import json
import subprocess
import zipfile
from pathlib import Path


def git(repo, *args):
    return subprocess.run(['git', '-C', str(repo), *args], check=True, stdout=subprocess.PIPE).stdout


def create_package(repo, output):
    library = repo / 'Mis Librerias'
    project_commit = git(repo, 'rev-parse', 'HEAD').decode().strip()
    library_commit = git(repo, 'rev-parse', 'HEAD:Mis Librerias').decode().strip()
    for source in [repo, library]:
        if git(source, 'status', '--porcelain').strip():
            raise RuntimeError(f'La copia debe estar limpia antes de empaquetar: {source}')
    if git(library, 'rev-parse', 'HEAD').decode().strip() != library_commit:
        raise RuntimeError('Inicializar el submódulo en el commit fijado antes de crear el ZIP.')

    files = {}
    excluded = []
    for source, commit, prefix in [(repo, project_commit, ''), (library, library_commit, 'Mis Librerias/')]:
        with zipfile.ZipFile(io.BytesIO(git(source, 'archive', '--format=zip', commit))) as archive:
            for entry in archive.infolist():
                if entry.is_dir():
                    continue
                name = prefix + entry.filename
                if any(part.lower() in ('.git', '.vs', 'bin', 'obj') for part in Path(name).parts):
                    excluded.append(name)
                    continue
                if name in files:
                    raise RuntimeError(f'Entrada duplicada: {name}')
                files[name] = archive.read(entry)

    for required in ['ACTIVIDAD ESPECIAL.sln', 'src/ACTIVIDAD ESPECIAL/ACTIVIDAD ESPECIAL.csproj',
                     'Mis Librerias/Utilidades/Utilidades.csproj', 'Mis Librerias/Utilidades/Funciones.cs',
                     'Mis Librerias/Utilidades/ColoresMenu.cs', 'Mis Librerias/Utilidades/FormatoMenu.cs',
                     'Mis Librerias/DataBases/DataBases.csproj', 'Mis Librerias/DataBases/ConstructorModelo.cs']:
        if required not in files:
            raise RuntimeError(f'Falta un archivo necesario: {required}')

    manifest_path = output.with_suffix('.manifest.json')
    if output.exists() or manifest_path.exists():
        raise FileExistsError('El ZIP o su manifiesto ya existen; utilizar otro nombre.')
    output.parent.mkdir(parents=True, exist_ok=True)
    with zipfile.ZipFile(output, 'x', compression=zipfile.ZIP_DEFLATED) as archive:
        for name, content in sorted(files.items()):
            archive.writestr(name, content)
    with zipfile.ZipFile(output) as archive:
        if archive.testzip() is not None:
            raise RuntimeError('El ZIP no superó la verificación CRC.')
        for name, content in files.items():
            if archive.read(name) != content:
                raise RuntimeError(f'El ZIP no conserva el archivo: {name}')

    manifest = dict(commitProyecto=project_commit, commitBibliotecas=library_commit,
                    sha256Zip=hashlib.sha256(output.read_bytes()).hexdigest(),
                    archivos={name: hashlib.sha256(content).hexdigest() for name, content in sorted(files.items())},
                    excluidosGenerados=excluded)
    manifest_path.write_text(json.dumps(manifest, ensure_ascii=False, indent=2), encoding='utf-8')
    print(f'ZIP verificado: {len(files)} archivos, {sum(n.startswith("Mis Librerias/") for n in files)} de bibliotecas.')
    print(output)
    return manifest


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--repositorio', type=Path, default=Path(__file__).resolve().parent.parent)
    parser.add_argument('--destino', type=Path, required=True)
    options = parser.parse_args()
    create_package(options.repositorio.resolve(), options.destino.resolve())
