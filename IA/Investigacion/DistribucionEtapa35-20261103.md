# Comprobación de clonación y ZIP con submódulo

Fecha: 03/11/2026. Tarea de distribución de la etapa 3.5. No se modifican aplicación ni bibliotecas.

## Documentación preparada

- README principal: comandos de clonación recursiva, inicialización, compilación y ejecución; alcance actual de desarrollo.
- [Submodulos-y-distribucion.md](../Submodulos-y-distribucion.md): diferencia entre recuperar la referencia fijada y adoptar una nueva versión, orden de publicación, cuenta/permisos y creación/verificación del ZIP.
- [crear_zip.py](../crear_zip.py): auxiliar de distribución sin dependencias externas de Python. Combina archivos versionados del principal y del commit fijado de Mis Librerias, preserva sus rutas y verifica el ZIP. No sobreescribe paquetes ni manifiestos existentes y exige copia limpia/versiones coincidentes.

## Clon nuevo desde GitHub

Se ejecutó `git clone --recurse-submodules --single-branch --branch main` desde https://github.com/51215927-spec/ACTIVIDAD-ESPECIAL.git en `IA/Verificacion/DistribucionEtapa35/Clon`. No se reutilizaron los respaldos o clones anteriores.

- Principal descargado: `1377ad00dab24da0899edd15deb46b365befddea`.
- Submódulo recuperado: `f4af93aea37cbff6cd73061136edd7a81abb209a` desde Mis-Librerias.
- `git submodule update --init --recursive` termina correctamente y conserva la referencia fijada.
- También se ejecutó `git submodule update --init --remote -- "Mis Librerias"` en ese clon: main remoto sigue en el mismo commit, por lo que la referencia no cambia y el clon permanece limpio. Se prueba así la ruta de actualización documentada sin publicar otra versión.
- Compilación de la solución principal correcta, cero errores y advertencias previas CA1416/CS0219.
- Clon y submódulo limpios después de compilar, con artefactos aislados.

Cuenta verificada antes del trabajo: `51215927-spec`, permiso ADMIN en ambos repositorios. SDK 10.0.401, destino net8.0 sin cambios; NuGetAudit=false en las comprobaciones, únicamente para desactivar la auditoría de paquetes durante estos comandos.

## Paquete de prueba

Se empaquetó ese mismo clon limpio con el auxiliar nuevo:

```powershell
python IA/crear_zip.py --repositorio IA/Verificacion/DistribucionEtapa35/Clon --destino IA/Verificacion/DistribucionEtapa35/StreamORT-etapa35-1377ad0.zip
```

Resultado: 380 archivos, de los que 22 pertenecen a Mis Librerias. Incluye fuentes y proyectos de Utilidades y DataBases, ColoresMenu, FormatoMenu, la solución independiente y documentación de bibliotecas. Se verificaron los archivos requeridos, ausencia de entradas duplicadas, CRC y coincidencia de bytes del ZIP con los dos commits.

La herramienta excluye `.git`, `.vs`, `bin` y `obj`, incluidos los generados antiguos que aún estuvieran versionados. Conserva los demás archivos versionados, incluidos documentación, registro de IA y ejecutable original. No incorpora herramientas ni copias ignoradas. El resultado no depende de inicializar el submódulo al extraer, porque sus archivos ya están dentro del paquete.

El ZIP pesa 2940603 bytes y se conserva localmente en `IA/Verificacion/DistribucionEtapa35/StreamORT-etapa35-1377ad0.zip`, excluido de Git. Su [manifiesto versionado](../../test/resultados/DistribucionEtapa35-20261103.manifest.json) contiene hashes de los archivos, commits y SHA-256 del paquete; el ZIP también conserva un manifiesto lateral en la carpeta de verificación.

## Extracción y compilación independientes de Git

Se extrajo con Expand-Archive a `IA/Verificacion/DistribucionEtapa35/Extraido`. Se comprobaron uno por uno los hashes SHA-256 de sus 380 archivos contra el manifiesto, sin diferencias. No contiene `.git` principal ni de bibliotecas.

Se compiló la solución de esa carpeta, con artefactos en `IA/Verificacion/DistribucionEtapa35/CompilacionExtraido`: cero errores, advertencias previas CA1416/CS0219. La presencia de las bibliotecas se comprobó en el ZIP y en la compilación de la copia extraída, sin clonar submódulos desde ella.

Como la carpeta extraída está físicamente dentro del workspace, se realizó una comprobación adicional fijando temporalmente `GIT_CEILING_DIRECTORIES` a la raíz extraída, para impedir que Git descubra el repositorio del directorio superior. `git rev-parse --show-toplevel` desde la aplicación devolvió el error esperado de ausencia de repositorio. La compilación con artefactos nuevos en `CompilacionExtraidoAislado` pasó igualmente con cero errores y las mismas advertencias. La variable se restauró en finally. La primera envoltura PowerShell se detuvo al tratar ese error nativo esperado como terminante; se ajustó su captura y se repitió correctamente, sin alterar el proyecto.

Comandos de compilación utilizados:

```powershell
dotnet build "IA/Verificacion/DistribucionEtapa35/Clon/ACTIVIDAD ESPECIAL.sln" --artifacts-path IA/Verificacion/DistribucionEtapa35/CompilacionClon -p:NuGetAudit=false
Expand-Archive -LiteralPath "IA/Verificacion/DistribucionEtapa35/StreamORT-etapa35-1377ad0.zip" -DestinationPath "IA/Verificacion/DistribucionEtapa35/Extraido"
dotnet build "IA/Verificacion/DistribucionEtapa35/Extraido/ACTIVIDAD ESPECIAL.sln" --artifacts-path IA/Verificacion/DistribucionEtapa35/CompilacionExtraido -p:NuGetAudit=false
```

Para repetir, usar carpetas y nombre de ZIP nuevos, sin sobrescribir copias anteriores. Se preservan todas las copias de seguridad previas.

## Alcance de entrega

Este paquete acredita el procedimiento y el contenido de fuentes del commit `1377ad0`. Es anterior a esta guía y al script recién creados, y no se presenta como ZIP final. En la etapa 6 se regenerará con el código terminado, documentación final y registro de IA, verificando nuevamente extracción/compilación/ejecución. No se dan por implementadas las operaciones del menú ni la persistencia por el hecho de que el paquete compile.

La última tarea pendiente de la etapa 3.5 sigue siendo actualizar integralmente README y guías de Mis-Librerias, y sincronizar la documentación del proyecto.
