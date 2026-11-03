// Sonda auxiliar: compara las dos formas de llamar al mismo menú real.
using Utilidades;

Console.OutputEncoding = System.Text.Encoding.UTF8;
MyConsole.ForegroundColor = ConsoleColor.Magenta;
MyConsole.BackgroundColor = ConsoleColor.DarkBlue;
MyConsole.Clear();

string[] opciones = { "Primera", "Segunda", "Salir" };
int elegido;

if (args[1] == "predeterminados")
{
    elegido = args[0] == "nuevo"
        ? Funciones.PedirOpcionMenu(opciones, "Comparación de colores", new ColoresMenu())
        : Funciones.PedirOpcionMenu(opciones, "Comparación de colores");
}
else if (args[0] == "nuevo")
{
    var colores = new ColoresMenu
    {
        Seleccion = ConsoleColor.Black,
        Cursor = ConsoleColor.Black,
        NoSeleccion = ConsoleColor.White,
        TextoNormal = ConsoleColor.Yellow,
        FondoSeleccion = ConsoleColor.Yellow,
        FondoCursor = ConsoleColor.Gray,
        FondoNoSeleccion = ConsoleColor.DarkBlue,
        FondoTextoNormal = ConsoleColor.DarkMagenta,
        TextoMensaje = ConsoleColor.Cyan,
        FondoMensaje = ConsoleColor.DarkRed
    };
    elegido = Funciones.PedirOpcionMenu(opciones, "Comparación de colores", colores);
}
else
{
    elegido = Funciones.PedirOpcionMenu(opciones, "Comparación de colores",
        seleccion: ConsoleColor.Black, cursor: ConsoleColor.Black,
        noSeleccion: ConsoleColor.White, textoNormal: ConsoleColor.Yellow,
        fondoSeleccion: ConsoleColor.Yellow, fondoCursor: ConsoleColor.Gray,
        fondoNoSeleccion: ConsoleColor.DarkBlue, fondoTextoNormal: ConsoleColor.DarkMagenta,
        textoMensaje: ConsoleColor.Cyan, fondoMensaje: ConsoleColor.DarkRed);
}

bool restaurados = MyConsole.ForegroundColor == ConsoleColor.Magenta
    && MyConsole.BackgroundColor == ConsoleColor.DarkBlue;
MyConsole.WriteLine($"RESULTADO={elegido};COLORES_RESTAURADOS={restaurados}");
MyConsole.ReadKey(true);
