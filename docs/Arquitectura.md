# Arquitectura de la reconstrucción de StreamORT

Actualizado: 04/11/2026. Dominio, las 12 opciones del menú, persistencia CSV y pruebas implementados. No se requiere UML ni usar o adaptar `MostrarTabla`, por indicación del usuario.

## Alcance y referencias

La aplicación de consola C# para .NET 8 administra una única competencia activa con equipos y creadores. Reconstruye las reglas observadas del ejecutable mediante POO, listas de objetos y las bibliotecas [Utilidades](../Mis%20Librerias/Utilidades/README.md) y [DataBases](../Mis%20Librerias/DataBases/README.md). Ambas se mantienen en el submódulo `Mis Librerias`; su referencia vigente es `7d4299c`.

El comportamiento comprobado se documenta en [Analisis.md](Analisis.md) y [Pruebas-StreamORT.md](Pruebas-StreamORT.md); la consigna y las indicaciones de trabajo están en [Contexto.md](../IA/Contexto.md). El [recorrido final](RecorridoFinal.md) conserva el contraste de presentación con una copia del original. Esta actualización documental consulta el código y esa evidencia, sin repetir la investigación ni modificar el programa.

Los tipos y clases siguientes describen la reconstrucción; no identifican la implementación interna del original. No se agregan edición, eliminación, partidas ni administración simultánea de competencias.

## Responsabilidades y estado

| Clase | Responsabilidad |
| --- | --- |
| `Program` | Recibir entradas, navegar, confirmar reemplazos, invocar operaciones y presentar resultados con Utilidades. |
| `Competencia` | Validar sus datos, administrar equipos y cupos, buscar, comparar y obtener rankings. |
| `Equipo` | Validar sus datos, administrar creadores, buscar alias y calcular estadísticas. |
| `Creador` | Validar sus datos individuales y determinar si es Estrella. |
| `Datos` | Mantener la competencia activa y coordinar guardado y lectura completos con DataBases. |
| `RegistroException` | Comunicar rechazos de altas sin mezclar validación del dominio con presentación. |

Todas las clases del proyecto principal son `internal`. `Datos` es estática. Las clases del dominio no leen teclas ni imprimen: devuelven resultados o lanzan excepciones que `Program` presenta. `Configurar` está implementado en las tres entidades persistidas, que cumplen `IEntidad<T>`.

No hay cuerpos con `NotImplementedException` ni opciones del menú vacías. Las pruebas están en el proyecto separado [StreamORT.Tests](../test/StreamORT.Tests/README.md); `InternalsVisibleTo` permite acceder al dominio sin hacerlo público.

## Objetos, propiedades y tipos

`Datos.competencia` conserva una sola referencia `Competencia?`, con get y set públicos por indicación del usuario. `null` representa que todavía no se creó o leyó una competencia. No hay carga automática al iniciar.

`Competencia` conserva `equiposRegistrados`; cada `Equipo`, `creadoresRegistrados`. Son listas privadas inicializadas en el constructor. Las propiedades públicas `equipos` y `creadores` devuelven listas nuevas con los mismos objetos: agregar o quitar elementos de esas copias no altera los registros internos. Las altas pasan por los métodos de dominio. Se conserva el orden de registro y carga.

Los datos fijados por constructor tienen solo get. `Equipo.nombreCompetencia` tiene private set para asignar la pertenencia al registrarlo. Las tablas de DataBases son locales al guardado o lectura, no otro estado editable global.

| Dato | Tipo y regla |
| --- | --- |
| Nombres, patrocinador y alias | `string`: obligatorios, sin espacios exteriores; conserva espacios interiores y tildes. |
| Año | `int`, entre 2000 y 2027. |
| Máximo de equipos | `int`, entre 1 y 10000. |
| Edad | `int`, entre 14 y 30. |
| Fecha de creación | `DateOnly`, no posterior a hoy; la entrada exige `dd/MM/yyyy`. |
| Seguidores por creador | `int`, entre 0 e int.MaxValue, en miles. |
| Total de seguidores | `long`, sumando en miles para evitar desbordamiento de int. |
| Valoración y promedio | `decimal`; valoración entre 1 y 10. |
| Búsquedas ausentes | `Equipo?` o `Creador?`, con resultado null. |

Los constructores validan que categoría y especialidad pertenezcan a sus enumeraciones. `ENUMs.cs` contiene `Categoria` (Gaming, Humor, Musica, Entretenimiento), `Especialidad` (Streamer, Youtuber, Tiktoker, Influencer) y `ResultadoComparacion`.

Las búsquedas y duplicados usan Trim y `StringComparison.OrdinalIgnoreCase`. El nombre de equipo es único en la competencia; el alias es único dentro de su equipo y puede repetirse en otro. No se eliminan tildes ni se normalizan acentos combinantes o espacios interiores.

No se redondea antes de guardar, calcular Estrella, comparar o ordenar. 7,999 no cumple valoración >= 8; 8,004 supera a 8,001 aunque ambos promedios se presenten como 8,00. La ficha usa `0.##` y los promedios visibles usan `N2`; estos formatos no modifican el dato almacenado.

## Contratos implementados

Las firmas muestran la API existente. En el código, los métodos tienen sus cuerpos implementados.

### Creador

Propiedades de solo lectura: `nombre`, `alias`, `nombreEquipo`, `edad`, `seguidores`, `valoracion` y `especialidad`. El constructor inicializa y valida los siete datos; la unicidad del alias corresponde al equipo.

```csharp
public Creador(string nombre, string alias, string nombreEquipo, int edad,
    int seguidores, decimal valoracion, Especialidad especialidad);
public bool EsEstrella();
public static void Configurar(ConstructorEntidad<Creador> entidad);
```

`EsEstrella` devuelve `seguidores >= 500 && valoracion >= 8`, con independencia de la especialidad. `Configurar` registra los siete campos en creadores.csv; Estrella se calcula al consultar y no se persiste.

### Equipo

Propiedades: `nombre`, `nombreCompetencia` (private set), `fechaDeCreacion`, `categoria` y `creadores` (copia de la lista interna).

```csharp
public Equipo(string nombre, DateOnly fechaDeCreacion, Categoria categoria,
    string nombreCompetencia = "");
public List<Creador> creadores { get; }
public Creador? BuscarCreador(string alias);
internal void AsignarCompetencia(string nombreCompetencia);
public void RegistrarCreador(Creador creador);
public decimal CalcularValoracionPromedio();
public int ContarStreamers();
public long CalcularTotalSeguidores();
public Creador? ObtenerCreadorMasPopular();
public static void Configurar(ConstructorEntidad<Equipo> entidad);
```

El cuarto parámetro del constructor permite recuperar la asociación desde CSV; al registrar un equipo, `Competencia` llama a `AsignarCompetencia` con su nombre. `RegistrarCreador` rechaza objetos nulos, pertenencia a otro equipo y alias duplicados mediante `RegistroException`, antes de modificar la lista. No hay un cupo de creadores inventado.

El promedio suma valoraciones y divide por cantidad; en un equipo vacío devuelve cero. `ContarStreamers` cuenta únicamente especialidad Streamer. Los seguidores se suman con acumulador long. El creador más popular es quien tiene más seguidores; en empate se conserva el primero y, sin creadores, se devuelve null. `Program` muestra ese resultado como Sin creadores.

`Configurar` registra nombre, nombreCompetencia, fechaDeCreacion y categoria. No persiste la lista ni las estadísticas.

### Competencia

Propiedades de solo lectura: `nombre`, `patrocinador`, `año`, `cantMaxEquipos` y `equipos` (copia de la lista interna).

```csharp
public Competencia(string nombre, string patrocinador, int año, int cantMaxEquipos);
public List<Equipo> equipos { get; }
public bool HayCupo();
public Equipo? BuscarEquipo(string nombre);
public void RegistrarEquipo(Equipo equipo);
public ResultadoComparacion CompararEquipos(string nombrePrimero, string nombreSegundo);
public Equipo? ObtenerEquipoConMasSeguidores();
public List<Equipo> ObtenerTop3();
public static void Configurar(ConstructorEntidad<Competencia> entidad);
```

El constructor valida datos e inicializa una lista vacía. `HayCupo` compara cantidad con máximo. `RegistrarEquipo` rechaza objeto nulo, nombre duplicado o cupo agotado sin alterar los registros; con éxito asigna la competencia y agrega el equipo.

`CompararEquipos` valida, en orden, existencia del primero, existencia del segundo, que sean diferentes y que ambos tengan creadores. Devuelve `PrimerEquipoInexistente`, `SegundoEquipoInexistente`, `MismoEquipo`, `EquipoSinCreadores`, `Empate`, `GanaPrimero` o `GanaSegundo`. Los ganadores se deciden por promedios completos, sin redondeo visual.

El equipo con más seguidores excluye los vacíos, incluye equipos con creadores y cero seguidores y conserva el primero ante empate. Si no hay candidatos devuelve null.

`ObtenerTop3` devuelve una lista nueva de hasta tres equipos con creadores. Usa recorridos e inserción ordenada por promedio descendente y nombre con comparador explícito es-AR sin distinguir mayúsculas. Si el comparador considera equivalentes dos nombres, se conserva el orden de registro. No altera la lista de equipos. Este criterio coincide con las muestras Unicode documentadas; no se afirma identidad universal con el comparador interno del original.

`Configurar` registra los cuatro datos generales. La cantidad de cupos ocupados se calcula, no se guarda.

### RegistroException

```csharp
public RegistroException(string mensaje);
```

Hereda de `Exception` y pasa el mensaje a `base(mensaje)`. `RegistrarEquipo` y `RegistrarCreador` devuelven void y comunican un rechazo con esta excepción. `Program` la captura al dar de alta y muestra Message. Los constructores usan `ArgumentException` para datos individuales inválidos; los errores de archivos conservan sus tipos propios.

### Datos

```csharp
public static Competencia? competencia { get; set; }
internal static string directorioModelo { get; private set; }
public static void Guardar(string directorio);
public static Competencia Leer(string directorio);
private static Competencia LeerConjunto(string directorio);
private static Modelo CrearModelo(string directorio);
```

`Guardar` exige una competencia activa y no cambia su referencia. `Leer` devuelve una competencia nueva, completa y validada, sin reemplazar la activa ni pedir confirmación. `Program` asigna el resultado solo después de confirmar si ya existía una competencia.

`CrearModelo` construye un modelo local con las tres entidades. Como Configurar es estático, usa temporalmente directorioModelo para formar las rutas absolutas y lo restaura mediante finally. No cambia el directorio de trabajo global. Este mecanismo está diseñado para la consola secuencial, no para persistencia concurrente.

### Program

```csharp
static void Main(string[] args);
static Competencia? CrearCompetencia();
static Equipo? CrearEquipo();
static Creador? CrearCreador();
static string PedirTextoRequerido(string mensaje);
static DateOnly PedirFechaEquipo();
static decimal PedirValoracion();
static bool ConfirmarReemplazo(string mensaje);
static void MostrarCompetencia(Competencia competencia);
static void MostrarEquipo(Competencia competencia);
static void MostrarCreador(Competencia competencia);
static void MostrarEquipoDatos(Equipo equipo);
static string FormatearCreador(Creador creador);
static string FormatearCategoria(Categoria categoria);
static void MostrarEstadisticas(Competencia competencia);
static void MostrarComparacion(Competencia competencia);
static void MostrarTop3(Competencia? competencia);
static ConsoleColor ObtenerFondoOpcion(int opcion);
static void MostrarTituloOpcion(int opcion);
static void MostrarEquipoConMasSeguidores(Competencia competencia);
```

Los métodos de creación devuelven un objeto válido o null si el flujo no se completa (cancelación, falta de cupo, equipo inexistente o rechazo manejado, según la operación). Los casos del menú registran los equipos y creadores en su dueño y capturan los rechazos. `CrearCreador` solicita y busca el equipo antes de pedir los datos del creador; no recibe un parámetro nombreEquipo.

`ConfirmarReemplazo` lee una tecla: s/S confirma; las demás cancelan. No usa PedirBool, que tiene contrato y/n. Crear una competencia y leer otra requieren confirmación cuando ya hay una activa. La lectura inválida falla antes de ofrecer el reemplazo y conserva la memoria.

`PedirTextoRequerido` recorta espacios exteriores y repregunta ante vacío. La fecha usa TryParseExact con dd/MM/yyyy y rechaza valores futuros. `PedirValoracion` intenta decimal.TryParse con la cultura actual y luego InvariantCulture, usando NumberStyles.Number y rango 1–10. Los ensayos documentados aceptan coma y punto; no se transforma a double. Los enteros usan PedirInt con sus rangos. Categoría y especialidad se eligen con listas numéricas de cuatro opciones.

`Main` conecta las 12 opciones. Crear, leer y salir funcionan sin competencia previa; las otras opciones muestran el aviso de crear o leer primero. Salir muestra TOP 3 y pausa, sin guardar automáticamente.

## Presentación con Utilidades

El menú principal usa la sobrecarga `PedirOpcionMenu(string[], string, ColoresMenu, FormatoMenu)`. `FormatoMenu` y `ColoresMenu` son structs. Main configura ancho 55, guarda la selección inicial entre operaciones, proporciona leyenda y nombre de competencia, y establece salida UTF-8.

La configuración visible usa texto negro y fondo Gray en la selección, texto White en las otras opciones, texto auxiliar Gray y títulos White. Los otros fondos del menú son Black. Hay marcador ►, navegación circular con flechas y confirmación con Enter. La biblioteca dispone de alternativa numérica cuando la consola no es interactiva.

Los formularios y fichas usan MyConsole y auxiliares de Utilidades, sin bordes en las fichas ni etiquetas automáticas en mensajes. **No se requiere MostrarTabla**, por indicación del usuario del 04/11/2026. La función permanece disponible en la biblioteca para otros usos.

| Operaciones | Fondo |
| --- | --- |
| Registrar equipo, mostrar equipo, estadísticas, comparación y más seguidores | DarkBlue |
| Registrar creador y mostrar creador | DarkMagenta |
| Crear/mostrar competencia y persistencia | Black |
| TOP 3 y cierre | DarkGreen |

Al volver, la pantalla se limpia y la selección se conserva. Las consultas presentan ausencias sin cerrar la aplicación. Los errores de persistencia se capturan en Main y se muestran al usuario. PedirInt informa Ingresá un número entero válido sin etiqueta automática, tanto para formato incorrecto como para valores fuera de rango.

## Persistencia con DataBases

La aplicación llama a Guardar/Leer con `datos`, relativo a su directorio de trabajo. El conjunto vigente está en `datos/actual`; las preparaciones y respaldos están bajo esa misma raíz. No se escribe en `Ejecutable/Datos` ni se implementa Base64.

| Archivo | Columnas, en orden |
| --- | --- |
| competencia.csv | nombre, patrocinador, año, cantMaxEquipos |
| equipos.csv | nombre, nombreCompetencia, fechaDeCreacion, categoria |
| creadores.csv | nombre, alias, nombreEquipo, edad, seguidores, valoracion, especialidad |

`nombreCompetencia` y `nombreEquipo` conservan las asociaciones; no son listas duplicadas ni relaciones gestionadas por un modelo global. DataBases escribe encabezados, separador coma, escape de campos y valores con cultura invariable. Fecha usa formato ISO y valoración conserva decimal. Los constructores corresponden a los nombres y tipos de las columnas. No se persisten estadísticas, Estrella ni resultados de comparación.

### Guardado protegido

1. Exigir competencia activa y resolver la raíz absoluta.
2. Crear una carpeta única preparacion-GUID y un modelo local. Agregar competencia, equipos y creadores en orden y llamar a GuardarTodo.
3. Releer las tres tablas con archivos obligatorios para comprobar su formato y construcción de entidades antes de publicarlas.
4. Si existe actual, moverlo a respaldo-GUID. Promover la preparación a actual.
5. Si falla esa promoción, intentar devolver el respaldo a actual y propagar el error. No borrar los respaldos completos anteriores.

GuardarTodo es atómico por archivo, no entre los tres archivos; la preparación evita escribir directamente sobre la única copia vigente. Una operación fallida puede dejar carpetas de preparación. La secuencia de renombrados no es una transacción universal ni garantiza resistencia a cualquier corte de energía. Las pruebas cubren los fallos controlados documentados.

### Lectura validada y recuperación

1. Si hay un directorio o archivo actual, intentar leer ese conjunto. Un actual corrupto informa error; no se sustituye silenciosamente por un respaldo.
2. Si falta actual, probar los directorios respaldo-* por fecha de modificación descendente, omitiendo los inválidos con los tipos de error previstos. Sin conjunto válido, lanzar IOException. Si no existe la raíz, lanzar DirectoryNotFoundException.
3. LeerConjunto exige los tres archivos con contenido. Equipos y creadores pueden tener solo encabezados; un archivo vacío se rechaza. Cargar las tres tablas y exigir exactamente una competencia.
4. Comprobar nombreCompetencia de cada equipo y registrarlo con validación de duplicados y cupo. Buscar el equipo de cada creador y registrarlo con validación de alias y pertenencia.
5. Devolver el conjunto reconstruido solo al completar todas las altas. Ante error, la competencia activa no cambia. La recuperación no modifica los archivos leídos.

La lectura no usa Modelo.Muchos: reconstruye explícitamente las relaciones con los criterios del dominio. La conservación de una tabla ante un error de lectura no se confunde con una transacción entre tablas.

## Verificación y entrega

El proyecto xUnit contiene 119 casos correctos: Creador 22, Equipo 25, Competencia 39 y Datos 33. Cubre límites, duplicados, cupos, búsquedas, estadísticas, vacíos, empates, precisión, Estrella y persistencia en carpetas temporales. DatosTests restaura la competencia y la cultura y ejecuta sus casos secuencialmente; la prueba de archivo bloqueado es específica de Windows.

El recorrido final conserva 204 comprobaciones correctas, revisión de 23 capturas de entradas inválidas y 110 PNG, con diez pantallas contrastadas a 120×40. Las PNG reconstruyen celdas ANSI; no son fotografías del escritorio ni una prueba de equivalencia para toda terminal, dimensión o entrada. Las pruebas previas de menú a 80×24 y 160×40 se conservan por separado.

Consultar [Pruebas.md](Pruebas.md), [RecorridoFinal.md](RecorridoFinal.md) y el [plan vigente](Plan.md). No hace falta repetir la investigación ni implementar operaciones que la documentación anterior todavía describía como provisionales. El README y el ZIP definitivo ya fueron revisados y verificados; el usuario subirá el DOCX de preguntas y respuestas a Google Docs y facilitará el enlace. El [registro de IA](../IA/Log.md) conserva el historial y las decisiones anteriores sin borrarlas.
