// Sonda de investigación: llama las utilidades reales sin modificar la aplicación.
using Utilidades;

Console.OutputEncoding = System.Text.Encoding.UTF8;
MyConsole.ForegroundColor = ConsoleColor.White;
MyConsole.BackgroundColor = ConsoleColor.DarkBlue;
MyConsole.Clear();

if (args[0] == "categoria")
{
    Funciones.PedirEnum<CategoriaSonda>("Categoría:");
}
else if (args[0] == "tabla")
{
    DecoracionConsola.MostrarTabla(new string[,]
    {
        { "Equipo:", "Alpha" },
        { "Creación:", "01/01/2000" },
        { "Categoría:", "Gaming" }
    });
    MyConsole.ReadKey(true);
}
else if (args[0] == "mensajes")
{
    DecoracionConsola.ImprimirMesajeDeError("Ese equipo ya existe.");
    DecoracionConsola.ImprimirMesajeDeExito("Equipo registrado.");
    MyConsole.ReadKey(true);
}

enum CategoriaSonda { Gaming, Humor, Musica, Entretenimiento }
