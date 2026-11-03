# Conversión de Mis Librerias a submódulo

Fecha: 03/11/2026. Tercera casilla de la etapa 3.5; no se modificó presentación ni lógica C#.

## Cambios

1. Se volvió a comprobar el respaldo y los originales contra los 58 hashes del [manifiesto previo](Comparacion-Bibliotecas-20261103.json). Coinciden. El remoto permanecía en `40da20f5feec7117f69a7f4d03e79ee7aa950889`.
2. En el repositorio de bibliotecas se sustituyó `Mis Librerias.slnx` por la versión local: solo `Utilidades` y `DataBases`, sin referencia externa a la aplicación. Compilación correcta antes del commit `a78e2c55e5e469e22874aab9d3bd7f87e285141b`, publicado en `main` antes de registrar el submódulo.
3. Se movió la carpeta original completa a `IA/Verificacion/PreparacionSubmodulo-20261103/OriginalesAntesDeSubmodulo`, tras verificar las rutas absolutas y que el destino no existiera. No se borró el respaldo previo ni se sobrescribieron originales.
4. Se retiraron del índice principal los archivos de la carpeta con `git rm -r --cached`, sin borrar sus copias. Se registró el submódulo mediante `git submodule add -b main https://github.com/51215927-spec/Mis-Librerias.git "Mis Librerias"`. `.gitmodules` conserva URL, ruta y rama; la entrada Git de modo `160000` fija el commit publicado. La rama configurada no actualiza automáticamente el commit fijado.
5. Los 14 archivos originales no generados coinciden por SHA-256 con los del submódulo, incluida la solución corregida. Se conservan los seis archivos adicionales del remoto. Los generados `bin/obj` dejaron de versionarse en el repositorio principal y permanecen en ambas copias de respaldo. No se modificaron las referencias de la aplicación ni de la solución principal.

## Verificación

- `gh auth status` y `gh repo view`: cuenta `51215927-spec`, permiso `ADMIN` en ambos repositorios.
- `dotnet build "ACTIVIDAD ESPECIAL.sln" --artifacts-path "IA/Verificacion/PreparacionSubmodulo-20261103/CompilacionProyecto" -p:NuGetAudit=false`: cero errores, advertencias previas `CA1416` y `CS0219`.
- `dotnet build "Mis Librerias/Mis Librerias.slnx" --artifacts-path "IA/Verificacion/PreparacionSubmodulo-20261103/CompilacionSubmodulo" -p:NuGetAudit=false`: cero errores, advertencia previa `CA1416`.
- También se compiló la solución de bibliotecas en el clon preparatorio antes de publicarla.
- SDK `10.0.401`; los proyectos siguen con destino .NET 8. Artefactos aislados para no editar generados originales. Se desactivó únicamente la auditoría de NuGet en estos comandos.
- Submódulo sin cambios pendientes, `git diff --cached --check` correcto, referencia fijada disponible en el remoto.

## Uso

Clonación completa:

```powershell
git clone --recurse-submodules https://github.com/51215927-spec/ACTIVIDAD-ESPECIAL.git
```

Inicialización o recuperación del commit fijado en una copia existente:

```powershell
git submodule update --init --recursive
```

Los cambios de bibliotecas se publican primero en su repositorio; después se registra y publica la nueva referencia de `Mis Librerias` en el principal. El ZIP de entrega debe incluir su contenido: un archivo del repositorio principal sin inicializar el submódulo no basta.

Siguiente tarea pendiente: contrastar las utilidades con las pantallas del ejecutable y documentar diferencias. No se inició `ColoresMenu` ni la conexión del menú.
