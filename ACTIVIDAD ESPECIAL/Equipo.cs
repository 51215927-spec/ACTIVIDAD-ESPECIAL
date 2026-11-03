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

        public Equipo(string nombre, DateOnly fechaDeCreacion, Categoria categoria) 
        {
            this.nombre = nombre;
            this.fechaDeCreacion = fechaDeCreacion;
            this.categoria = categoria;
        }

        public List<Creador> creadores
        {
            get
            {
                throw new NotImplementedException("La consulta de creadores está pendiente de implementación.");
            }
        }

        public Creador? BuscarCreador(string alias)
        {
            throw new NotImplementedException("La búsqueda de creadores está pendiente de implementación.");
        }

        public bool RegistrarCreador(Creador creador, out string error)
        {
            throw new NotImplementedException("El registro de creadores está pendiente de implementación.");
        }

        public decimal CalcularValoracionPromedio()
        {
            throw new NotImplementedException("El promedio de valoración está pendiente de implementación.");
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
