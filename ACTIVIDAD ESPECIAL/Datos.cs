using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataBases;

namespace ACTIVIDAD_ESPECIAL
{
    internal static class Datos
    {
        public static Competencia? competencia { get; set; }

        public static void Guardar(string directorio)
        {
            throw new NotImplementedException("El guardado de datos está pendiente de implementación.");
        }

        public static Competencia Leer(string directorio)
        {
            throw new NotImplementedException("La lectura de datos está pendiente de implementación.");
        }

        private static Modelo CrearModelo(string directorio)
        {
            throw new NotImplementedException("La construcción del modelo CSV está pendiente de implementación.");
        }
    }
}
