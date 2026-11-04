using System.Globalization;
using ACTIVIDAD_ESPECIAL;

internal static class PruebaDecimales
{
    public static void Ejecutar()
    {
        CultureInfo culturaAnterior = CultureInfo.CurrentCulture;
        Competencia? estadoAnterior = Datos.competencia;
        string raiz = Path.Combine(Path.GetTempPath(), "StreamORT-Decimales-" + Guid.NewGuid().ToString("N"));
        decimal[] valores = { 8.5m, 8.001m, 8.004m, 7.999m };
        string[] nombres = { "Mayor", "Alfa", "Zeta", "Menor" };
        try
        {
            foreach (string cultura in new[] { "es-AR", "en-US" })
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(cultura);
                Competencia original = new Competencia("Precisión", "Prueba", 2026, 4);
                for (int i = 0; i < valores.Length; i++)
                {
                    Equipo equipo = new Equipo(nombres[i], new DateOnly(2020, 1, 1), Categoria.Gaming);
                    foreach (Especialidad especialidad in Enum.GetValues<Especialidad>())
                    {
                        foreach (int seguidores in new[] { 499, 500 })
                        {
                            string alias = especialidad + "-" + seguidores;
                            equipo.RegistrarCreador(new Creador(alias, alias, equipo.nombre, 20,
                                seguidores, valores[i], especialidad));
                        }
                    }
                    original.RegistrarEquipo(equipo);
                }
                Datos.competencia = original;
                string directorio = Path.Combine(raiz, cultura);
                Datos.Guardar(directorio);
                // Leer con otra cultura demuestra que el CSV no depende del separador local.
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(cultura == "es-AR" ? "en-US" : "es-AR");
                Competencia recuperada = Datos.Leer(directorio);
                foreach (Competencia competencia in new[] { original, recuperada })
                {
                    for (int i = 0; i < valores.Length; i++)
                    {
                        Equipo equipo = competencia.BuscarEquipo(nombres[i])!;
                        if (equipo.CalcularValoracionPromedio() != valores[i])
                            throw new Exception("Se redondeó el promedio de " + equipo.nombre);
                        foreach (Creador creador in equipo.creadores)
                        {
                            bool estrellaEsperada = i != 3 && creador.seguidores == 500;
                            if (creador.valoracion != valores[i] || creador.EsEstrella() != estrellaEsperada)
                                throw new Exception("Se perdió precisión o cambió Estrella: " + creador.alias);
                        }
                    }
                    if (competencia.CompararEquipos("Zeta", "Alfa") != ResultadoComparacion.GanaPrimero ||
                        competencia.CompararEquipos("Alfa", "Zeta") != ResultadoComparacion.GanaSegundo)
                        throw new Exception("8.004 debe superar a 8.001 aunque ambos se muestren como 8.00.");
                    if (!competencia.ObtenerTop3().Select(e => e.nombre).SequenceEqual(new[] { "Mayor", "Zeta", "Alfa" }))
                        throw new Exception("El TOP 3 debe usar los promedios completos.");
                }
                string fichaAlfa = 8.001m.ToString("F2", CultureInfo.InvariantCulture);
                string fichaZeta = 8.004m.ToString("F2", CultureInfo.InvariantCulture);
                if (fichaAlfa != "8.00" || fichaAlfa != fichaZeta)
                    throw new Exception("La muestra no representa dos valores visualmente iguales.");
            }
            Console.WriteLine("Correcto: decimales exactos, promedios, Estrella, comparación y TOP 3; es-AR/en-US.");
            Console.WriteLine("Copias decimales: " + raiz);
        }
        finally
        {
            CultureInfo.CurrentCulture = culturaAnterior;
            Datos.competencia = estadoAnterior;
        }
    }
}
