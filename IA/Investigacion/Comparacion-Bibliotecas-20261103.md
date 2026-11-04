# Preparación de Mis Librerias como submódulo

Fecha: 03/11/2026. Alcance: primera tarea pendiente de la etapa 3.5, comparar y respaldar. No se realizó la conversión ni se modificaron bibliotecas.

## Referencias y respaldo

- Proyecto principal al iniciar: `ec96a4e`.
- Repositorio de bibliotecas: https://github.com/51215927-spec/Mis-Librerias.git, rama `main`, commit comparado `40da20f5feec7117f69a7f4d03e79ee7aa950889`.
- Cuenta comprobada con `gh`: `51215927-spec`, permiso `ADMIN` en ambos repositorios.
- Clon de consulta: `IA/Verificacion/PreparacionSubmodulo-20261103/Remoto`.
- Respaldo completo: `IA/Verificacion/PreparacionSubmodulo-20261103/Respaldo/Mis Librerias`.
- Respaldo comprobado: 58 archivos, 524355 bytes. Se compararon cantidad y SHA-256 de cada archivo con el original, incluidos los artefactos existentes de `bin` y `obj`, sin modificarlos.

El clon y el respaldo son locales y están excluidos de Git por la regla existente `/IA/Verificacion/`. El [informe JSON](../../test/resultados/Comparacion-Bibliotecas-20261103.json) conserva el manifiesto de hashes y resultados en el repositorio. Los archivos versionados originales también permanecen recuperables desde el commit del proyecto indicado arriba. No borrar el respaldo hasta verificar la migración.

## Comparación

Se compararon por SHA-256 todos los archivos de trabajo, incluyendo ocultos; se excluyeron `.git`, `bin` y `obj` de la comparación funcional. Los generados sí se incluyeron en el respaldo.

| Resultado | Archivos |
| --- | --- |
| Idénticos, 13 | Los cuatro archivos C# y el proyecto de `DataBases`; los tres archivos C# y el proyecto de `Utilidades`; `README.md` y los tres documentos de `Documentacion`. |
| Diferente, 1 | `Mis Librerias.slnx`. |
| Solo locales, 0 | Ningún archivo no generado. |
| Solo remotos, 6 | `.gitignore`, dos documentos dentro de `DataBases` y los tres archivos de `DataBases viejo`. |

El conteo exacto de los 13 idénticos es: siete archivos C# (cuatro en `DataBases` y tres en `Utilidades`), dos proyectos y cuatro documentos (`README.md` y tres de `Documentacion`). `ConstructorModelo.cs` contiene `IEntidad<TSelf>` y su método estático abstracto `Configurar`; su contenido es idéntico en ambos repositorios. No hay contratos locales que deban trasladarse.

## Diferencia de la solución y resolución prevista

La solución local de bibliotecas contiene únicamente:

```xml
<Solution>
  <Project Path="Utilidades/Utilidades.csproj" />
  <Project Path="DataBases/DataBases.csproj" />
</Solution>
```

La remota incluye además `../ACTIVIDAD ESPECIAL/ACTIVIDAD ESPECIAL/ACTIVIDAD ESPECIAL.csproj`. Esa ruta depende de una estructura externa y no corresponde a la ubicación del proyecto al anidar el repositorio en `Mis Librerias`.

Resolución definida para la migración: conservar la solución local de bibliotecas, sin la referencia a la aplicación, y publicar ese cambio en el repositorio de bibliotecas antes de fijar el commit del submódulo. La solución principal ya incluye la aplicación y ambos proyectos; sus referencias `Mis Librerias/DataBases/DataBases.csproj` y `Mis Librerias/Utilidades/Utilidades.csproj` deben conservarse. El cambio de `.slnx` todavía no fue aplicado ni publicado; corresponde a la siguiente tarea de migración.

Los seis archivos exclusivos del remoto se conservarán al usarlo como submódulo. No se eliminará `DataBases viejo` ni se moverá su código al proyecto actual. Su mera existencia no implica que deba integrarse a la aplicación. Los generados locales no deben copiarse sobre el clon: se mantienen en el respaldo y se regenerarán al compilar. En la conversión se deberá retirar del índice principal la carpeta actualmente versionada y registrar el submódulo, evitando borrar el respaldo o perder los originales antes de verificar el resultado.

## Verificación y siguiente paso

Completadas la comparación y la copia íntegra. Revisados los contratos y las referencias de proyectos. No se compila porque esta tarea no modifica código ni soluciones; la compilación de ambas soluciones será obligatoria al aplicar la resolución y convertir la carpeta. No se ejecutaron pruebas del menú ni se crearon `.gitmodules` o `ColoresMenu`.

Próxima tarea: convertir la carpeta a submódulo, aplicando y publicando la resolución descrita. Esta tarea no se inició en esta interacción.
