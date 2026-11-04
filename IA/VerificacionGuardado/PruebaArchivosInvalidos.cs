using ACTIVIDAD_ESPECIAL;
using DataBases;

internal static class PruebaArchivosInvalidos
{
    public static void Ejecutar()
    {
        string raiz = Path.Combine(Path.GetTempPath(), "StreamORT-Errores-" + Guid.NewGuid().ToString("N"));
        string original = Path.Combine(raiz, "original");
        Competencia? anterior = Datos.competencia;
        Competencia competencia = new Competencia("Prueba", "Sponsor", 2026, 2);
        Equipo equipo = new Equipo("Equipo", new DateOnly(2020, 1, 1), Categoria.Gaming);
        equipo.RegistrarCreador(new Creador("Ana", "alias", "Equipo", 20, 500, 8.004m, Especialidad.Streamer));
        competencia.RegistrarEquipo(equipo);
        Datos.competencia = competencia;
        string[] archivos = { "competencia.csv", "equipos.csv", "creadores.csv" };
        int casos = 0;
        try
        {
            Datos.Guardar(original);
            foreach (string archivo in archivos)
            {
                foreach (string variante in new[] { "ausente", "vacio", "espacios", "cabecera", "fila-rota", "duplicado" })
                {
                    string copia = Path.Combine(raiz, (++casos).ToString());
                    Directory.CreateDirectory(Path.Combine(copia, "actual"));
                    foreach (string nombre in archivos)
                        File.Copy(Path.Combine(original, "actual", nombre), Path.Combine(copia, "actual", nombre));
                    string ruta = Path.Combine(copia, "actual", archivo);
                    string[] lineas = File.ReadAllLines(ruta);
                    switch (variante)
                    {
                        case "ausente": File.Move(ruta, ruta + ".apartado"); break;
                        case "vacio": File.WriteAllText(ruta, ""); break;
                        case "espacios": File.WriteAllText(ruta, " \n\t"); break;
                        case "cabecera": File.WriteAllText(ruta, "columna-inexistente\n"); break;
                        case "fila-rota": File.AppendAllText(ruta, "\n\"sin cerrar"); break;
                        case "duplicado": File.AppendAllText(ruta, "\n" + lineas[1]); break;
                    }
                    ExigirRechazo(copia, competencia, archivo + "/" + variante);
                }
            }
            foreach (string archivo in new[] { "equipos.csv", "creadores.csv" })
            {
                string copia = Path.Combine(raiz, (++casos).ToString());
                Directory.CreateDirectory(Path.Combine(copia, "actual"));
                foreach (string nombre in archivos)
                    File.Copy(Path.Combine(original, "actual", nombre), Path.Combine(copia, "actual", nombre));
                string ruta = Path.Combine(copia, "actual", archivo);
                string texto = File.ReadAllText(ruta);
                texto = archivo == "equipos.csv" ? texto.Replace("\"Prueba\"", "\"Otra\"")
                    : texto.Replace("\"Equipo\"", "\"Inexistente\"");
                File.WriteAllText(ruta, texto);
                ExigirRechazo(copia, competencia, archivo + "/relacion");
            }
            ExigirRechazo(Path.Combine(raiz, "inexistente"), competencia, "directorio ausente");
            casos++;

            string[] anteriores = archivos.Select(a => File.ReadAllText(Path.Combine(original, "actual", a))).ToArray();
            // En Windows, un archivo abierto sin compartir impide mover el conjunto actual.
            using (FileStream bloqueo = new FileStream(Path.Combine(original, "actual", "competencia.csv"),
                FileMode.Open, FileAccess.Read, FileShare.None))
            {
                bool rechazado = false;
                try { Datos.Guardar(original); }
                catch (IOException) { rechazado = true; }
                if (!rechazado)
                    throw new Exception("No se produjo el error de escritura esperado con el archivo bloqueado.");
            }
            for (int i = 0; i < archivos.Length; i++)
                if (File.ReadAllText(Path.Combine(original, "actual", archivos[i])) != anteriores[i])
                    throw new Exception("El fallo de escritura alteró la copia anterior.");
            if (!ReferenceEquals(Datos.competencia, competencia) || competencia.equipos.Count != 1 ||
                equipo.creadores.Count != 1 || equipo.creadores[0].valoracion != 8.004m)
                throw new Exception("El fallo de escritura alteró la memoria.");
            casos++;
            Console.WriteLine($"Correcto: {casos} casos de archivos inválidos y escritura bloqueada; memoria y copia anterior conservadas.");
            Console.WriteLine("Copias de errores: " + raiz);
        }
        finally
        {
            Datos.competencia = anterior;
        }
    }

    private static void ExigirRechazo(string directorio, Competencia activa, string caso)
    {
        bool rechazado = false;
        try { Datos.Leer(directorio); }
        catch (Exception excepcion) when (excepcion is IOException || excepcion is CsvFormatException ||
            excepcion is RegistroException || excepcion is ArgumentException)
        {
            rechazado = true;
        }
        if (!rechazado)
            throw new Exception("Se aceptó el caso inválido: " + caso);
        if (!ReferenceEquals(Datos.competencia, activa) || activa.equipos.Count != 1 ||
            activa.equipos[0].creadores.Count != 1 || activa.equipos[0].creadores[0].valoracion != 8.004m)
            throw new Exception("La lectura fallida alteró la memoria: " + caso);
    }
}
