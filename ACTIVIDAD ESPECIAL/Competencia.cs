using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTIVIDAD_ESPECIAL
{
    internal class Competencia
    {
        private const int MIN_AÑO_COMPETENCIA = 2000;
        private const int MAX_AÑO_COMPETENCIA = 2027;
        private const int MIN_CANT_MAX_EQUIPOS = 1;
        private const int MAX_CANT_MAX_EQUIPOS = 10_000;

        public string nombre { get; }
        public string patrocinador { get; }
        public int año { get; }
        public int cantMaxEquipos { get; }

        public Competencia(string nombre, string patrocinador, int año, int cantMaxEquipos)
        {
            if (año < MIN_AÑO_COMPETENCIA)
                throw new ArgumentException($"La competencia debe de ser despues de {MIN_AÑO_COMPETENCIA}");

            if (año > MAX_AÑO_COMPETENCIA)
                throw new ArgumentException($"La competencia debe de ser antes de {MAX_AÑO_COMPETENCIA}");

            if (cantMaxEquipos < MIN_CANT_MAX_EQUIPOS)
                throw new ArgumentException($"La competencia debe de tener al menos {MIN_CANT_MAX_EQUIPOS} equipos");

            if (cantMaxEquipos > MAX_CANT_MAX_EQUIPOS)
                throw new ArgumentException($"La competencia debe de tener menos de {MAX_CANT_MAX_EQUIPOS} equipos");

            this.nombre = nombre;
            this.patrocinador = patrocinador;
            this.año = año;
            this.cantMaxEquipos = cantMaxEquipos;
        }
    }
}
