# Pruebas unitarias de StreamORT

Proyecto xUnit para .NET 8, incluido en `ACTIVIDAD ESPECIAL.sln` y con referencia a la aplicación. La aplicación permite que este proyecto acceda a sus clases internas mediante `InternalsVisibleTo`, sin cambiar su visibilidad pública.

Ejecutar desde la raíz:

```powershell
dotnet test "ACTIVIDAD ESPECIAL.sln"
```

Contiene 86 casos de dominio de la tarea 2 de la etapa 6 del [plan](../../docs/Plan.md):

- `CreadorTests`: 22 casos sobre constructores, límites, especialidades y Estrella sin redondeo.
- `EquipoTests`: 25 casos sobre fechas, categorías, alias, pertenencia, búsquedas, estadísticas, listas vacías y popularidad.
- `CompetenciaTests`: 39 casos sobre constructores, cupos, duplicados, búsquedas, comparación, seguidores y TOP 3.

Los escenarios incluyen decimales, totales superiores a `int.MaxValue`, empates y protección de las listas consultadas. Las fechas futuras se calculan a partir de hoy, para evitar que la prueba deje de ser válida con el tiempo. Los datos son objetos en memoria; no se leen ni escriben los archivos originales. Las pruebas de persistencia quedan pendientes para la tarea 3.

Verificación del 04/11/2026: `dotnet build "ACTIVIDAD ESPECIAL.sln" --no-restore` sin errores (advertencia previa CS0219); `dotnet test "ACTIVIDAD ESPECIAL.sln" --no-build --no-restore` con 86 casos correctos, cero fallos y cero omitidos sobre .NET 8.0.33.

En xUnit, `[Fact]` identifica una prueba y `Assert` comprueba el resultado; `[Theory]` con `[InlineData]` permite repetir una prueba con distintos datos.
