using System.Diagnostics;
using System.Text.Json;
using ACTIVIDAD_ESPECIAL;

internal static class PruebaIdaVuelta
{
    public static void Ejecutar()
    {
        string raiz = Path.Combine(Path.GetTempPath(), "StreamORT-IdaVuelta-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(raiz);
        foreach (string caso in new[] { "completa", "sin-equipos", "equipos-vacios" })
        {
            string directorio = Path.Combine(raiz, caso);
            EjecutarProceso("guardar-prueba", directorio, caso);
            EjecutarProceso("leer-prueba", directorio, caso);
        }
        Console.WriteLine("Correcto: ida y vuelta en procesos separados, tres escenarios, todos los campos y estadísticas.");
        Console.WriteLine("Copias de prueba: " + raiz);
    }

    public static void EjecutarProceso(string accion, string directorio, string caso)
    {
        ProcessStartInfo inicio = new ProcessStartInfo("dotnet");
        inicio.ArgumentList.Add("--roll-forward");
        inicio.ArgumentList.Add("Major");
        inicio.ArgumentList.Add(typeof(PruebaIdaVuelta).Assembly.Location);
        inicio.ArgumentList.Add(accion);
        inicio.ArgumentList.Add(directorio);
        inicio.ArgumentList.Add(caso);
        inicio.UseShellExecute = false;
        inicio.CreateNoWindow = true;
        using Process proceso = Process.Start(inicio) ?? throw new Exception("No se pudo iniciar la prueba.");
        if (!proceso.WaitForExit(30000))
        {
            proceso.Kill();
            throw new Exception("Se agotó el tiempo de la prueba.");
        }
        if (proceso.ExitCode != 0)
            throw new Exception("Falló el proceso de " + accion + " del caso " + caso);
    }

    public static void Guardar(string directorio, string caso)
    {
        if (Datos.competencia != null)
            throw new Exception("El proceso debe empezar sin competencia.");
        Competencia competencia = new Competencia("Batalla ñ, \"2026\"", "Música; sponsor\\especial", 2026, 10000);
        if (caso != "sin-equipos")
        {
            Equipo primero = new Equipo("Águilas, \"Sur\"", new DateOnly(2020, 2, 29), Categoria.Musica);
            Equipo segundo = new Equipo("Humor", new DateOnly(2000, 1, 1), Categoria.Humor);
            Equipo tercero = new Equipo("Gaming", new DateOnly(2021, 12, 31), Categoria.Gaming);
            Equipo vacio = new Equipo("Vacío", new DateOnly(2022, 6, 15), Categoria.Entretenimiento);
            competencia.RegistrarEquipo(primero);
            competencia.RegistrarEquipo(segundo);
            competencia.RegistrarEquipo(tercero);
            competencia.RegistrarEquipo(vacio);
            if (caso == "completa")
            {
                primero.RegistrarCreador(new Creador("Ana, \"Ñ\"", "compartido", primero.nombre, 14, int.MaxValue, 8.004m, Especialidad.Streamer));
                primero.RegistrarCreador(new Creador("Luis", "otro", primero.nombre, 30, int.MaxValue, 8.001m, Especialidad.Youtuber));
                segundo.RegistrarCreador(new Creador("María", "compartido", segundo.nombre, 20, 500, 7.999m, Especialidad.Tiktoker));
                tercero.RegistrarCreador(new Creador("Sol", "sol", tercero.nombre, 21, 0, 8.5m, Especialidad.Influencer));
            }
        }
        Datos.competencia = competencia;
        Datos.Guardar(directorio);
        File.WriteAllText(Path.Combine(directorio, "esperado.json"), Describir(competencia));
    }

    public static void Leer(string directorio)
    {
        if (Datos.competencia != null)
            throw new Exception("La lectura debe iniciar sin estado de la ejecución anterior.");
        Competencia recuperada = Datos.Leer(directorio);
        string esperado = File.ReadAllText(Path.Combine(directorio, "esperado.json"));
        if (Describir(recuperada) != esperado)
            throw new Exception("No coinciden campos, orden, asociaciones o estadísticas.");
        if (Datos.competencia != null)
            throw new Exception("Leer modificó el estado antes de la confirmación.");
        Datos.competencia = recuperada;
        Console.WriteLine("Verificado tras reinicio: " + Path.GetFileName(directorio));
    }

    private static string Describir(Competencia competencia)
    {
        return JsonSerializer.Serialize(new
        {
            competencia.nombre, competencia.patrocinador, competencia.año, competencia.cantMaxEquipos,
            equipos = competencia.equipos.Select(e => new
            {
                e.nombre, e.nombreCompetencia, e.fechaDeCreacion, e.categoria,
                creadores = e.creadores.Select(c => new
                {
                    c.nombre, c.alias, c.nombreEquipo, c.edad, c.seguidores, c.valoracion, c.especialidad,
                    estrella = c.EsEstrella()
                }),
                promedio = e.CalcularValoracionPromedio(), streamers = e.ContarStreamers(),
                seguidores = e.CalcularTotalSeguidores(), popular = e.ObtenerCreadorMasPopular()?.alias
            }),
            comparaciones = competencia.equipos.SelectMany(a => competencia.equipos.Select(b =>
                competencia.CompararEquipos(a.nombre, b.nombre))).ToArray(),
            masSeguidores = competencia.ObtenerEquipoConMasSeguidores()?.nombre,
            top3 = competencia.ObtenerTop3().Select(e => e.nombre).ToArray()
        });
    }
}
