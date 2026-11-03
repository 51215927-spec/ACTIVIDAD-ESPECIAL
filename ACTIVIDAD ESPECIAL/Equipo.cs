using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataBases;

namespace ACTIVIDAD_ESPECIAL
{
    internal class Equipo : IEntidad<Equipo>
    {
        public string nombre { get; }
        public DateOnly fechaDeCreacion { get; }
        public Categoria categoria { get; }
        private List<Creador> creadoresRegistrados { get; }

        public Equipo(string nombre, DateOnly fechaDeCreacion, Categoria categoria) 
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del equipo es obligatorio.");
            if (fechaDeCreacion > DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException("La fecha de creación no puede ser futura.");
            if (!Enum.IsDefined(typeof(Categoria), categoria))
                throw new ArgumentException("La categoría no es válida.");

            this.nombre = nombre.Trim();
            this.fechaDeCreacion = fechaDeCreacion;
            this.categoria = categoria;
            creadoresRegistrados = new List<Creador>();
        }

        public List<Creador> creadores
        {
            get
            {
                return new List<Creador>(creadoresRegistrados);
            }
        }

        public Creador? BuscarCreador(string alias)
        {
            if (string.IsNullOrWhiteSpace(alias))
                return null;

            foreach (Creador creador in creadoresRegistrados)
            {
                if (string.Equals(creador.alias, alias.Trim(), StringComparison.OrdinalIgnoreCase))
                    return creador;
            }

            return null;
        }

        public void RegistrarCreador(Creador creador)
        {
            if (creador == null)
                throw new RegistroException("El creador es obligatorio.");
            if (!string.Equals(creador.nombreEquipo, nombre, StringComparison.OrdinalIgnoreCase))
                throw new RegistroException("El creador pertenece a otro equipo.");
            if (BuscarCreador(creador.alias) != null)
                throw new RegistroException("Ya existe un creador con ese alias en el equipo.");

            creadoresRegistrados.Add(creador);
        }

        public decimal CalcularValoracionPromedio()
        {
            if (creadoresRegistrados.Count == 0)
                return 0;

            decimal total = 0;
            foreach (Creador creador in creadoresRegistrados)
            {
                total += creador.valoracion;
            }

            return total / creadoresRegistrados.Count;
        }

        public int ContarStreamers()
        {
            throw new NotImplementedException("El conteo de streamers está pendiente de implementación.");
        }

        public long CalcularTotalSeguidores()
        {
            throw new NotImplementedException("El total de seguidores está pendiente de implementación.");
        }

        public Creador? ObtenerCreadorMasPopular()
        {
            throw new NotImplementedException("La consulta del creador más popular está pendiente de implementación.");
        }

        public static void Configurar(ConstructorEntidad<Equipo> entidad)
        {
            throw new NotImplementedException("La configuración CSV de equipos está pendiente de implementación.");
        }
    }
}
