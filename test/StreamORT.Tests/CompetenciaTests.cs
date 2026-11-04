using ACTIVIDAD_ESPECIAL;

namespace StreamORT.Tests
{
    public class CompetenciaTests
    {
        [Theory]
        [InlineData(2000, 1)]
        [InlineData(2027, 10000)]
        public void ConstructorAceptaLimitesYConservaDatos(int año, int cupo)
        {
            Competencia competencia = new Competencia(" Torneo Uno ", " Marca Uno ", año, cupo);

            Assert.Equal("Torneo Uno", competencia.nombre);
            Assert.Equal("Marca Uno", competencia.patrocinador);
            Assert.Equal(año, competencia.año);
            Assert.Equal(cupo, competencia.cantMaxEquipos);
            Assert.Empty(competencia.equipos);
            Assert.True(competencia.HayCupo());
        }

        [Theory]
        [InlineData(null, "Marca")]
        [InlineData("", "Marca")]
        [InlineData("  ", "Marca")]
        [InlineData("Torneo", null)]
        [InlineData("Torneo", "")]
        [InlineData("Torneo", "  ")]
        public void ConstructorRechazaTextosVacios(string? nombre, string? patrocinador)
        {
            Assert.Throws<ArgumentException>(() => new Competencia(nombre!, patrocinador!, 2026, 5));
        }

        [Theory]
        [InlineData(1999, 1)]
        [InlineData(2028, 1)]
        [InlineData(2026, 0)]
        [InlineData(2026, 10001)]
        public void ConstructorRechazaNumerosFueraDeRango(int año, int cupo)
        {
            Assert.Throws<ArgumentException>(() => new Competencia("Torneo", "Marca", año, cupo));
        }

        [Fact]
        public void CompetenciaVaciaNoTieneCandidatos()
        {
            Competencia competencia = new Competencia("Torneo", "Marca", 2026, 5);

            Assert.Null(competencia.ObtenerEquipoConMasSeguidores());
            Assert.Empty(competencia.ObtenerTop3());
            Assert.Null(competencia.BuscarEquipo("ausente"));
        }

        [Theory]
        [InlineData("Equipo Uno")]
        [InlineData("EQUIPO UNO")]
        [InlineData("  equipo uno  ")]
        public void RegistroAsignaCompetenciaYBusquedaIgnoraCajaYEspacios(string consulta)
        {
            Competencia competencia = new Competencia("Torneo", "Marca", 2026, 5);
            Equipo equipo = new Equipo("Equipo Uno", new DateOnly(2024, 1, 1), Categoria.Gaming);
            competencia.RegistrarEquipo(equipo);

            Assert.Equal("Torneo", equipo.nombreCompetencia);
            Assert.Same(equipo, competencia.BuscarEquipo(consulta));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("ausente")]
        public void BuscarEquipoAusenteDevuelveNull(string? consulta)
        {
            Competencia competencia = new Competencia("Torneo", "Marca", 2026, 5);
            competencia.RegistrarEquipo(new Equipo("Equipo", new DateOnly(2024, 1, 1), Categoria.Gaming));

            Assert.Null(competencia.BuscarEquipo(consulta!));
        }

        [Fact]
        public void RechazaEquipoNuloYDuplicadoSinModificarRegistros()
        {
            Competencia competencia = new Competencia("Torneo", "Marca", 2026, 5);
            Equipo primero = new Equipo("Equipo", new DateOnly(2024, 1, 1), Categoria.Gaming);
            competencia.RegistrarEquipo(primero);
            Equipo duplicado = new Equipo(" EQUIPO ", new DateOnly(2024, 1, 1), Categoria.Humor);

            Assert.Throws<RegistroException>(() => competencia.RegistrarEquipo(null!));
            Assert.Throws<RegistroException>(() => competencia.RegistrarEquipo(duplicado));
            Assert.Same(primero, Assert.Single(competencia.equipos));
            Assert.Equal("", duplicado.nombreCompetencia);
        }

        [Fact]
        public void CupoSeAgotaYRechazaOtroEquipoSinAsignarlo()
        {
            Competencia competencia = new Competencia("Torneo", "Marca", 2026, 1);
            competencia.RegistrarEquipo(new Equipo("Uno", new DateOnly(2024, 1, 1), Categoria.Gaming));
            Equipo otro = new Equipo("Dos", new DateOnly(2024, 1, 1), Categoria.Humor);

            Assert.False(competencia.HayCupo());
            Assert.Throws<RegistroException>(() => competencia.RegistrarEquipo(otro));
            Assert.Single(competencia.equipos);
            Assert.Null(competencia.BuscarEquipo("Dos"));
            Assert.Equal("", otro.nombreCompetencia);
        }

        [Theory]
        [InlineData("ausente", "Uno", 0)]
        [InlineData("Uno", "ausente", 1)]
        [InlineData("Uno", " UNO ", 2)]
        [InlineData("Uno", "Dos", 3)]
        [InlineData("Dos", "Uno", 3)]
        public void ComparacionInformaInexistentesMismoEquipoYVacios(string primero, string segundo, int esperado)
        {
            Competencia competencia = new Competencia("Torneo", "Marca", 2026, 5);
            Equipo uno = new Equipo("Uno", new DateOnly(2024, 1, 1), Categoria.Gaming);
            uno.RegistrarCreador(new Creador("Ana", "a", "Uno", 20, 0, 8, Especialidad.Streamer));
            competencia.RegistrarEquipo(uno);
            competencia.RegistrarEquipo(new Equipo("Dos", new DateOnly(2024, 1, 1), Categoria.Gaming));

            Assert.Equal((ResultadoComparacion)esperado, competencia.CompararEquipos(primero, segundo));
        }

        [Fact]
        public void ComparacionEntreDosEquiposVaciosSeRechaza()
        {
            Competencia competencia = new Competencia("Torneo", "Marca", 2026, 5);
            competencia.RegistrarEquipo(new Equipo("Uno", new DateOnly(2024, 1, 1), Categoria.Gaming));
            competencia.RegistrarEquipo(new Equipo("Dos", new DateOnly(2024, 1, 1), Categoria.Gaming));

            Assert.Equal(ResultadoComparacion.EquipoSinCreadores, competencia.CompararEquipos("Uno", "Dos"));
        }

        [Fact]
        public void ComparacionUsaPromediosSinRedondearYDetectaEmpate()
        {
            Competencia competencia = new Competencia("Torneo", "Marca", 2026, 5);
            Equipo uno = new Equipo("Uno", new DateOnly(2024, 1, 1), Categoria.Gaming);
            uno.RegistrarCreador(new Creador("Ana", "a", "Uno", 20, 0, 8m, Especialidad.Streamer));
            uno.RegistrarCreador(new Creador("Eva", "b", "Uno", 20, 0, 8.008m, Especialidad.Streamer));
            Equipo dos = new Equipo("Dos", new DateOnly(2024, 1, 1), Categoria.Gaming);
            dos.RegistrarCreador(new Creador("Luis", "a", "Dos", 20, 0, 8.001m, Especialidad.Streamer));
            Equipo tres = new Equipo("Tres", new DateOnly(2024, 1, 1), Categoria.Gaming);
            tres.RegistrarCreador(new Creador("Sol", "a", "Tres", 20, 0, 8.004m, Especialidad.Streamer));
            competencia.RegistrarEquipo(uno);
            competencia.RegistrarEquipo(dos);
            competencia.RegistrarEquipo(tres);

            Assert.Equal(ResultadoComparacion.GanaPrimero, competencia.CompararEquipos("Uno", "Dos"));
            Assert.Equal(ResultadoComparacion.GanaSegundo, competencia.CompararEquipos("Dos", "Uno"));
            Assert.Equal(ResultadoComparacion.Empate, competencia.CompararEquipos("Uno", "Tres"));
        }

        [Fact]
        public void EquiposSinCreadoresSeExcluyenDeSeguidoresYTop()
        {
            Competencia competencia = new Competencia("Torneo", "Marca", 2026, 5);
            competencia.RegistrarEquipo(new Equipo("Vacio", new DateOnly(2024, 1, 1), Categoria.Gaming));

            Assert.Null(competencia.ObtenerEquipoConMasSeguidores());
            Assert.Empty(competencia.ObtenerTop3());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(500)]
        public void EmpateDeSeguidoresConservaPrimerEquipoConCreadores(int seguidores)
        {
            Competencia competencia = new Competencia("Torneo", "Marca", 2026, 5);
            competencia.RegistrarEquipo(new Equipo("Vacio", new DateOnly(2024, 1, 1), Categoria.Gaming));
            Equipo primero = new Equipo("Zeta", new DateOnly(2024, 1, 1), Categoria.Gaming);
            primero.RegistrarCreador(new Creador("Ana", "a", "Zeta", 20, seguidores, 1, Especialidad.Streamer));
            Equipo segundo = new Equipo("Alfa", new DateOnly(2024, 1, 1), Categoria.Gaming);
            segundo.RegistrarCreador(new Creador("Luis", "a", "Alfa", 20, seguidores, 10, Especialidad.Streamer));
            competencia.RegistrarEquipo(primero);
            competencia.RegistrarEquipo(segundo);

            Assert.Same(primero, competencia.ObtenerEquipoConMasSeguidores());
        }

        [Fact]
        public void MayorTotalDeSeguidoresPuedeSuperarIntMaxValue()
        {
            Competencia competencia = new Competencia("Torneo", "Marca", 2026, 5);
            Equipo uno = new Equipo("Uno", new DateOnly(2024, 1, 1), Categoria.Gaming);
            uno.RegistrarCreador(new Creador("Ana", "a", "Uno", 20, int.MaxValue, 10, Especialidad.Streamer));
            Equipo dos = new Equipo("Dos", new DateOnly(2024, 1, 1), Categoria.Gaming);
            dos.RegistrarCreador(new Creador("Luis", "a", "Dos", 20, int.MaxValue, 1, Especialidad.Streamer));
            dos.RegistrarCreador(new Creador("Eva", "b", "Dos", 20, 1, 1, Especialidad.Streamer));
            competencia.RegistrarEquipo(uno);
            competencia.RegistrarEquipo(dos);

            Assert.Same(dos, competencia.ObtenerEquipoConMasSeguidores());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void TopIncluyeHastaTresCandidatos(int cantidad)
        {
            Competencia competencia = new Competencia("Torneo", "Marca", 2026, 5);
            for (int i = 1; i <= cantidad; i++)
            {
                string nombre = $"Equipo {i}";
                Equipo equipo = new Equipo(nombre, new DateOnly(2024, 1, 1), Categoria.Gaming);
                equipo.RegistrarCreador(new Creador("Ana", "a", nombre, 20, 0, i, Especialidad.Streamer));
                competencia.RegistrarEquipo(equipo);
            }

            List<Equipo> top = competencia.ObtenerTop3();

            Assert.Equal(Math.Min(cantidad, 3), top.Count);
            Assert.Equal($"Equipo {cantidad}", top[0].nombre);
        }

        [Fact]
        public void TopOrdenaPorPromedioExactoYDesempataPorNombre()
        {
            Competencia competencia = new Competencia("Torneo", "Marca", 2026, 10);
            string[] nombres = { "Zeta", "Beta", "Alfa", "Delta", "Mayor" };
            foreach (string nombre in nombres)
            {
                Equipo equipo = new Equipo(nombre, new DateOnly(2024, 1, 1), Categoria.Gaming);
                decimal valoracion = nombre == "Mayor" ? 8.004m : 8.001m;
                equipo.RegistrarCreador(new Creador("Ana", "a", nombre, 20, 0, valoracion, Especialidad.Streamer));
                competencia.RegistrarEquipo(equipo);
            }
            competencia.RegistrarEquipo(new Equipo("Vacio", new DateOnly(2024, 1, 1), Categoria.Gaming));

            List<Equipo> top = competencia.ObtenerTop3();

            Assert.Equal(new[] { "Mayor", "Alfa", "Beta" }, top.Select(e => e.nombre));
            Assert.Equal(nombres.Append("Vacio"), competencia.equipos.Select(e => e.nombre));
            top.Clear();
            Assert.Equal(3, competencia.ObtenerTop3().Count);
        }

        [Fact]
        public void ModificarListaConsultadaNoEliminaEquiposRegistrados()
        {
            Competencia competencia = new Competencia("Torneo", "Marca", 2026, 5);
            competencia.RegistrarEquipo(new Equipo("Uno", new DateOnly(2024, 1, 1), Categoria.Gaming));
            competencia.equipos.Clear();

            Assert.Single(competencia.equipos);
        }
    }
}
