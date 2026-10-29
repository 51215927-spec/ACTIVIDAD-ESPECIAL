using Microsoft.VisualBasic;
using System.Net;
using Utilidades;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Utilidades.DecoracionConsola;
using static Utilidades.Funciones;


namespace ACTIVIDAD_ESPECIAL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string TITULO_REGISTRAR_EQUIPO =
            """
            ============================================
                    STREAMORT | REGISTRAR EQUIPO
            ============================================
            """;

            const string TITULO_MENU_PRINCIPAL =
            """
            ╔════════════════════════════════════════════════════╗
            ║        STREAMORT - BATALLA DE CREADORES            ║
            ╚════════════════════════════════════════════════════╝
            """;

            string[] menu =
            {
                "1. Crear competencia",
                "2. Registrar equipo",
                "3. Registrar creador en un equipo",
                "4. Mostrar información de la competencia",
                "5. Mostrar información de un equipo",
                "6. Mostrar información de un creador",
                "7. Mostrar estadísticas de un equipo",
                "8. Comparar valoración de dos equipos",
                "9. Mostrar el equipo con más seguidores",
                "10. Guardar datos de la competencia",
                "11. Leer datos de la competencia",
                "12. Salir"
            };

            int opcion;

            while (true) 
            {
                opcion = PedirOpcionMenu(
                    menu,
                    TITULO_MENU_PRINCIPAL,
                    seleccion: ConsoleColor.Black,
                    cursor: ConsoleColor.Black,
                    fondoCursor: ConsoleColor.White,
                    fondoSeleccion: ConsoleColor.White,
                    noSeleccion: ConsoleColor.White,
                    textoNormal: ConsoleColor.White,
                    textoMensaje: ConsoleColor.White
                ) + 1; // para que sea 1-12 en ves de 0-11

                if (opcion == 12)
                {
                    // Mostrar el top 3
                    MyConsole.ReadKey(true);
                    break;
                }

                if (opcion != 1 && Datos.competencia == null)
                {
                    MyConsole.WriteLine("Primero tenés que crear o leer una competencia.");
                    MyConsole.WriteLine("Presioná cualquier tecla para volver al menú...");
                    MyConsole.ReadKey(true);
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        break;

                    case 2:                        
                        break;

                    case 3:
                        break;

                    case 4:
                        break;

                    case 5:
                        break;

                    case 6:
                        break;                    

                    case 7:
                        break; 

                    case 8:
                        break;

                    case 9:
                        break;

                    case 10:
                        break;

                    case 11:
                        break;
                }

                WaitAndClean();
            }
        }

        static Competencia CrearCompetencia() { }
        static Equipo CrearEquipo() { }
        static Creador CrearCreador() { }
    }
}
