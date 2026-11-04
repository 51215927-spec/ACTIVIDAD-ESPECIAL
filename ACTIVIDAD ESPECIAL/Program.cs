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
            Console.OutputEncoding = System.Text.Encoding.UTF8;
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
                "Crear competencia",
                "Registrar equipo",
                "Registrar creador en un equipo",
                "Mostrar información de la competencia",
                "Mostrar información de un equipo",
                "Mostrar información de un creador",
                "Mostrar estadísticas de un equipo",
                "Comparar valoración de dos equipos",
                "Mostrar el equipo con más seguidores",
                "Guardar datos de la competencia",
                "Leer datos de la competencia",
                "Salir"
            };

            var coloresMenu = new ColoresMenu
            {
                Seleccion = ConsoleColor.Black,
                Cursor = ConsoleColor.Black,
                NoSeleccion = ConsoleColor.White,
                TextoNormal = ConsoleColor.Gray,
                FondoSeleccion = ConsoleColor.Gray,
                FondoCursor = ConsoleColor.Gray,
                TextoMensaje = ConsoleColor.White
            };
            var formatoMenu = new FormatoMenu { Ancho = 55 };
            int seleccionInicial = 0;
            int opcion;

            while (true) 
            {
                formatoMenu.SeleccionInicial = seleccionInicial;
                formatoMenu.Pie = "Azul: equipos | Violeta: creadores | Negro: general\n"
                    + "Competencia: " + (Datos.competencia?.nombre ?? "Sin crear");
                opcion = PedirOpcionMenu(
                    menu,
                    TITULO_MENU_PRINCIPAL,
                    coloresMenu,
                    formatoMenu
                ) + 1; // El menú devuelve índices desde cero; el switch usa opciones de 1 a 12.
                seleccionInicial = opcion - 1;

                MyConsole.ForegroundColor = ConsoleColor.White;
                MyConsole.BackgroundColor = ConsoleColor.Black;
                MyConsole.Clear();
                MyConsole.ReinicializarRegistros();

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
                        Competencia? competenciaNueva = CrearCompetencia();
                        if (competenciaNueva != null)
                        {
                            Datos.competencia = competenciaNueva;
                            MyConsole.WriteLine("Competencia creada.");
                        }
                        break;

                    case 2:
                        Equipo? equipoNuevo = CrearEquipo();
                        if (equipoNuevo != null)
                        {
                            try
                            {
                                Datos.competencia.RegistrarEquipo(equipoNuevo);
                                MyConsole.WriteLine("Equipo registrado.");
                            }
                            catch (RegistroException excepcion)
                            {
                                MyConsole.WriteLine(excepcion.Message);
                            }
                        }
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

                MyConsole.WriteLine("\nPresioná cualquier tecla para volver al menú...");
                WaitAndClean();
            }
        }

        static Competencia? CrearCompetencia()
        {
            if (Datos.competencia != null &&
                !ConfirmarReemplazo("¿Reemplazar la competencia actual y sus datos? (S/N):"))
                return null;

            while (true)
            {
                try
                {
                    string nombre = PedirTextoRequerido("Nombre de la competencia: ");
                    string patrocinador = PedirTextoRequerido("Patrocinador: ");
                    int año = PedirInt("Año (2000 a 2027): ", 2000, 2027);
                    int cantMaxEquipos = PedirInt("Cantidad máxima de equipos (1 a 10000): ", 1, 10000);
                    return new Competencia(nombre, patrocinador, año, cantMaxEquipos);
                }
                catch (ArgumentException excepcion)
                {
                    MyConsole.WriteLine(excepcion.Message);
                    MyConsole.WriteLine("Volvé a ingresar los datos de la competencia.");
                }
            }
        }
        static Equipo? CrearEquipo()
        {
            if (Datos.competencia == null || !Datos.competencia.HayCupo())
            {
                MyConsole.WriteLine("No quedan cupos.");
                return null;
            }

            while (true)
            {
                try
                {
                    string nombre = PedirTextoRequerido("Nombre del equipo: ");
                    DateOnly fecha = PedirFechaEquipo();
                    int opcionCategoria = PedirInt("Categoría (1 a 4): ", 1, 4);
                    return new Equipo(nombre, fecha, (Categoria)(opcionCategoria - 1));
                }
                catch (ArgumentException excepcion)
                {
                    MyConsole.WriteLine(excepcion.Message);
                    MyConsole.WriteLine("Volvé a ingresar los datos del equipo.");
                }
            }
        }
        static Creador CrearCreador(string nombreEquipo)
        {
            throw new NotImplementedException("La creación de creadores está pendiente de implementación.");
        }

        static string PedirTextoRequerido(string mensaje)
        {
            while (true)
            {
                string texto = PedirString(mensaje).Trim();
                if (!string.IsNullOrWhiteSpace(texto))
                    return texto;

                MyConsole.WriteLine("El dato no puede estar vacío.");
            }
        }

        static DateOnly PedirFechaEquipo()
        {
            while (true)
            {
                string texto = PedirString("Fecha de creación (dd/MM/yyyy): ");
                if (DateOnly.TryParseExact(texto, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateOnly fecha) &&
                    fecha <= DateOnly.FromDateTime(DateTime.Today))
                    return fecha;

                MyConsole.WriteLine("Ingresá una fecha válida con formato dd/MM/yyyy y que no sea futura.");
            }
        }

        static decimal PedirValoracion()
        {
            throw new NotImplementedException("La entrada de valoración está pendiente de implementación.");
        }

        static bool ConfirmarReemplazo(string mensaje)
        {
            MyConsole.Write(mensaje);
            ConsoleKeyInfo tecla = MyConsole.ReadKey(true);
            MyConsole.WriteLine("");
            return tecla.KeyChar is 's' or 'S';
        }

        static void MostrarCompetencia(Competencia competencia)
        {
            throw new NotImplementedException("La presentación de la competencia está pendiente de implementación.");
        }

        static void MostrarEquipo(Equipo equipo)
        {
            throw new NotImplementedException("La presentación de equipos está pendiente de implementación.");
        }

        static void MostrarCreador(Creador creador)
        {
            throw new NotImplementedException("La presentación de creadores está pendiente de implementación.");
        }

        static void MostrarEstadisticas(Equipo equipo)
        {
            throw new NotImplementedException("La presentación de estadísticas está pendiente de implementación.");
        }

        static void MostrarComparacion(Competencia competencia)
        {
            throw new NotImplementedException("La presentación de comparaciones está pendiente de implementación.");
        }

        static void MostrarTop3(Competencia? competencia)
        {
            throw new NotImplementedException("La presentación del TOP 3 está pendiente de implementación.");
        }
    }
}
