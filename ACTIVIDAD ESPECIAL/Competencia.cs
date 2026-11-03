using System;
using System.Collections.Generic;
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

            equiposRegistrados.Add(equipo);
        }

        public ResultadoComparacion CompararEquipos(string nombrePrimero, string nombreSegundo)
        {
            throw new NotImplementedException("La comparación de equipos está pendiente de implementación.");
        }

        public Equipo? ObtenerEquipoConMasSeguidores()
        {
            throw new NotImplementedException("La consulta del equipo con más seguidores está pendiente de implementación.");
        }

        public List<Equipo> ObtenerTop3()
        {
            throw new NotImplementedException("El TOP 3 está pendiente de implementación.");
        }

        public static void Configurar(ConstructorEntidad<Competencia> entidad)
        {
            throw new NotImplementedException("La configuración CSV de competencias está pendiente de implementación.");
        }
    }
}
