using ACTIVIDAD_ESPECIAL;

namespace StreamORT.Tests
{
    public class EquipoTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ConstructorAceptaCategoriasYFechaActual(int categoria)
        {
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Today);
            Equipo equipo = new Equipo(" Equipo Uno ", hoy, (Categoria)categoria, " Torneo ");

            Assert.Equal("Equipo Uno", equipo.nombre);
            Assert.Equal("Torneo", equipo.nombreCompetencia);
            Assert.Equal(hoy, equipo.fechaDeCreacion);
            Assert.Equal((Categoria)categoria, equipo.categoria);
            Assert.Empty(equipo.creadores);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ConstructorRechazaNombreVacio(string? nombre)
        {
            Assert.Throws<ArgumentException>(() =>
                new Equipo(nombre!, new DateOnly(2024, 2, 29), Categoria.Gaming));
        }

        [Fact]
        public void ConstructorRechazaFechaFutura()
        {
            DateOnly mañana = DateOnly.FromDateTime(DateTime.Today).AddDays(1);

            Assert.Throws<ArgumentException>(() => new Equipo("Equipo", mañana, Categoria.Gaming));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(4)]
        public void ConstructorRechazaCategoriaInexistente(int categoria)
        {
            Assert.Throws<ArgumentException>(() =>
                new Equipo("Equipo", new DateOnly(2024, 2, 29), (Categoria)categoria));
        }

        [Fact]
        public void EquipoVacioDevuelveEstadisticasNeutras()
        {
            Equipo equipo = new Equipo("Equipo", new DateOnly(2024, 2, 29), Categoria.Humor);

            Assert.Equal(0m, equipo.CalcularValoracionPromedio());
            Assert.Equal(0, equipo.ContarStreamers());
            Assert.Equal(0L, equipo.CalcularTotalSeguidores());
            Assert.Null(equipo.ObtenerCreadorMasPopular());
            Assert.Null(equipo.BuscarCreador("ausente"));
        }

        [Theory]
        [InlineData("alias")]
        [InlineData("ALIAS")]
        [InlineData("  aLiAs  ")]
        public void BuscaAliasSinDistinguirCajaNiEspaciosExteriores(string consulta)
        {
            Equipo equipo = new Equipo("Equipo", new DateOnly(2024, 2, 29), Categoria.Musica);
            Creador creador = new Creador("Ana", "alias", "equipo", 20, 10, 8, Especialidad.Streamer);
            equipo.RegistrarCreador(creador);

            Assert.Same(creador, equipo.BuscarCreador(consulta));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("ausente")]
        public void BuscarAliasAusenteDevuelveNull(string? consulta)
        {
            Equipo equipo = new Equipo("Equipo", new DateOnly(2024, 2, 29), Categoria.Musica);
            equipo.RegistrarCreador(new Creador("Ana", "alias", "Equipo", 20, 0, 8, Especialidad.Streamer));

            Assert.Null(equipo.BuscarCreador(consulta!));
        }

        [Fact]
        public void RechazaAltasInvalidasSinModificarLaLista()
        {
            Equipo equipo = new Equipo("Equipo", new DateOnly(2024, 2, 29), Categoria.Gaming);
            Creador primero = new Creador("Ana", "alias", "Equipo", 20, 0, 8, Especialidad.Streamer);
            equipo.RegistrarCreador(primero);

            Assert.Throws<RegistroException>(() => equipo.RegistrarCreador(null!));
            Assert.Throws<RegistroException>(() => equipo.RegistrarCreador(
                new Creador("Luis", " ALIAS ", "Equipo", 21, 0, 9, Especialidad.Youtuber)));
            Assert.Throws<RegistroException>(() => equipo.RegistrarCreador(
                new Creador("Luis", "otro", "Otro equipo", 21, 0, 9, Especialidad.Youtuber)));
            Assert.Same(primero, Assert.Single(equipo.creadores));
        }

        [Fact]
        public void PermiteMismoAliasEnEquiposDistintos()
        {
            Equipo primero = new Equipo("Uno", new DateOnly(2024, 1, 1), Categoria.Gaming);
            Equipo segundo = new Equipo("Dos", new DateOnly(2024, 1, 1), Categoria.Humor);
            primero.RegistrarCreador(new Creador("Ana", "alias", "Uno", 20, 0, 8, Especialidad.Streamer));
            segundo.RegistrarCreador(new Creador("Luis", "alias", "Dos", 20, 0, 8, Especialidad.Streamer));

            Assert.Single(primero.creadores);
            Assert.Single(segundo.creadores);
        }

        [Fact]
        public void EstadisticasConservanDecimalesYSumanMasAllaDeIntMaxValue()
        {
            Equipo equipo = new Equipo("Equipo", new DateOnly(2024, 1, 1), Categoria.Gaming);
            equipo.RegistrarCreador(new Creador("Ana", "a", "Equipo", 20, int.MaxValue, 8.001m, Especialidad.Streamer));
            equipo.RegistrarCreador(new Creador("Luis", "b", "Equipo", 20, 1, 8.004m, Especialidad.Youtuber));
            equipo.RegistrarCreador(new Creador("Eva", "c", "Equipo", 20, 0, 8.001m, Especialidad.Tiktoker));
            equipo.RegistrarCreador(new Creador("Sol", "d", "Equipo", 20, 0, 7.998m, Especialidad.Influencer));

            Assert.Equal(8.001m, equipo.CalcularValoracionPromedio());
            Assert.Equal(2147483648L, equipo.CalcularTotalSeguidores());
            Assert.Equal(1, equipo.ContarStreamers());
            Assert.Equal("a", equipo.ObtenerCreadorMasPopular()!.alias);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(500)]
        public void PopularidadEnEmpateConservaPrimeroRegistrado(int seguidores)
        {
            Equipo equipo = new Equipo("Equipo", new DateOnly(2024, 1, 1), Categoria.Gaming);
            Creador primero = new Creador("Ana", "z", "Equipo", 20, seguidores, 1, Especialidad.Streamer);
            equipo.RegistrarCreador(primero);
            equipo.RegistrarCreador(new Creador("Luis", "a", "Equipo", 20, seguidores, 10, Especialidad.Streamer));

            Assert.Same(primero, equipo.ObtenerCreadorMasPopular());
        }

        [Fact]
        public void PopularidadEligeMayorAunqueSeRegistreDespues()
        {
            Equipo equipo = new Equipo("Equipo", new DateOnly(2024, 1, 1), Categoria.Gaming);
            equipo.RegistrarCreador(new Creador("Ana", "a", "Equipo", 20, 1, 10, Especialidad.Streamer));
            Creador mayor = new Creador("Luis", "b", "Equipo", 20, 2, 1, Especialidad.Streamer);
            equipo.RegistrarCreador(mayor);

            Assert.Same(mayor, equipo.ObtenerCreadorMasPopular());
        }

        [Fact]
        public void ModificarListaConsultadaNoEliminaCreadoresRegistrados()
        {
            Equipo equipo = new Equipo("Equipo", new DateOnly(2024, 1, 1), Categoria.Gaming);
            equipo.RegistrarCreador(new Creador("Ana", "a", "Equipo", 20, 1, 8, Especialidad.Streamer));
            equipo.creadores.Clear();

            Assert.Single(equipo.creadores);
        }
    }
}
