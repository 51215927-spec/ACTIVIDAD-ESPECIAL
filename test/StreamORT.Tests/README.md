# Pruebas unitarias de StreamORT

Proyecto xUnit para .NET 8, incluido en `ACTIVIDAD ESPECIAL.sln` y con referencia a la aplicación. La aplicación permite que este proyecto acceda a sus clases internas mediante `InternalsVisibleTo`, sin cambiar su visibilidad pública.

Ejecutar desde la raíz:

```powershell
dotnet test "ACTIVIDAD ESPECIAL.sln"
```

Contiene 119 casos de las tareas 2 y 3 de la etapa 6 del [plan](../../docs/Plan.md):

- `CreadorTests`: 22 casos sobre constructores, límites, especialidades y Estrella sin redondeo.
- `EquipoTests`: 25 casos sobre fechas, categorías, alias, pertenencia, búsquedas, estadísticas, listas vacías y popularidad.
- `CompetenciaTests`: 39 casos sobre constructores, cupos, duplicados, búsquedas, comparación, seguidores y TOP 3.
- `DatosTests`: 33 casos de persistencia sobre carpetas temporales únicas: ida/vuelta entre culturas, campos y cálculos, conjuntos vacíos, respaldos, archivos inválidos, relaciones y errores de escritura.

Los escenarios incluyen decimales, totales superiores a `int.MaxValue`, empates y protección de las listas consultadas. Las fechas futuras se calculan a partir de hoy, para evitar que la prueba deje de ser válida con el tiempo. No se leen ni escriben los archivos originales.

`DatosTests` restaura la competencia activa y la cultura al terminar cada caso; elimina solo su carpeta temporal, tras comprobar su ruta. Sus casos se ejecutan secuencialmente por pertenecer a la misma clase xUnit, ya que `Datos` mantiene estado estático. Las otras clases de pruebas no acceden a ese estado. Si se agregan otras clases que usen `Datos`, deberán compartir una colección xUnit para evitar concurrencia entre ellas.

El bloqueo de archivos que impide mover la carpeta actual se comprueba solo en Windows; en otros sistemas ese caso termina sin ejecutar el bloqueo. Las pruebas nuevas corren en el proceso de xUnit. La comprobación previa entre procesos y las confirmaciones de consola se conservan en las sondas de etapa 5.

Verificación del 04/11/2026: `dotnet build "ACTIVIDAD ESPECIAL.sln" --no-restore` sin errores ni advertencias en compilación incremental; `dotnet test "ACTIVIDAD ESPECIAL.sln" --no-build --no-restore` con 119 casos correctos, cero fallos y cero omitidos sobre .NET 8.0.33 en Windows. Persisten las advertencias previas CA1416/CS0219 que pueden aparecer al recompilar.

En xUnit, `[Fact]` identifica una prueba y `Assert` comprueba el resultado; `[Theory]` con `[InlineData]` permite repetir una prueba con distintos datos.
