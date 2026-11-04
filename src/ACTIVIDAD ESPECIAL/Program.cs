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
                MyConsole.BackgroundColor = ObtenerFondoOpcion(opcion);
                MyConsole.Clear();
                MyConsole.ReinicializarRegistros();

                MostrarTituloOpcion(opcion);

                if (opcion == 12)
                {
                    MostrarTop3(Datos.competencia);
                    MyConsole.WriteLine("¡Hasta la próxima! Presioná una tecla para cerrar.");
                    if (!Console.IsInputRedirected)
                        MyConsole.ReadKey(true);
                    break;
                }

                if (opcion != 1 && opcion != 11 && Datos.competencia == null)
                {
                    MyConsole.WriteLine("Primero tenés que crear o leer una competencia.");
                    MyConsole.WriteLine("\nPresioná cualquier tecla para volver al menú...");
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
                                Competencia competencia = Datos.competencia!;
                                competencia.RegistrarEquipo(equipoNuevo);
                                MyConsole.WriteLine("Equipo registrado.");
                            }
                            catch (RegistroException excepcion)
                            {
                                MyConsole.WriteLine(excepcion.Message);
                            }
                        }
                        break;

                    case 3:
                        Creador? creadorNuevo = CrearCreador();
                        if (creadorNuevo != null)
                        {
                            Equipo? equipo = Datos.competencia!.BuscarEquipo(creadorNuevo.nombreEquipo);
                            if (equipo != null)
                            {
                                try
                                {
                                    equipo.RegistrarCreador(creadorNuevo);
                                    MyConsole.WriteLine("Creador registrado.");
                                }
                                catch (RegistroException excepcion)
                                {
                                    MyConsole.WriteLine(excepcion.Message);
                                }
                            }
                        }
                        break;

                    case 4:
                        MostrarCompetencia(Datos.competencia!);
                        break;

                    case 5:
                        MostrarEquipo(Datos.competencia!);
                        break;

                    case 6:
                        MostrarCreador(Datos.competencia!);
                        break;                    

                    case 7:
                        MostrarEstadisticas(Datos.competencia!);
                        break; 

                    case 8:
                        MostrarComparacion(Datos.competencia!);
                        break;

                    case 9:
                        MostrarEquipoConMasSeguidores(Datos.competencia!);
                        break;

                    case 10:
                        try
                        {
                            Datos.Guardar("datos");
                            MyConsole.WriteLine("Datos guardados correctamente.");
                        }
                        catch (Exception excepcion)
                        {
                            MyConsole.WriteLine($"No se pudieron guardar los datos: {excepcion.Message}");
                        }
                        break;

                    case 11:
                        try
                        {
                            Competencia competenciaLeida = Datos.Leer("datos");
                            if (Datos.competencia != null &&
                                !ConfirmarReemplazo("¿Reemplazar la competencia actual y sus datos? (S/N):"))
                            {
                                MyConsole.WriteLine("Lectura cancelada.");
                                break;
                            }
                            Datos.competencia = competenciaLeida;
                            MyConsole.WriteLine($"Datos recuperados: {competenciaLeida.equipos.Count} equipos.");
                        }
                        catch (Exception excepcion)
                        {
                            MyConsole.WriteLine($"No se pudieron leer los datos: {excepcion.Message}");
                        }
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
                    MyConsole.WriteLine("Categoría:");
                    MyConsole.WriteLine("1. Gaming");
                    MyConsole.WriteLine("2. Humor");
                    MyConsole.WriteLine("3. Música");
                    MyConsole.WriteLine("4. Entretenimiento");
                    int opcionCategoria = PedirInt("Opción (1 a 4): ", 1, 4);
                    return new Equipo(nombre, fecha, (Categoria)(opcionCategoria - 1));
                }
                catch (ArgumentException excepcion)
                {
                    MyConsole.WriteLine(excepcion.Message);
                    MyConsole.WriteLine("Volvé a ingresar los datos del equipo.");
                }
            }
        }
        static Creador? CrearCreador()
        {
            string nombreEquipo = PedirTextoRequerido("Nombre del equipo: ");
            Equipo? equipo = Datos.competencia!.BuscarEquipo(nombreEquipo);
            if (equipo == null)
            {
                MyConsole.WriteLine("No se encontró el equipo.");
                return null;
            }

            try
            {
                string nombre = PedirTextoRequerido("Nombre del creador: ");
                string alias = PedirTextoRequerido("Alias: ");
                int edad = PedirInt("Edad (14 a 30): ", Creador.MIN_EDAD, Creador.MAX_EDAD);
                int seguidores = PedirInt("Seguidores (en miles) (0 a 2147483647): ", 0, int.MaxValue);
                decimal valoracion = PedirValoracion();
                MyConsole.WriteLine("Especialidad:");
                MyConsole.WriteLine("1. Streamer");
                MyConsole.WriteLine("2. Youtuber");
                MyConsole.WriteLine("3. Tiktoker");
                MyConsole.WriteLine("4. Influencer");
                int opcionEspecialidad = PedirInt("Opción (1 a 4): ", 1, 4);
                return new Creador(nombre, alias, equipo.nombre, edad, seguidores, valoracion,
                    (Especialidad)(opcionEspecialidad - 1));
            }
            catch (ArgumentException excepcion)
            {
                MyConsole.WriteLine(excepcion.Message);
                return null;
            }
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
            while (true)
            {
                string texto = PedirString("Valoración (1 a 10): ");
                if (decimal.TryParse(texto, System.Globalization.NumberStyles.Number,
                    System.Globalization.CultureInfo.CurrentCulture, out decimal valoracion) &&
                    valoracion >= 1 && valoracion <= 10)
                    return valoracion;

                if (decimal.TryParse(texto, System.Globalization.NumberStyles.Number,
                    System.Globalization.CultureInfo.InvariantCulture, out valoracion) &&
                    valoracion >= 1 && valoracion <= 10)
                    return valoracion;

                MyConsole.WriteLine("Ingresá un decimal válido (por ejemplo 8,5).");
            }
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
            MyConsole.WriteLine($"{competencia.nombre} | Sponsor: {competencia.patrocinador} | Año: {competencia.año} | Cupos: {competencia.equipos.Count}/{competencia.cantMaxEquipos}");
            bool primero = true;
            foreach (Equipo equipo in competencia.equipos)
            {
                if (!primero)
                    MyConsole.WriteLine("");
                MostrarEquipoDatos(equipo);
                primero = false;
            }
        }

        static void MostrarEquipo(Competencia competencia)
        {
            string nombre = PedirTextoRequerido("Nombre del equipo: ");
            Equipo? equipoEncontrado = competencia.BuscarEquipo(nombre);

            if (equipoEncontrado == null)
            {
                MyConsole.WriteLine("No se encontró el equipo.");
                return;
            }

            MostrarEquipoDatos(equipoEncontrado);
        }

        static void MostrarCreador(Competencia competencia)
        {
            string nombreEquipo = PedirTextoRequerido("Nombre del equipo: ");
            Equipo? equipo = competencia.BuscarEquipo(nombreEquipo);
            if (equipo == null)
            {
                MyConsole.WriteLine("No se encontró el creador.");
                return;
            }

            string alias = PedirTextoRequerido("Alias del creador: ");
            Creador? creadorEncontrado = equipo.BuscarCreador(alias);
            if (creadorEncontrado == null)
            {
                MyConsole.WriteLine("No se encontró el creador.");
                return;
            }

            MyConsole.WriteLine(FormatearCreador(creadorEncontrado));
        }

        static void MostrarEquipoDatos(Equipo equipo)
        {
            MyConsole.WriteLine($"Equipo: {equipo.nombre} | Creación: {equipo.fechaDeCreacion:dd/MM/yyyy} | Categoría: {FormatearCategoria(equipo.categoria)}");
            MyConsole.WriteLine($"Creadores ({equipo.creadores.Count}):");
            if (equipo.creadores.Count == 0)
                MyConsole.WriteLine("  (Sin creadores)");
            foreach (Creador creador in equipo.creadores)
                MyConsole.WriteLine($"  - {FormatearCreador(creador)}");
        }

        static string FormatearCreador(Creador creador)
        {
            string estrella = creador.EsEstrella() ? "Sí" : "No";
            return $"{creador.nombre} (@{creador.alias}) | Edad: {creador.edad} | Seguidores: {creador.seguidores} mil | Valoración: {creador.valoracion:0.##} | Especialidad: {creador.especialidad} | Estrella: {estrella}";
        }

        static string FormatearCategoria(Categoria categoria)
        {
            return categoria == Categoria.Musica ? "Música" : categoria.ToString();
        }

        static void MostrarEstadisticas(Competencia competencia)
        {
            string nombre = PedirTextoRequerido("Nombre del equipo: ");
            Equipo? equipoEncontrado = competencia.BuscarEquipo(nombre);

            if (equipoEncontrado == null)
            {
                MyConsole.WriteLine("No se encontró el equipo.");
                return;
            }

            Creador? popular = equipoEncontrado.ObtenerCreadorMasPopular();
            MyConsole.WriteLine($"Equipo: {equipoEncontrado.nombre}");
            MyConsole.WriteLine($"Valoración promedio: {equipoEncontrado.CalcularValoracionPromedio():N2}");
            MyConsole.WriteLine($"Cantidad de streamers: {equipoEncontrado.ContarStreamers()}");
            MyConsole.WriteLine($"Total seguidores: {equipoEncontrado.CalcularTotalSeguidores()} mil");
            MyConsole.WriteLine($"Creador más popular: {(popular == null ? "Sin creadores" : FormatearCreador(popular))}");
        }

        static void MostrarComparacion(Competencia competencia)
        {
            string primero = PedirTextoRequerido("Primer equipo: ");
            Equipo? equipoPrimero = competencia.BuscarEquipo(primero);
            if (equipoPrimero == null)
            {
                MyConsole.WriteLine("No se encontró el equipo.");
                return;
            }

            string segundo = PedirTextoRequerido("Segundo equipo: ");
            Equipo? equipoSegundo = competencia.BuscarEquipo(segundo);
            if (equipoSegundo == null)
            {
                MyConsole.WriteLine("No se encontró el equipo.");
                return;
            }

            ResultadoComparacion resultado = competencia.CompararEquipos(primero, segundo);
            if (resultado == ResultadoComparacion.MismoEquipo)
            {
                MyConsole.WriteLine("Elegí dos equipos diferentes.");
                return;
            }
            if (resultado == ResultadoComparacion.EquipoSinCreadores)
            {
                MyConsole.WriteLine("Ambos equipos deben tener creadores.");
                return;
            }

            decimal promedioPrimero = equipoPrimero.CalcularValoracionPromedio();
            decimal promedioSegundo = equipoSegundo.CalcularValoracionPromedio();
            MyConsole.WriteLine($"{equipoPrimero.nombre}: {promedioPrimero:N2} | {equipoSegundo.nombre}: {promedioSegundo:N2}");
            MyConsole.WriteLine(resultado == ResultadoComparacion.Empate
                ? "¡Empate!"
                : $"Ganador: {(resultado == ResultadoComparacion.GanaPrimero ? equipoPrimero.nombre : equipoSegundo.nombre)}");
        }

        static void MostrarTop3(Competencia? competencia)
        {
            MyConsole.WriteLine("=== TOP 3 DE STREAMORT ===");
            if (competencia == null)
            {
                MyConsole.WriteLine("No hay equipos con creadores para armar el ranking.");
                return;
            }

            List<Equipo> mejores = competencia.ObtenerTop3();
            if (mejores.Count == 0)
            {
                MyConsole.WriteLine("No hay equipos con creadores para armar el ranking.");
                return;
            }

            for (int i = 0; i < mejores.Count; i++)
                MyConsole.WriteLine($"{i + 1}. {mejores[i].nombre} - Promedio: {mejores[i].CalcularValoracionPromedio():N2}");
        }

        static ConsoleColor ObtenerFondoOpcion(int opcion)
        {
            if (opcion == 2 || opcion == 5 || opcion == 7 || opcion == 8 || opcion == 9)
                return ConsoleColor.DarkBlue;
            if (opcion == 3 || opcion == 6)
                return ConsoleColor.DarkMagenta;
            if (opcion == 12)
                return ConsoleColor.DarkGreen;
            return ConsoleColor.Black;
        }

        static void MostrarTituloOpcion(int opcion)
        {
            string? titulo = opcion switch
            {
                1 => "STREAMORT | CREAR COMPETENCIA",
                2 => "STREAMORT | REGISTRAR EQUIPO",
                3 => "STREAMORT | REGISTRAR CREADOR EN UN EQUIPO",
                4 => "STREAMORT | MOSTRAR INFORMACIÓN DE LA COMPETENCIA",
                5 => "STREAMORT | MOSTRAR INFORMACIÓN DE UN EQUIPO",
                6 => "STREAMORT | MOSTRAR INFORMACIÓN DE UN CREADOR",
                7 => "STREAMORT | MOSTRAR ESTADÍSTICAS DE UN EQUIPO",
                8 => "STREAMORT | COMPARAR VALORACIÓN DE DOS EQUIPOS",
                9 => "STREAMORT | MOSTRAR EL EQUIPO CON MÁS SEGUIDORES",
                12 => null,
                _ => null
            };

            if (titulo != null)
            {
                MyConsole.WriteLine("============================================");
                MyConsole.WriteLine($"  {titulo}");
                MyConsole.WriteLine("============================================");
                MyConsole.WriteLine("");
            }
        }

        static void MostrarEquipoConMasSeguidores(Competencia competencia)
        {
            Equipo? equipo = competencia.ObtenerEquipoConMasSeguidores();
            if (equipo == null)
            {
                MyConsole.WriteLine("No hay equipos con creadores.");
                return;
            }

            MyConsole.WriteLine($"{equipo.nombre}: {equipo.CalcularTotalSeguidores()} mil seguidores.");
        }
    }
}
