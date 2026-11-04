// Investigación del menú real; no pertenece a la aplicación ni a sus pruebas unitarias finales.
using Utilidades;

Console.OutputEncoding = System.Text.Encoding.UTF8;
var colores = new ColoresMenu
{
    Seleccion = ConsoleColor.Black, Cursor = ConsoleColor.Black,
    NoSeleccion = ConsoleColor.White, TextoNormal = ConsoleColor.Gray,
    FondoSeleccion = ConsoleColor.Gray, FondoCursor = ConsoleColor.Gray,
    TextoMensaje = ConsoleColor.White
};
var formato = new FormatoMenu
{
    Ancho = 55,
    Pie = "Azul: equipos | Violeta: creadores | Negro: general\nCompetencia: Sin crear"
};
string[] opciones =
{
    "Crear competencia", "Registrar equipo", "Registrar creador en un equipo",
    "Mostrar información de la competencia", "Mostrar información de un equipo",
    "Mostrar información de un creador", "Mostrar estadísticas de un equipo",
    "Comparar valoración de dos equipos", "Mostrar el equipo con más seguidores",
    "Guardar datos de la competencia", "Leer datos de la competencia", "Salir"
};
const string titulo = "╔════════════════════════════════════════════════════╗\n"
    + "║        STREAMORT - BATALLA DE CREADORES            ║\n"
    + "╚════════════════════════════════════════════════════╝";

MyConsole.ForegroundColor = ConsoleColor.Gray;
MyConsole.BackgroundColor = ConsoleColor.Black;
MyConsole.Clear();

if (args[0] == "validaciones")
{
    int comprobados = 0;
    foreach (string caso in new[] { "null", "vacio", "ancho", "indiceNegativo", "indiceFuera" })
    {
        var invalido = new FormatoMenu();
        string[]? lista = opciones;
        if (caso == "null") lista = null;
        if (caso == "vacio") lista = Array.Empty<string>();
        if (caso == "ancho") invalido.Ancho = -1;
        if (caso == "indiceNegativo") invalido.SeleccionInicial = -1;
        if (caso == "indiceFuera") invalido.SeleccionInicial = opciones.Length;
        try
        {
            Funciones.PedirOpcionMenu(lista!, titulo, colores, invalido);
            throw new Exception("Se aceptó una configuración inválida.");
        }
        catch (ArgumentException error)
        {
            string esperado = caso == "null" ? "ArgumentNullException"
                : caso == "vacio" ? "ArgumentException" : "ArgumentOutOfRangeException";
            if (error.GetType().Name != esperado) throw;
            comprobados++;
        }
    }
    if (Console.ForegroundColor != ConsoleColor.Gray || Console.BackgroundColor != ConsoleColor.Black
        || !MyConsole.GuardadoActivo) throw new Exception("Se alteró el estado al validar.");
    Console.WriteLine($"VALIDACIONES={comprobados}");
    return;
}

if (args[0] is "categoria" or "especialidad")
{
    ConsoleColor fondo = args[0] == "categoria" ? ConsoleColor.DarkBlue : ConsoleColor.DarkMagenta;
    MyConsole.ForegroundColor = ConsoleColor.White;
    MyConsole.BackgroundColor = fondo;
    MyConsole.Clear();
    MyConsole.WriteLine("Datos anteriores del formulario");
    colores = new ColoresMenu
    {
        NoSeleccion = ConsoleColor.White, TextoNormal = ConsoleColor.White,
        TextoMensaje = ConsoleColor.White, FondoNoSeleccion = fondo,
        FondoTextoNormal = fondo, FondoMensaje = fondo
    };
    formato.SeleccionNumerica = true;
    opciones = args[0] == "categoria"
        ? new[] { "Gaming", "Humor", "Música", "Entretenimiento" }
        : new[] { "Streamer", "Youtuber", "Tiktoker", "Influencer" };
    int indice = Funciones.PedirOpcionMenu(opciones,
        args[0] == "categoria" ? "Categoría:" : "Especialidad:", colores, formato);
    MyConsole.WriteLine($"RESULTADO={indice};REGISTRO={MyConsole.GuardadoActivo}");
    if (!Console.IsInputRedirected) MyConsole.ReadKey(true);
    return;
}

if (args[0] == "largo")
    opciones = Enumerable.Range(1, 80).Select(i => "Opción larga " + i).ToArray();
if (args[0] == "sin-ciclo")
    formato.NavegacionCircular = false;
if (args[0] == "sin-numeros")
    formato.NumerarOpciones = false;
if (args[0] == "sin-registro")
    MyConsole.DesactivarGuardado();

int elegido = Funciones.PedirOpcionMenu(opciones, titulo, colores, formato);
MyConsole.WriteLine($"RESULTADO={elegido};REGISTRO={MyConsole.GuardadoActivo}");
MyConsole.ReadKey(true);
if (args[0] == "flechas")
{
    formato.SeleccionInicial = elegido;
    Funciones.PedirOpcionMenu(opciones, titulo, colores, formato);
}
