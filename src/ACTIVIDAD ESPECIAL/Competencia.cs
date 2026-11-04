using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataBases;

namespace ACTIVIDAD_ESPECIAL
{
    internal class Competencia : IEntidad<Competencia>
    {
        private const int MIN_AÑO_COMPETENCIA = 2000;
        private const int MAX_AÑO_COMPETENCIA = 2027;
        private const int MIN_CANT_MAX_EQUIPOS = 1;
        private const int MAX_CANT_MAX_EQUIPOS = 10_000;

        public string nombre { get; }
        public string patrocinador { get; }
        public int año { get; }
        public int cantMaxEquipos { get; }
        private List<Equipo> equiposRegistrados { get; }

        public Competencia(string nombre, string patrocinador, int año, int cantMaxEquipos)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre de la competencia es obligatorio.");
            if (string.IsNullOrWhiteSpace(patrocinador))
                throw new ArgumentException("El patrocinador es obligatorio.");

            if (año < MIN_AÑO_COMPETENCIA)
                throw new ArgumentException($"La competencia debe de ser despues de {MIN_AÑO_COMPETENCIA}");

            if (año > MAX_AÑO_COMPETENCIA)
                throw new ArgumentException($"La competencia debe de ser antes de {MAX_AÑO_COMPETENCIA}");

            if (cantMaxEquipos < MIN_CANT_MAX_EQUIPOS)
                throw new ArgumentException($"La competencia debe de tener al menos {MIN_CANT_MAX_EQUIPOS} equipos");

            if (cantMaxEquipos > MAX_CANT_MAX_EQUIPOS)
                throw new ArgumentException($"La competencia debe de tener menos de {MAX_CANT_MAX_EQUIPOS} equipos");

            this.nombre = nombre.Trim();
            this.patrocinador = patrocinador.Trim();
            this.año = año;
            this.cantMaxEquipos = cantMaxEquipos;
            equiposRegistrados = new List<Equipo>();
        }

        public List<Equipo> equipos
        {
            get
            {
                return new List<Equipo>(equiposRegistrados);
            }
        }

        public bool HayCupo()
        {
            return equiposRegistrados.Count < cantMaxEquipos;
        }

        public Equipo? BuscarEquipo(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return null;

            foreach (Equipo equipo in equiposRegistrados)
            {
                if (string.Equals(equipo.nombre, nombre.Trim(), StringComparison.OrdinalIgnoreCase))
                    return equipo;
            }

            return null;
        }

        public void RegistrarEquipo(Equipo equipo)
        {
            if (equipo == null)
                throw new RegistroException("El equipo es obligatorio.");
            if (BuscarEquipo(equipo.nombre) != null)
                throw new RegistroException("Ya existe un equipo con ese nombre.");
            if (!HayCupo())
                throw new RegistroException("No hay cupo para otro equipo.");

            equipo.AsignarCompetencia(nombre);
            equiposRegistrados.Add(equipo);
        }

        public ResultadoComparacion CompararEquipos(string nombrePrimero, string nombreSegundo)
        {
            Equipo? primero = BuscarEquipo(nombrePrimero);
            if (primero == null)
                return ResultadoComparacion.PrimerEquipoInexistente;

            Equipo? segundo = BuscarEquipo(nombreSegundo);
            if (segundo == null)
                return ResultadoComparacion.SegundoEquipoInexistente;
            if (primero == segundo)
                return ResultadoComparacion.MismoEquipo;
            if (primero.creadores.Count == 0 || segundo.creadores.Count == 0)
                return ResultadoComparacion.EquipoSinCreadores;

            decimal promedioPrimero = primero.CalcularValoracionPromedio();
            decimal promedioSegundo = segundo.CalcularValoracionPromedio();
            if (promedioPrimero > promedioSegundo)
                return ResultadoComparacion.GanaPrimero;
            if (promedioPrimero < promedioSegundo)
                return ResultadoComparacion.GanaSegundo;

            return ResultadoComparacion.Empate;
        }

        public Equipo? ObtenerEquipoConMasSeguidores()
        {
            Equipo? masSeguidores = null;
            long mayorTotal = 0;
            foreach (Equipo equipo in equiposRegistrados)
            {
                if (equipo.creadores.Count == 0)
                    continue;

                long total = equipo.CalcularTotalSeguidores();
                if (masSeguidores == null || total > mayorTotal)
                {
                    masSeguidores = equipo;
                    mayorTotal = total;
                }
            }

            return masSeguidores;
        }

        public List<Equipo> ObtenerTop3()
        {
            List<Equipo> mejores = new List<Equipo>();
            StringComparer comparador = StringComparer.Create(CultureInfo.GetCultureInfo("es-AR"), true);

            foreach (Equipo equipo in equiposRegistrados)
            {
                if (equipo.creadores.Count == 0)
                    continue;

                decimal promedio = equipo.CalcularValoracionPromedio();
                int posicion = 0;
                while (posicion < mejores.Count)
                {
                    decimal promedioActual = mejores[posicion].CalcularValoracionPromedio();
                    if (promedio > promedioActual ||
                        (promedio == promedioActual && comparador.Compare(equipo.nombre, mejores[posicion].nombre) < 0))
                        break;

                    posicion++;
                }

                mejores.Insert(posicion, equipo);
                if (mejores.Count > 3)
                    mejores.RemoveAt(3);
            }

            return mejores;
        }

        public static void Configurar(ConstructorEntidad<Competencia> entidad)
        {
            entidad.Archivo(Path.Combine(Datos.directorioModelo, "competencia.csv"))
                .Columna(c => c.nombre)
                .Columna(c => c.patrocinador)
                .Columna(c => c.año)
                .Columna(c => c.cantMaxEquipos);
        }
    }
}
