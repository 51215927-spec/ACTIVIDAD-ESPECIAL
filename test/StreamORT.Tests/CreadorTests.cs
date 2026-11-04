using ACTIVIDAD_ESPECIAL;

namespace StreamORT.Tests
{
    public class CreadorTests
    {
        [Theory]
        [InlineData(14, 0, 1)]
        [InlineData(30, int.MaxValue, 10)]
        public void ConstructorAceptaLimitesYConservaDatos(int edad, int seguidores, int valoracion)
        {
            Creador creador = new Creador(" Ana María ", " alias ", " Equipo Uno ",
                edad, seguidores, valoracion, Especialidad.Influencer);

            Assert.Equal("Ana María", creador.nombre);
            Assert.Equal("alias", creador.alias);
            Assert.Equal("Equipo Uno", creador.nombreEquipo);
            Assert.Equal(edad, creador.edad);
            Assert.Equal(seguidores, creador.seguidores);
            Assert.Equal((decimal)valoracion, creador.valoracion);
            Assert.Equal(Especialidad.Influencer, creador.especialidad);
        }

        [Theory]
        [InlineData(null, "alias", "Equipo")]
        [InlineData("", "alias", "Equipo")]
        [InlineData("  ", "alias", "Equipo")]
        [InlineData("Ana", null, "Equipo")]
        [InlineData("Ana", "", "Equipo")]
        [InlineData("Ana", "  ", "Equipo")]
        [InlineData("Ana", "alias", null)]
        [InlineData("Ana", "alias", "")]
        [InlineData("Ana", "alias", "  ")]
        public void ConstructorRechazaTextosVacios(string? nombre, string? alias, string? equipo)
        {
            Assert.Throws<ArgumentException>(() =>
                new Creador(nombre!, alias!, equipo!, 20, 500, 8, Especialidad.Streamer));
        }

        [Theory]
        [InlineData(13, 0, "8")]
        [InlineData(31, 0, "8")]
        [InlineData(20, -1, "8")]
        [InlineData(20, 0, "0.999")]
        [InlineData(20, 0, "10.001")]
        public void ConstructorRechazaNumerosFueraDeRango(int edad, int seguidores, string valor)
        {
            decimal valoracion = decimal.Parse(valor, System.Globalization.CultureInfo.InvariantCulture);

            Assert.Throws<ArgumentException>(() =>
                new Creador("Ana", "alias", "Equipo", edad, seguidores, valoracion, Especialidad.Streamer));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(4)]
        public void ConstructorRechazaEspecialidadInexistente(int especialidad)
        {
            Assert.Throws<ArgumentException>(() =>
                new Creador("Ana", "alias", "Equipo", 20, 500, 8, (Especialidad)especialidad));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void EstrellaRespetaAmbosUmbralesSinRedondearEnTodaEspecialidad(int especialidad)
        {
            Especialidad tipo = (Especialidad)especialidad;
            Creador limite = new Creador("Ana", "a", "Equipo", 20, 500, 8m, tipo);
            Creador pocosSeguidores = new Creador("Ana", "b", "Equipo", 20, 499, 10m, tipo);
            Creador bajaValoracion = new Creador("Ana", "c", "Equipo", 20, 500, 7.999m, tipo);
            Creador decimalValido = new Creador("Ana", "d", "Equipo", 20, 501, 8.001m, tipo);

            Assert.True(limite.EsEstrella());
            Assert.False(pocosSeguidores.EsEstrella());
            Assert.False(bajaValoracion.EsEstrella());
            Assert.True(decimalValido.EsEstrella());
            Assert.Equal(7.999m, bajaValoracion.valoracion);
            Assert.Equal(8.001m, decimalValido.valoracion);
        }
    }
}
