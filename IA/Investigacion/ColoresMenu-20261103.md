# ColoresMenu y sobrecarga compatible

Fecha: 03/11/2026. Tarea de diseño y declaración de la etapa 3.5.

Se documentaron primero en `IA/Arquitectura.md` las diez propiedades, valores predeterminados y firma; después se realizaron dos cambios separados en el repositorio de bibliotecas:

| Commit de Mis-Librerias | Cambio | Verificación antes de publicar |
| --- | --- | --- |
| `770c6ac` | `Utilidades/ColoresMenu.cs`: struct público, diez propiedades ConsoleColor con get/set y constructor sin parámetros. | Solución de bibliotecas compilada, cero errores; CA1416 previa. |
| `115b1b7c80bd876d71bf4042cc97971e6b4b1682` | Nueva sobrecarga de `PedirOpcionMenu` en Funciones.cs y documentación de uso en la guía de Utilidades. | Solución principal y sonda compiladas; comparación interactiva correcta. |

Ambos commits están publicados en `main` de https://github.com/51215927-spec/Mis-Librerias.git. Se conserva la firma previa con sus parámetros opcionales para mantener las llamadas de `Program`, `PedirEnum` y otros consumidores actuales. El tercer argumento de la nueva firma es obligatorio, de modo que las llamadas de dos argumentos siguen resolviendo a la anterior.

Ejemplo de uso disponible en la biblioteca:

```csharp
var colores = new ColoresMenu
{
    Seleccion = ConsoleColor.Black,
    Cursor = ConsoleColor.Black,
    NoSeleccion = ConsoleColor.White,
    FondoSeleccion = ConsoleColor.Gray,
    FondoCursor = ConsoleColor.Gray
};
int indice = Funciones.PedirOpcionMenu(opciones, "Menú:", colores);
```

El constructor inicializa los mismos valores predeterminados de la firma anterior. `default(ColoresMenu)` y los elementos de un array recién creado permanecen con valores cero (Black), según las reglas de structs; utilizar `new ColoresMenu()` o el inicializador para obtener la paleta predeterminada. No hay configuración del dominio o del formato dentro del struct.

## Comprobación interactiva

La sonda `SondaColoresMenu/` llama ambas firmas en cuatro sesiones ConPTY sobre .NET 8.0.33: firma anterior y nueva con paleta predeterminada, y ambas con una paleta personalizada que distingue título, cursor, resultado confirmado, otras opciones y fondos. En cada sesión se captura inicio, flecha abajo y confirmación con Enter.

Resultado: las tres pantallas de cada par son idénticas por sus segmentos de texto, colores, posiciones y estado del cursor. Las cuatro sesiones devuelven índice 1 y restauran el texto Magenta/fondo DarkBlue que tenía el llamador. Se inspeccionó la captura del resultado confirmado de la paleta personalizada.

Los [resultados](ColoresMenu-20261103/resultado.json), entradas y segmentos por celda de las cuatro sesiones se conservan con doce PNG y doce TXT en `ColoresMenu-20261103/`. Las imágenes reutilizan el mecanismo ANSI documentado en el contraste visual previo, con su misma fuente y paleta explícitas; no son fotografías de una ventana física. Las capturas crudas .ansi.txt quedan ignoradas por la regla existente. Esta comprobación no prueba todas las paletas, modos o entradas ni sustituye el proyecto final de pruebas unitarias.

Repetición desde la raíz, con las herramientas locales de la investigación previa:

```powershell
dotnet build "Mis Librerias/Mis Librerias.slnx" --artifacts-path IA/Verificacion/ColoresMenu/CompilacionTipo -p:NuGetAudit=false
dotnet build "ACTIVIDAD ESPECIAL.sln" --artifacts-path IA/Verificacion/ColoresMenu/CompilacionIntegracion -p:NuGetAudit=false
dotnet build IA/Investigacion/SondaColoresMenu/SondaColoresMenu.csproj --artifacts-path IA/Verificacion/ColoresMenu/CompilacionSonda -p:NuGetAudit=false
python IA/Investigacion/verificar_colores_menu.py
```

Compilación correcta con SDK 10.0.401, proyectos net8.0 sin cambios: CA1416 previa en bibliotecas y sonda; CA1416 y CS0219 previas en la solución principal. Artefactos aislados en IA/Verificacion; NuGetAudit=false desactiva solo la auditoría para estas ejecuciones. La sonda no pertenece a la solución académica ni introduce dependencias en la aplicación.

## Alcance pendiente

Esta sobrecarga delega en la implementación actual. No modifica todavía marcador, ayuda INFO, ancho de resaltado, navegación, menú múltiple ni las llamadas de `Program`. Los mensajes INFO actuales fuerzan cian independientemente de TextoNormal; se documenta el comportamiento vigente para no confundirlo con una adaptación ya realizada. Los siguientes pasos del plan abordarán el formato idéntico del ejecutable un método por vez.
