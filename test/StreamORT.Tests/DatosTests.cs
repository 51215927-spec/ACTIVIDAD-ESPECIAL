using System.Globalization;
using System.Text.Json;
using ACTIVIDAD_ESPECIAL;
using DataBases;

namespace StreamORT.Tests
{
    // xUnit ejecuta los casos de esta clase de forma secuencial: comparten el estado estático de Datos.
    public class DatosTests : IDisposable
    {
        private readonly string raiz = Path.Combine(Path.GetTempPath(), "StreamORT-xUnit-" + Guid.NewGuid().ToString("N"));
        private readonly Competencia? anterior = Datos.competencia;
        private readonly string directorioModeloAnterior = Datos.directorioModelo;
        private readonly CultureInfo culturaAnterior = CultureInfo.CurrentCulture;
        private readonly string[] archivos = { "competencia.csv", "equipos.csv", "creadores.csv" };

        [Theory]
        [InlineData("es-AR", "en-US")]
        [InlineData("en-US", "es-AR")]
        public void IdaVueltaConservaCamposRelacionesOrdenYCalculosEntreCulturas(string escritura, string lectura)
        {
            Competencia original = CrearCompetenciaCompleta();
            string esperado = Describir(original);
            Datos.competencia = original;
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(escritura);
            Datos.Guardar(raiz);
            Datos.competencia = null;
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(lectura);

            Competencia recuperada = Datos.Leer(raiz);

            Assert.Equal(esperado, Describir(recuperada));
            Assert.NotSame(original, recuperada);
            Assert.NotSame(original.equipos[0], recuperada.equipos[0]);
            Assert.NotSame(original.equipos[0].creadores[0], recuperada.equipos[0].creadores[0]);
            Assert.Null(Datos.competencia);
            Assert.Equal(4294967294L, recuperada.equipos[0].CalcularTotalSeguidores());
            Assert.Equal(8.0025m, recuperada.equipos[0].CalcularValoracionPromedio());
            Assert.True(recuperada.equipos[0].creadores[0].EsEstrella());
            Assert.False(recuperada.equipos[1].creadores[0].EsEstrella());
            Assert.Equal(directorioModeloAnterior, Datos.directorioModelo);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void IdaVueltaAceptaCompetenciaSinEquiposOConEquipoVacio(bool agregarEquipo)
        {
            Competencia original = new Competencia("Torneo", "Marca", 2026, 5);
            if (agregarEquipo)
                original.RegistrarEquipo(new Equipo("Vacio", new DateOnly(2024, 1, 1), Categoria.Gaming));
            Datos.competencia = original;
            Datos.Guardar(raiz);

            Competencia recuperada = Datos.Leer(raiz);

            Assert.Equal(Describir(original), Describir(recuperada));
            Assert.Same(original, Datos.competencia);
            foreach (string archivo in archivos)
                Assert.True(File.Exists(Path.Combine(raiz, "actual", archivo)));
        }

        [Fact]
        public void GuardarSinCompetenciaNoCreaDirectorio()
        {
            Datos.competencia = null;

            Assert.Throws<InvalidOperationException>(() => Datos.Guardar(raiz));
            Assert.False(Directory.Exists(raiz));
        }

        [Fact]
        public void ReemplazoPorCompetenciaVaciaNoDejaFilasYConservaRespaldoCompleto()
        {
            Competencia original = CrearCompetenciaCompleta();
            Datos.competencia = original;
            Datos.Guardar(raiz);
            byte[][] anteriores = LeerArchivosActuales();
            Competencia nueva = new Competencia("Nueva", "Otro patrocinador", 2027, 1);
            Datos.competencia = nueva;
            Datos.Guardar(raiz);

            Competencia recuperada = Datos.Leer(raiz);

            Assert.Equal(Describir(nueva), Describir(recuperada));
            Assert.Empty(recuperada.equipos);
            string respaldo = Assert.Single(Directory.GetDirectories(raiz, "respaldo-*"));
            for (int i = 0; i < archivos.Length; i++)
                Assert.Equal(anteriores[i], File.ReadAllBytes(Path.Combine(respaldo, archivos[i])));
        }

        [Fact]
        public void LecturaRecuperaRespaldoSiFaltaActual()
        {
            Competencia original = CrearCompetenciaCompleta();
            Datos.competencia = original;
            Datos.Guardar(raiz);
            Datos.competencia = new Competencia("Nueva", "Marca", 2026, 1);
            Datos.Guardar(raiz);
            Directory.Move(Path.Combine(raiz, "actual"), Path.Combine(raiz, "apartado"));

            Assert.Equal(Describir(original), Describir(Datos.Leer(raiz)));
            Assert.Equal("Nueva", Datos.competencia.nombre);
            Assert.False(Directory.Exists(Path.Combine(raiz, "actual")));
        }

        [Fact]
        public void RecuperacionOmiteRespaldoInvalidoMasReciente()
        {
            Competencia original = CrearCompetenciaCompleta();
            Datos.competencia = original;
            Datos.Guardar(raiz);
            string respaldo = Path.Combine(raiz, "respaldo-valido");
            Directory.Move(Path.Combine(raiz, "actual"), respaldo);
            string invalido = Path.Combine(raiz, "respaldo-invalido");
            Directory.CreateDirectory(invalido);
            Directory.SetLastWriteTimeUtc(respaldo, new DateTime(2020, 1, 1));
            Directory.SetLastWriteTimeUtc(invalido, new DateTime(2021, 1, 1));

            Assert.Equal(Describir(original), Describir(Datos.Leer(raiz)));
            Assert.Same(original, Datos.competencia);
        }

        [Fact]
        public void ActualCorruptoSeRechazaAunqueExistaRespaldoValido()
        {
            Datos.competencia = CrearCompetenciaCompleta();
            Datos.Guardar(raiz);
            Datos.Guardar(raiz);
            File.WriteAllText(Path.Combine(raiz, "actual", "creadores.csv"), "columna-inexistente\n");
            string memoria = Describir(Datos.competencia);

            Assert.Throws<CsvFormatException>(() => Datos.Leer(raiz));
            Assert.Equal(memoria, Describir(Datos.competencia));
        }

        [Theory]
        [InlineData("competencia.csv", "ausente")]
        [InlineData("competencia.csv", "vacio")]
        [InlineData("competencia.csv", "espacios")]
        [InlineData("competencia.csv", "cabecera")]
        [InlineData("competencia.csv", "fila-rota")]
        [InlineData("competencia.csv", "duplicado")]
        [InlineData("equipos.csv", "ausente")]
        [InlineData("equipos.csv", "vacio")]
        [InlineData("equipos.csv", "espacios")]
        [InlineData("equipos.csv", "cabecera")]
        [InlineData("equipos.csv", "fila-rota")]
        [InlineData("equipos.csv", "duplicado")]
        [InlineData("creadores.csv", "ausente")]
        [InlineData("creadores.csv", "vacio")]
        [InlineData("creadores.csv", "espacios")]
        [InlineData("creadores.csv", "cabecera")]
        [InlineData("creadores.csv", "fila-rota")]
        [InlineData("creadores.csv", "duplicado")]
        public void LecturaRechazaArchivosInvalidosSinCambiarMemoria(string archivo, string variante)
        {
            Competencia original = CrearCompetenciaCompleta();
            Datos.competencia = original;
            string memoria = Describir(original);
            Datos.Guardar(raiz);
            string ruta = Path.Combine(raiz, "actual", archivo);
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

            Exception? error = Record.Exception(() => Datos.Leer(raiz));

            Assert.NotNull(error);
            Assert.True(error is IOException || error is CsvFormatException || error is RegistroException,
                $"Excepción inesperada: {error}");
            Assert.Same(original, Datos.competencia);
            Assert.Equal(memoria, Describir(original));
            Assert.Equal(directorioModeloAnterior, Datos.directorioModelo);
        }

        [Theory]
        [InlineData("equipos.csv")]
        [InlineData("creadores.csv")]
        public void LecturaRechazaRelacionesInexistentes(string archivo)
        {
            Competencia original = new Competencia("Torneo", "Marca", 2026, 2);
            Equipo equipo = new Equipo("Equipo", new DateOnly(2024, 1, 1), Categoria.Gaming);
            equipo.RegistrarCreador(new Creador("Ana", "alias", "Equipo", 20, 500, 8.004m, Especialidad.Streamer));
            original.RegistrarEquipo(equipo);
            Datos.competencia = original;
            Datos.Guardar(raiz);
            string ruta = Path.Combine(raiz, "actual", archivo);
            string texto = File.ReadAllText(ruta);
            string alterado = archivo == "equipos.csv"
                ? texto.Replace("\"Torneo\"", "\"Otra\"")
                : texto.Replace("\"Equipo\"", "\"Inexistente\"");
            Assert.NotEqual(texto, alterado);
            File.WriteAllText(ruta, alterado);

            Assert.Throws<IOException>(() => Datos.Leer(raiz));
            Assert.Same(original, Datos.competencia);
            Assert.Equal(8.004m, original.equipos[0].creadores[0].valoracion);
        }

        [Fact]
        public void LecturaRechazaDirectorioAusenteSinCrearlo()
        {
            Datos.competencia = CrearCompetenciaCompleta();
            Competencia original = Datos.competencia;

            Assert.Throws<DirectoryNotFoundException>(() => Datos.Leer(raiz));
            Assert.False(Directory.Exists(raiz));
            Assert.Same(original, Datos.competencia);
        }

        [Fact]
        public void LecturaRechazaDirectorioSinConjuntoValido()
        {
            Directory.CreateDirectory(Path.Combine(raiz, "respaldo-incompleto"));
            Datos.competencia = null;

            Assert.Throws<IOException>(() => Datos.Leer(raiz));
            Assert.Null(Datos.competencia);
        }

        [Fact]
        public void GuardadoRechazaDestinoOcupadoPorArchivoSinAlterarlo()
        {
            Directory.CreateDirectory(raiz);
            string actual = Path.Combine(raiz, "actual");
            File.WriteAllText(actual, "obstaculo");
            Competencia original = CrearCompetenciaCompleta();
            Datos.competencia = original;

            Assert.Throws<IOException>(() => Datos.Guardar(raiz));
            Assert.Equal("obstaculo", File.ReadAllText(actual));
            Assert.Same(original, Datos.competencia);
            Assert.Equal(directorioModeloAnterior, Datos.directorioModelo);
        }

        [Fact]
        public void GuardadoBloqueadoConservaUltimaCopiaYMemoriaEnWindows()
        {
            // El bloqueo de un archivo impide mover su carpeta en Windows.
            if (!OperatingSystem.IsWindows())
                return;

            Datos.competencia = CrearCompetenciaCompleta();
            Datos.Guardar(raiz);
            byte[][] anteriores = LeerArchivosActuales();
            Competencia nueva = new Competencia("Nueva", "Marca", 2027, 1);
            Datos.competencia = nueva;
            using (FileStream bloqueo = new FileStream(Path.Combine(raiz, "actual", "competencia.csv"),
                FileMode.Open, FileAccess.Read, FileShare.None))
            {
                Assert.Throws<IOException>(() => Datos.Guardar(raiz));
            }

            for (int i = 0; i < archivos.Length; i++)
                Assert.Equal(anteriores[i], File.ReadAllBytes(Path.Combine(raiz, "actual", archivos[i])));
            Assert.Same(nueva, Datos.competencia);
            Assert.Equal(directorioModeloAnterior, Datos.directorioModelo);
        }

        private byte[][] LeerArchivosActuales()
        {
            return archivos.Select(a => File.ReadAllBytes(Path.Combine(raiz, "actual", a))).ToArray();
        }

        private static Competencia CrearCompetenciaCompleta()
        {
            Competencia competencia = new Competencia("Batalla ñ, \"2026\"", "Música; sponsor\\especial", 2026, 10000);
            Equipo primero = new Equipo("Águilas, \"Sur\"", new DateOnly(2020, 2, 29), Categoria.Musica);
            Equipo segundo = new Equipo("Humor", new DateOnly(2000, 1, 1), Categoria.Humor);
            Equipo tercero = new Equipo("Gaming", new DateOnly(2021, 12, 31), Categoria.Gaming);
            Equipo vacio = new Equipo("Vacío", new DateOnly(2022, 6, 15), Categoria.Entretenimiento);
            primero.RegistrarCreador(new Creador("Ana, \"Ñ\"", "compartido", primero.nombre, 14, int.MaxValue, 8.004m, Especialidad.Streamer));
            primero.RegistrarCreador(new Creador("Luis", "otro", primero.nombre, 30, int.MaxValue, 8.001m, Especialidad.Youtuber));
            segundo.RegistrarCreador(new Creador("María", "compartido", segundo.nombre, 20, 500, 7.999m, Especialidad.Tiktoker));
            tercero.RegistrarCreador(new Creador("Sol", "sol", tercero.nombre, 21, 0, 8.5m, Especialidad.Influencer));
            competencia.RegistrarEquipo(primero);
            competencia.RegistrarEquipo(segundo);
            competencia.RegistrarEquipo(tercero);
            competencia.RegistrarEquipo(vacio);
            return competencia;
        }

        private static string Describir(Competencia competencia)
        {
            // Incluye campos y resultados observables; no serializa detalles privados de implementación.
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

        public void Dispose()
        {
            Datos.competencia = anterior;
            CultureInfo.CurrentCulture = culturaAnterior;
            // La única raíz que se elimina es la carpeta temporal única creada por este caso.
            if (Directory.Exists(raiz))
            {
                string temporal = Path.TrimEndingDirectorySeparator(Path.GetFullPath(Path.GetTempPath()))
                    + Path.DirectorySeparatorChar;
                string destino = Path.GetFullPath(raiz);
                Assert.StartsWith(temporal, destino, StringComparison.OrdinalIgnoreCase);
                Assert.StartsWith("StreamORT-xUnit-", Path.GetFileName(destino));
                Directory.Delete(destino, true);
            }
        }
    }
}
