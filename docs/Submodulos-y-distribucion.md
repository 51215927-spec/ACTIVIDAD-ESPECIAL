# Clonar, actualizar y distribuir el proyecto

`Mis Librerias` pertenece a https://github.com/51215927-spec/Mis-Librerias.git. `.gitmodules` indica su ruta, URL y rama `main`; el repositorio principal fija un commit concreto. Tener `main` configurada no actualiza las bibliotecas automáticamente.

## Copia nueva

Desde la carpeta donde se quiere guardar el proyecto:

```powershell
git clone --recurse-submodules https://github.com/51215927-spec/ACTIVIDAD-ESPECIAL.git
cd ACTIVIDAD-ESPECIAL
git submodule status
dotnet build "ACTIVIDAD ESPECIAL.sln"
```

La compilación necesita SDK .NET 8 o compatible; la ejecución normal requiere runtime .NET 8. En una copia de GitHub obtenida como ZIP convencional, los archivos del submódulo pueden faltar. Utilizar clonación recursiva o el paquete completo descrito debajo.

## Copia existente o actualización del proyecto principal

Con el trabajo propio guardado y revisando primero el estado:

```powershell
git status
git pull --ff-only
git submodule update --init --recursive
git submodule status
```

El último comando de actualización descarga y coloca las bibliotecas en el commit fijado por el proyecto. No elige automáticamente la versión más reciente. Un submódulo puede quedar con HEAD separado de una rama; es normal al recuperar una versión fijada. Antes de editar bibliotecas, cambiar a su rama de trabajo y guardar los cambios en su propio repositorio.

## Adoptar una versión nueva de Mis-Librerias

Publicar primero los cambios en el repositorio de bibliotecas. Verificar la cuenta y el acceso antes de cada publicación:

```powershell
gh auth status
gh repo view 51215927-spec/Mis-Librerias --json nameWithOwner,viewerPermission
gh repo view 51215927-spec/ACTIVIDAD-ESPECIAL --json nameWithOwner,viewerPermission
```

Con ambos repositorios limpios y la nueva versión ya publicada en la rama `main` de bibliotecas, actualizar la copia del principal:

```powershell
git submodule update --init --remote -- "Mis Librerias"
git diff --submodule
dotnet build "ACTIVIDAD ESPECIAL.sln"
```

`--remote` adopta la versión de la rama configurada; es distinto de recuperar el commit que ya estaba fijado. Tras revisar y verificar el cambio, registrar únicamente esa referencia y los archivos propios de la tarea:

```powershell
git add "Mis Librerias"
git commit -m "Actualizar versión de Mis Librerias"
git push origin main
```

El commit principal guarda la nueva referencia. Si no se publica primero el commit de bibliotecas, otras personas no podrán recuperar esa versión al clonar. No usar el repositorio principal para guardar los archivos internos del submódulo como si fueran archivos normales.

## ZIP con las bibliotecas incluidas

La herramienta auxiliar [crear_zip.py](../scripts/crear_zip.py) combina los archivos versionados del commit principal con los del commit fijado de `Mis Librerias`. Necesita Python 3 y Git; usa solo la biblioteca estándar de Python. No modifica archivos del proyecto ni requiere paquetes en la aplicación C#.

Ejecutar desde una copia limpia, con los cambios que se deseen distribuir ya publicados e inicializado el submódulo:

```powershell
git submodule update --init --recursive
python scripts/crear_zip.py --destino test/resultados/local/StreamORT.zip
Expand-Archive -LiteralPath "test/resultados/local/StreamORT.zip" -DestinationPath "test/resultados/local/PaqueteExtraido"
Push-Location "test/resultados/local/PaqueteExtraido"
dotnet restore "ACTIVIDAD ESPECIAL.sln"
dotnet build "ACTIVIDAD ESPECIAL.sln"
dotnet test "ACTIVIDAD ESPECIAL.sln"
dotnet run --project "src/ACTIVIDAD ESPECIAL/ACTIVIDAD ESPECIAL.csproj"
Pop-Location
```

El script se niega a sobrescribir un ZIP o manifiesto existente: utilizar otro nombre al regenerar. También rechaza copias con cambios pendientes o un submódulo en una versión distinta de la fijada. `--repositorio RUTA` permite empaquetar otra copia limpia, por ejemplo un clon nuevo.

El ZIP incluye fuentes, solución, proyectos, bibliotecas, documentación y demás archivos versionados, incluido el ejecutable recuperado. Excluye metadatos Git, `.vs`, `bin` y `obj`, aunque haya archivos generados antiguos versionados. Las herramientas y copias locales ignoradas, actualmente bajo test/herramientas y test/resultados/local, no se incluyen. Las bibliotecas se incorporan como archivos reales: después de extraer no hace falta inicializar Git para compilar. Las pruebas xUnit sí están incluidas; la restauración necesita acceso a NuGet para obtener sus paquetes. Ejecutar desde la carpeta extraída también aísla su directorio relativo datos. Para continuar desarrollo con historial, usar el clon recursivo.

Junto al ZIP se genera un manifiesto JSON con los commits de origen, SHA-256 del ZIP, hash de cada archivo y lista de generados excluidos. El script comprueba CRC, archivos necesarios, ausencia de entradas duplicadas y coincidencia de los bytes. Se debe extraer y compilar el paquete generado, como en el ejemplo; la presencia de un ZIP no acredita por sí sola la compilación.

La comprobación realizada está documentada en [Investigacion/DistribucionEtapa35-20261103.md](../IA/Log.md#archivo-distribucionetapa35-20261103). El ZIP final deberá regenerarse con el proyecto terminado, su documentación final y el registro de IA requerido, repitiendo la comprobación de extracción/compilación. Ese cierre de entrega sigue en la etapa 6.
