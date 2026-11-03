using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataBases;

namespace ACTIVIDAD_ESPECIAL
{
    internal class Creador : IEntidad<Creador>
    {
        public const int MIN_EDAD = 14;
        public const int MAX_EDAD = 30;

        public string nombre { get; }
        public string alias { get; }
        public string nombreEquipo { get; }
        public int edad { get; }
        public decimal valoracion { get; }
        public int seguidores { get; }
        public Especialidad especialidad { get; }

        public Creador(string nombre, string alias, string nombreEquipo, int edad,
            int seguidores, decimal valoracion, Especialidad especialidad)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del creador es obligatorio.");
            if (string.IsNullOrWhiteSpace(alias))
                throw new ArgumentException("El alias del creador es obligatorio.");
            if (string.IsNullOrWhiteSpace(nombreEquipo))
                throw new ArgumentException("El nombre del equipo es obligatorio.");

            if (edad < MIN_EDAD)
                throw new ArgumentException($"La edad debe ser mayor o igual a {MIN_EDAD}.");
            if (edad > MAX_EDAD)
                throw new ArgumentException($"La edad debe ser menor o igual a {MAX_EDAD}.");

            if (seguidores < 0)
                throw new ArgumentException("Los seguidores deben ser mayores o iguales a 0.");
            if (valoracion < 1 || valoracion > 10)
                throw new ArgumentException("La valoración debe estar entre 1 y 10.");
            if (!Enum.IsDefined(typeof(Especialidad), especialidad))
                throw new ArgumentException("La especialidad no es válida.");

            this.nombre = nombre.Trim();
            this.alias = alias.Trim();
            this.nombreEquipo = nombreEquipo.Trim();
            this.edad = edad;
            this.seguidores = seguidores;
            this.valoracion = valoracion;
            this.especialidad = especialidad;
        }

        public bool EsEstrella()
        {
            return seguidores >= 500 && valoracion >= 8;
        }

        public static void Configurar(ConstructorEntidad<Creador> entidad)
        {
            throw new NotImplementedException("La configuración CSV de creadores está pendiente de implementación.");
        }
    }
}
