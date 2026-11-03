# Arquitectura de la reconstrucción de StreamORT

Fecha: 03/11/2026. Estado: dominio de Creador, Equipo y Competencia implementado; menú y persistencia pendientes, sin UML.

## Alcance y referencias

La aplicación será una consola C# para .NET 8, con una sola competencia activa. Este diseño parte de [Analisis.md](Analisis.md), [Pruebas-StreamORT.md](Pruebas-StreamORT.md), [Contexto.md](Contexto.md) y del código actual de la aplicación y sus bibliotecas. No se volvió a ejecutar el original en esta tarea: se utiliza la investigación ya documentada.

Las reglas funcionales descritas como comprobadas proceden de esos ensayos. Los tipos, las firmas, el reparto de responsabilidades y la protección del guardado son propuestas para la reconstrucción; no describen clases internas del original. No se agregan edición, eliminación, partidas ni administración simultánea de competencias.

## Estado actual

- `Competencia` valida textos, año y máximo de equipos, inicializa su lista privada y devuelve copias. Implementa cupo, búsqueda y registro con `RegistroException`, comparación por promedio decimal, equipo con más seguidores y TOP 3. Solo queda `Configurar` para persistencia.
- `Equipo` valida nombre, fecha no futura y categoría, inicializa su lista privada y devuelve copias al consultarla. Implementa búsqueda, registro con `RegistroException` y las cuatro estadísticas. Solo queda `Configurar`, reservado para la persistencia integral.
- `Creador` inicializa y valida sus siete datos. Los textos se guardan sin espacios exteriores y la valoración conserva su precisión decimal. `EsEstrella()` está implementado: seguidores >= 500 y valoración >= 8, sin redondeo ni condición de especialidad. Solo queda la configuración CSV, pospuesta por indicación del usuario hasta implementar la persistencia del proyecto completo.
- `Datos` expone una propiedad `Competencia?` con get y set públicos, sin lógica personalizada, y declara las operaciones de persistencia, aún provisionales.
- `Program` muestra el menú, pero los casos 1 a 11 están vacíos y falta el TOP 3. Su control de acceso bloquea incorrectamente la lectura sin competencia.
- Los métodos auxiliares de `Program` y la persistencia conservan cuerpos provisionales con `NotImplementedException`; las operaciones de dominio y los getters de colecciones ya están implementados. Las clases persistidas implementan `IEntidad<T>` mediante `Configurar` provisional, reservado para la etapa 5. La compilación actual pasa sin errores y con las advertencias previas CA1416 y CS0219.
- Por indicación del usuario, los registros tienen firmas `void` sin parámetros de salida. `RegistroException` conserva el mensaje recibido mediante `base(mensaje)`. Ambos registros ya validan sus condiciones y lanzan esta excepción antes de modificar sus listas ante un rechazo; falta capturarla en los flujos del menú.

## Organización y propiedad de los objetos

`Program` recibe entradas, invoca operaciones y muestra resultados. `Datos` conserva la competencia activa y coordina los archivos. `Competencia` es dueña de una lista de equipos; cada `Equipo` es dueño de una lista de creadores. `Creador` conserva sus datos y calcula si es estrella.

Por indicación del usuario se usarán propiedades para acceder a los datos: solo `get` si se fijan al construir el objeto y `private set` si la clase necesita reemplazar su valor después. `Datos.competencia` tiene `set` público por indicación expresa del usuario, sin lógica personalizada. Las consultas directas de colecciones serán propiedades de lectura; las búsquedas, altas y cálculos seguirán como métodos.

Las listas se almacenan en propiedades privadas de solo lectura, inicializadas en los constructores: `List<Creador> creadoresRegistrados` en `Equipo` y `List<Equipo> equiposRegistrados` en `Competencia`. Los getters públicos devuelven copias; agregar o quitar elementos de esas copias no altera los registros internos. Las altas pasan por los métodos responsables de validar duplicados y cupos.

No habrá listas globales de equipos o creadores en `Datos`. Las tablas de `DataBases` serán estructuras temporales para guardar o reconstruir el mismo conjunto de objetos, nunca un segundo estado editable de la aplicación. Se conserva el orden de registro y carga.

Las clases de dominio no leen teclas ni imprimen mensajes. Devuelven objetos, números, listas o resultados de comparación; los rechazos de registro se comunican con excepciones personalizadas. `Program` decide su presentación.

## Tipos y criterios comunes

| Dato | Tipo propuesto | Motivo |
| --- | --- | --- |
| Nombres, patrocinador y alias | `string` | Recortar espacios exteriores y rechazar vacío o solo espacios. Conservar espacios interiores y tildes. |
| Año, cupo y edad | `int` | Rangos comprobados: 2000–2027, 1–10000 y 14–30. |
| Fecha de creación | `DateOnly` | No tiene hora y no puede ser posterior a hoy. |
| Seguidores de un creador | `int` | Entre 0 e `int.MaxValue`, en miles. |
| Total de seguidores | `long` | Se comprobó un total de 2147483648 mil. Convertir a `long` antes de sumar. |
| Valoración y promedio | `decimal` | Admitir decimales y conservar el valor antes del formato visual. Es una elección del diseño, no un tipo identificado en el original. |
| Competencia o búsqueda ausente | `Competencia?`, `Equipo?`, `Creador?` | Representar explícitamente la ausencia con `null`. |

No redondear antes de calcular Estrella, comparar o ordenar. Por ejemplo, 7,999 no cumple valoración >= 8, y 8,004 supera a 8,001 aunque ambos promedios se muestren como 8,00. `decimal` tiene precisión finita: no se afirma que represente cualquier longitud de entrada; se verificará al menos la precisión de las muestras investigadas.

Para búsquedas y duplicados se propone `StringComparison.OrdinalIgnoreCase` después de `Trim()`. No quitar tildes, normalizar acentos combinantes ni comprimir espacios interiores. Esta elección debe contrastarse con las muestras Unicode: los ensayos no identifican el comparador universal del original.

El TOP 3 implementa un comparador cultural explícito `es-AR`, sin distinguir mayúsculas, y ordena por promedio descendente y luego nombre. Usa inserción estable: si dos nombres son culturalmente equivalentes conserva su orden de registro. La comprobación auxiliar reproduce el orden de la muestra original `Aguila`, `águila`, `Águila`, también al cambiar CurrentCulture entre `en-US` y `es-AR`. Se verificó sobre Windows con .NET 10; el comparador es una elección de diseño contrastada con esa muestra, sin identificar el comparador interno del original ni garantizar equivalencia para todo Unicode o plataforma.

## Clases y contratos propuestos

Las firmas y propiedades siguientes representan el diseño acordado y ya están declaradas en el proyecto, incluidos los registros sin parámetro de salida y `RegistroException`. Su constructor está implementado; los métodos y getters provisionales conservan pendiente la lógica descrita en sus contratos. Los miembros indicados como públicos pertenecen a clases `internal`.

### Creador

Responsabilidad: representar un creador válido y resolver sus reglas individuales.

Propiedades de solo lectura: `string nombre`, `string alias`, `string nombreEquipo`, `int edad`, `int seguidores`, `decimal valoracion` y `Especialidad especialidad`. Se conserva `nombreEquipo` para persistir la asociación existente; `Equipo` verificará que coincida con su nombre y no habrá una operación para trasladarlo.

```csharp
public Creador(string nombre, string alias, string nombreEquipo, int edad,
    int seguidores, decimal valoracion, Especialidad especialidad);
public bool EsEstrella();
public static void Configurar(ConstructorEntidad<Creador> entidad);
```

El constructor inicializa todos los atributos, valida textos, edad 14–30, seguidores >= 0, valoración 1–10 y especialidad definida. Los límites de `int` forman parte del tipo de seguidores. La unicidad del alias corresponde al equipo, no al constructor del creador.

`EsEstrella()` devuelve `seguidores >= 500 && valoracion >= 8`, sin redondear ni condicionar por especialidad. `Configurar` registra únicamente los siete datos persistidos mediante `IEntidad<Creador>`; no guarda Estrella.

### Equipo

Responsabilidad: representar un equipo, gestionar sus creadores y calcular estadísticas.

Propiedades de solo lectura implementadas: `string nombre`, `DateOnly fechaDeCreacion`, `Categoria categoria` y `List<Creador> creadores`. La propiedad privada de solo lectura `List<Creador> creadoresRegistrados` almacena los registros y se inicializa en el constructor. Todos los contratos de esta clase están implementados excepto `Configurar`, pospuesto hasta la persistencia del proyecto completo por indicación del usuario.

```csharp
public Equipo(string nombre, DateOnly fechaDeCreacion, Categoria categoria);
public List<Creador> creadores { get; }
public Creador? BuscarCreador(string alias);
public void RegistrarCreador(Creador creador);
public decimal CalcularValoracionPromedio();
public int ContarStreamers();
public long CalcularTotalSeguidores();
public Creador? ObtenerCreadorMasPopular();
public static void Configurar(ConstructorEntidad<Equipo> entidad);
```

| Operación | Contrato |
| --- | --- |
| Constructor | Validar nombre requerido, fecha no futura y categoría definida; inicializar lista vacía. |
| `creadores.get` | Devolver copia de la lista, respetando orden de registro. Sin setter. |
| `BuscarCreador` | Buscar alias dentro del equipo con los criterios comunes; devolver `null` si no existe. |
| `RegistrarCreador` | Ante alias repetido, objeto nulo o asociación con otro equipo, lanzar `RegistroException` con el motivo, sin modificar la lista. Con éxito agregar y terminar normalmente; no devuelve valor. No inventar cupo de creadores. |
| `CalcularValoracionPromedio` | Sumar valoraciones y dividir por cantidad; devolver 0 si está vacío. |
| `ContarStreamers` | Contar exclusivamente especialidad `Streamer`. |
| `CalcularTotalSeguidores` | Sumar en miles con acumulador `long`. |
| `ObtenerCreadorMasPopular` | Mayor número de seguidores; en empate conservar el primero. Devolver `null` si está vacío. |
| `Configurar` | Mapear nombre, fecha y categoría; excluir la lista de creadores. |

### Competencia

Responsabilidad: datos generales, cupos, altas y búsquedas de equipos, comparación y rankings.

Propiedades de solo lectura: `string nombre`, `string patrocinador`, `int año`, `int cantMaxEquipos` y `List<Equipo> equipos`. Al implementar la lista se agregará la propiedad privada de solo lectura `List<Equipo> equiposRegistrados`.

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

El constructor mantiene los límites actuales, valida textos e inicializa la lista vacía. El getter de `equipos` devuelve una copia; no tiene setter. `HayCupo` compara cantidad con máximo. `BuscarEquipo` devuelve `null` si no encuentra el nombre. `RegistrarEquipo` lanza `RegistroException` ante objeto nulo, nombre duplicado o cupo agotado, con el motivo y sin alterar el estado; con éxito agrega y termina normalmente, sin devolver valor.

`CompararEquipos` valida primero el primer equipo, después el segundo, que sean diferentes y que ambos tengan creadores. Devuelve un valor de `ResultadoComparacion`: `PrimerEquipoInexistente`, `SegundoEquipoInexistente`, `MismoEquipo`, `EquipoSinCreadores`, `Empate`, `GanaPrimero` o `GanaSegundo`. Se propone este enum para que `Program` no tenga que comparar textos ni recalcular ganadores. Los promedios para mostrar se consultan en los equipos encontrados.

`ObtenerEquipoConMasSeguidores` excluye equipos vacíos y conserva el primero entre los empatados. Participan los equipos con creadores y cero seguidores. Devuelve `null` cuando ninguno tiene creadores.

`ObtenerTop3` devuelve una lista nueva de hasta tres equipos con creadores, ordenada por promedio descendente y nombre en los empates. Una competencia sin candidatos devuelve una lista vacía. No altera el orden de registro de la lista original. Puede resolverse con recorridos y una lista auxiliar, sin exigir técnicas todavía no estudiadas.

`Configurar` mapea solo los cuatro datos generales. Los cupos ocupados se calculan contando equipos y no se persisten.

### Enumeraciones

Se conservan todas las enumeraciones en `ENUMs.cs` por indicación del usuario: `Categoria` con `Gaming`, `Humor`, `Musica`, `Entretenimiento`; `Especialidad` con `Streamer`, `Youtuber`, `Tiktoker`, `Influencer`; y `ResultadoComparacion` con los siete estados anteriores. Mostrar «Música» en la interfaz. No asignar significado a valores fuera de las enumeraciones; rechazarlos también al cargar archivos.

### RegistroException

Responsabilidad: comunicar un rechazo de registro con un mensaje que `Program` pueda mostrar. Se propone una única excepción personalizada para mantener el diseño sencillo: clase `internal RegistroException : Exception`, en `RegistroException.cs` dentro del proyecto principal. Su constructor público recibe `string mensaje` y lo pasa al constructor de `Exception` mediante `base(mensaje)`, de modo que quede disponible en `Message`.

```csharp
public RegistroException(string mensaje);
```

`RegistrarEquipo` y `RegistrarCreador` deben comprobar todas sus condiciones antes de agregar a la lista. Si alguna falla, lanzan esta excepción; si terminan normalmente, el registro fue exitoso. No devuelven indicadores de éxito ni mensajes mediante parámetros de salida. Por ejemplo, el motivo puede ser «Ya existe un equipo con ese nombre», «No hay cupo para otro equipo» o «El creador pertenece a otro equipo».

Al conectar las altas del menú, `Program` capturará `RegistroException` mediante `try/catch` y mostrará su `Message`, sin finalizar la aplicación. Los constructores mantienen `ArgumentException` para datos individuales inválidos; los errores de archivos conservan sus tipos propios. Las búsquedas y los cálculos mantienen sus contratos actuales.

### Datos

Responsabilidad: conservar la única competencia activa y coordinar la persistencia, sin imprimir ni pedir confirmaciones.

```csharp
public static Competencia? competencia { get; set; }
public static void Guardar(string directorio);
public static Competencia Leer(string directorio);
private static Modelo CrearModelo(string directorio);
```

`Program` asigna directamente `Datos.competencia` después de confirmar el reemplazo y construir por completo el nuevo objeto. No hay un método separado de reemplazo ni validaciones en el setter. El valor `null` representa la ausencia de competencia activa.

`Guardar` exige competencia activa y obtiene de ella los equipos y creadores. No cambia la memoria. `Leer` devuelve una competencia nueva completa y validada, sin cambiar `competencia`: solo después de una lectura correcta `Program` la asigna a `Datos.competencia`.

Los errores de archivos o formato se propagan para que `Program` los informe. No convertir un error en una competencia vacía ni ignorar filas inválidas. Los constructores lanzan `ArgumentException` para datos individuales inválidos y los rechazos de altas lanzan `RegistroException`. Si esta excepción ocurre al reconstruir las relaciones durante `Leer`, se propaga hasta `Program`: se descarta la competencia temporal y no se reemplaza el estado activo.

### Program

Responsabilidad: navegación, entradas, confirmaciones y presentación usando `Utilidades`.

Se propone completar las firmas existentes y agregar auxiliares conforme se conecte cada flujo:

```csharp
static void Main(string[] args);
static Competencia CrearCompetencia();
static Equipo CrearEquipo();
static Creador CrearCreador(string nombreEquipo);
static string PedirTextoRequerido(string mensaje);
static DateOnly PedirFechaEquipo();
static decimal PedirValoracion();
static bool ConfirmarReemplazo(string mensaje);
static void MostrarCompetencia(Competencia competencia);
static void MostrarEquipo(Equipo equipo);
static void MostrarCreador(Creador creador);
static void MostrarEstadisticas(Equipo equipo);
static void MostrarComparacion(Competencia competencia);
static void MostrarTop3(Competencia? competencia);
```

`CrearCreador` recibe el nombre del equipo ya encontrado, evitando pedirlo dos veces. Los métodos de creación devuelven objetos válidos; los casos del menú registran esos objetos en su dueño. Cupos y duplicados deben consultarse antes de solicitar los campos restantes, según el orden observado, y comprobarse nuevamente en los métodos de alta.

`ConfirmarReemplazo` lee una tecla: `s` o `S` confirma; las otras teclas ensayadas cancelan. No usar directamente `PedirBool`, que actualmente exige `y/n` y repregunta.

`PedirFechaEquipo` usa `PedirString` y `DateOnly.TryParseExact` con `dd/MM/yyyy`, además de rechazar fechas futuras. `PedirDateOnly` usa un análisis flexible y no garantiza ese contrato.

`PedirValoracion` usa `PedirString` y `decimal.TryParse`, admitiendo coma o punto decimal sin separadores de miles y validando 1–10. No pasar por `PedirDouble` para después convertir a `decimal`, porque introduciría una conversión innecesaria antes de guardar. Los enteros usan `PedirInt`; las categorías y especialidades usan `PedirOpcionMenu` o `PedirEnum`, que ya lo utiliza. Validar textos en el constructor y también al solicitarlos permite repreguntar inmediatamente.

Las tablas se arman como `string[,]` para `MostrarTabla`. Promedios, unidades, «Sin creadores» y «Estrella: Sí/No» son presentación. Aplicar fondo negro a operaciones generales, azul a equipos y violeta a creadores, restaurando la pantalla al volver.

## Correspondencia con el menú

| Opción | Flujo propuesto |
| --- | --- |
| 1 | Confirmar si hay competencia, crear objeto y reemplazar referencia; los equipos anteriores dejan de formar parte del estado activo. |
| 2 | Consultar cupo y duplicado; crear equipo y registrarlo en la competencia. |
| 3 | Buscar equipo, consultar alias, crear creador y registrarlo en ese equipo. |
| 4 | Consultar competencia y recorrer equipos y creadores para mostrar tablas. |
| 5 | Buscar y mostrar equipo, incluidos sus creadores. |
| 6 | Buscar equipo y luego creador por alias; mostrar ficha. |
| 7 | Buscar equipo y mostrar los cuatro cálculos definidos en `Equipo`. |
| 8 | Pedir y validar el primer equipo antes de pedir el segundo; usar `CompararEquipos` y mostrar sus promedios. |
| 9 | Consultar `ObtenerEquipoConMasSeguidores` o informar ausencia de candidatos. |
| 10 | Invocar `Datos.Guardar`, informar éxito o error sin cambiar la memoria. |
| 11 | Confirmar si hay competencia; ejecutar `Datos.Leer` y reemplazar solo tras éxito completo. |
| 12 | Mostrar TOP 3 y esperar una tecla. No guardar automáticamente. |

Crear, leer y salir funcionan sin competencia previa. Las opciones 2 a 10 requieren competencia. Al iniciar no se cargan archivos automáticamente. Informar búsquedas fallidas y listas vacías con los mensajes respaldados por el análisis.

La interfaz debe ser idéntica a la del ejecutable, por la nueva indicación del usuario del 03/11/2026. Se reemplaza la prioridad anterior que admitía diferencias visuales. El original conserva selección y navegación circular. La firma pública actual de `PedirOpcionMenu` no recibe selección inicial; contrastar y adaptar ese comportamiento en la nueva etapa 3.5 de `Plan.md`, antes de conectar el menú. El usuario solicita adaptar las bibliotecas y que `PedirOpcionMenu` reciba un `struct ColoresMenu` para agrupar los colores sin multiplicar parámetros. Los campos y la firma se definirán antes de implementar, a partir del formato observado. `Mis Librerias` ya es un submódulo del repositorio de bibliotecas indicado por el usuario: https://github.com/51215927-spec/Mis-Librerias.git (rama predeterminada `main`, acceso `ADMIN` verificado con `gh` el 03/11/2026); conservar archivos, contratos y referencias actuales. La conversión fija el commit `a78e2c55e5e469e22874aab9d3bd7f87e285141b`, con solución de bibliotecas independiente y referencias principales conservadas. Ambas soluciones compilan; las adaptaciones visuales y `ColoresMenu` siguen sin implementar. La persistencia continúa en CSV sin reproducir Base64.

## Persistencia con DataBases

### Formato y configuración

Se proponen `competencia.csv`, `equipos.csv` y `creadores.csv`, independientes de los `.txt` del original. No escribir en `Ejecutable/Datos` ni implementar importación Base64 como parte de esta tarea.

| Archivo | Columnas propuestas |
| --- | --- |
| `competencia.csv` | `nombre`, `patrocinador`, `año`, `cantMaxEquipos` |
| `equipos.csv` | `nombre`, `fechaDeCreacion`, `categoria` |
| `creadores.csv` | `nombre`, `alias`, `nombreEquipo`, `edad`, `seguidores`, `valoracion`, `especialidad` |

La biblioteca escribe encabezados, separador coma, escape de comillas y valores con cultura invariable. Soporta `DateOnly`, `decimal` y enums. No crear un parser CSV propio ni concatenar campos manualmente. No persistir listas ni resultados calculados.

Cada clase persistida implementará `IEntidad<T>` y su `Configurar`. El constructor de cada clase debe tener exactamente los parámetros correspondientes a las columnas, con nombres y tipos compatibles: `CsvManager` los exige al construir el modelo. Las firmas propuestas cumplen esa condición.

`CrearModelo` usa `new ConstructorModelo().Agregar<Competencia>().Agregar<Equipo>().Agregar<Creador>().Construir()`. Como `Configurar` es estático y no recibe directorio, se propone un campo privado temporal de configuración en `Datos`, accesible mediante una propiedad interna para formar las rutas absolutas. Solo se modifica mientras se construye el modelo y se restaura con `finally`; las rutas quedan resueltas en `Archivo`. Es una solución para esta consola secuencial, sin cambiar el directorio de trabajo global ni modificar la biblioteca; no se diseña para persistencia concurrente.

No utilizar `Modelo.Muchos` como almacenamiento del dominio: devuelve una copia y su resolución de claves usa `Equals`, que por sí sola no reproduce las búsquedas acordadas sin distinguir mayúsculas. Reconstruir las relaciones explícitamente con los métodos de alta.

### Lectura completa antes de reemplazar memoria

1. Crear un modelo local y cargar cada tabla con `Cargar<T>(exigirArchivo: true)`. `CargarTodo` permite archivos inexistentes y no ofrece este contrato.
2. Exigir exactamente una fila de competencia. Los archivos de equipos y creadores pueden tener solo encabezados cuando no hay registros; rechazar un archivo completamente vacío como política de integridad propuesta, aunque la biblioteca lo interpreta como tabla vacía.
3. Registrar los equipos en la competencia temporal, validando cupo y nombres únicos.
4. Asociar cada creador por `nombreEquipo` y registrarlo con validación de alias. Rechazar creadores sin equipo, filas inválidas y duplicados; conservar el orden de carga.
5. Devolver la competencia temporal solo cuando todo sea válido. Ante error, descartar esa construcción y mantener el estado activo anterior.

No cargar directamente sobre un modelo global activo: aunque una tabla conserva sus datos si falla su propia lectura, eso no convierte la carga de varias tablas en una operación indivisible.

### Guardado protegido

El código vigente escribe cada archivo mediante un temporal, pero el comentario y la implementación de `Modelo.GuardarTodo()` aclaran que **no es una transacción entre archivos**. No alcanza con llamar a ese método sobre la única copia válida.

Propuesta sin modificar `DataBases`:

1. Crear una carpeta de preparación única dentro del directorio de datos; construir allí un modelo y cargar tablas inicialmente vacías.
2. Agregar una fila de competencia, todos los equipos y todos los creadores a sus `Tabla<T>`, y llamar a `GuardarTodo` en esa carpeta nueva.
3. Releer ese conjunto y validar todos sus datos y relaciones antes de publicarlo.
4. Mantener el conjunto anterior en una carpeta de respaldo y promover la carpeta preparada como `actual`. Si la promoción falla, intentar restaurar el respaldo e informar el error; conservar el respaldo aunque la restauración falle.
5. Al leer, si falta `actual` por una promoción interrumpida, permitir recuperar el respaldo completo y validarlo. Un conjunto actual corrupto debe generar un error explícito, sin sustituirlo silenciosamente.

Usar operaciones de carpetas únicamente sobre rutas resueltas dentro del directorio de datos, con nombres internos definidos por la aplicación. No borrar la única copia válida. La secuencia de renombrados no equivale a una transacción del sistema; exige ensayos de recuperación y no permite prometer resistencia universal a cortes de energía. La garantía propuesta es conservar una copia completa recuperable ante los fallos de escritura controlados.

## Implementación pendiente y orden recomendado

1. Compilación recuperada y contratos declarados. Los cuerpos provisionales permanecen desconectados del menú hasta implementarse.
2. Dominio completo: `Creador`, `Equipo` y `Competencia` implementan sus validaciones, listas, búsquedas, registros con excepción, estadísticas, comparación y TOP 3. Las enumeraciones están en `ENUMs.cs`. `Configurar` se reserva para la persistencia integral.
3. `Datos` ya representa la ausencia de competencia con una propiedad `Competencia?` de get/set públicos y reemplazo mediante una sola referencia; sus métodos de persistencia permanecen provisionales.
4. Conectar las 12 opciones, corregir acceso a lectura, entradas, confirmaciones, tablas y colores. Comprobar navegación con la API disponible.
5. Implementar configuración CSV, guardado protegido, reconstrucción temporal y recuperación con copias de datos.
6. Crear un proyecto separado de pruebas unitarias. Cubrir límites, duplicados por equipo, cupos, vacíos, Estrella, precisión, totales amplios y desempates. Verificar que las altas rechazadas lancen `RegistroException` sin modificar las listas. Probar persistencia con directorios temporales, incluidos fallos durante la publicación y recuperación del respaldo y rechazos de registro durante la reconstrucción.
7. Compilar, ejecutar pruebas y recorrer manualmente los flujos del menú contra la evidencia. Contrastar especialmente el comparador Unicode y el formato de redondeo, sin asumir equivalencia universal.
8. Actualizar documentos y registro de IA; preparar y comprobar el ZIP completo y reunir las consultas y respuestas en el Google Docs requerido.

No hace falta repetir toda la investigación inicial: agregar ensayos concretos cuando surja una duda que afecte una decisión. Mantener cambios atómicos, explicación, commit y push según las reglas del repositorio. El dominio está implementado; el menú todavía no invoca sus operaciones ni captura los rechazos y la persistencia permanece pendiente. Las comprobaciones auxiliares no sustituyen las pruebas unitarias formales ni el recorrido completo previstos para la entrega.
