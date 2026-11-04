# Pruebas unitarias de StreamORT

Proyecto xUnit para .NET 8, incluido en `ACTIVIDAD ESPECIAL.sln` y con referencia a la aplicación. La aplicación permite que este proyecto acceda a sus clases internas mediante `InternalsVisibleTo`, sin cambiar su visibilidad pública.

Ejecutar desde la raíz:

```powershell
dotnet test "ACTIVIDAD ESPECIAL.sln"
```

Todavía no contiene casos de prueba. Los casos de dominio y persistencia corresponden a las tareas 2 y 3 de la etapa 6 del [plan](../../docs/Plan.md). Se eliminó la prueba vacía de la plantilla para no contarla como verificación del comportamiento.

En xUnit, `[Fact]` identifica una prueba y `Assert` comprueba el resultado; `[Theory]` con `[InlineData]` permite repetir una prueba con distintos datos.
