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
            if (edad < MIN_EDAD)
                throw new ArgumentException($"La edad debe ser mayor o igual a {MIN_EDAD}.");
            if (edad > MAX_EDAD)
                throw new ArgumentException($"La edad debe ser menor o igual a {MAX_EDAD}.");

            this.nombre = nombre;
            this.alias = alias;
            this.nombreEquipo = nombreEquipo;
            this.edad = edad;
            this.seguidores = seguidores;
            this.valoracion = valoracion;
            this.especialidad = especialidad;
        }

        public bool EsEstrella()
        {
            throw new NotImplementedException("El cálculo de Estrella está pendiente de implementación.");
        }

        public static void Configurar(ConstructorEntidad<Creador> entidad)
        {
            throw new NotImplementedException("La configuración CSV de creadores está pendiente de implementación.");
        }
    }
}
