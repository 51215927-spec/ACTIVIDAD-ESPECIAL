using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTIVIDAD_ESPECIAL
{
    internal class Equipo
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
    }
}
