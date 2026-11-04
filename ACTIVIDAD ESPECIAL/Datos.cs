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
        internal static string directorioModelo { get; private set; } = Path.GetFullPath("datos");

        public static void Guardar(string directorio)
        {
            Competencia activa = competencia ?? throw new InvalidOperationException("No hay competencia para guardar.");
            string raiz = Path.GetFullPath(directorio);
            Directory.CreateDirectory(raiz);
            string preparada = Path.Combine(raiz, "preparacion-" + Guid.NewGuid().ToString("N"));
            string actual = Path.Combine(raiz, "actual");
            string respaldo = Path.Combine(raiz, "respaldo-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(preparada);

            Modelo modelo = CrearModelo(preparada);
            modelo.CargarTodo();
            modelo.Tabla<Competencia>().Add(activa);
            foreach (Equipo equipo in activa.equipos)
            {
                modelo.Tabla<Equipo>().Add(equipo);
                foreach (Creador creador in equipo.creadores)
                    modelo.Tabla<Creador>().Add(creador);
            }
            modelo.GuardarTodo();

            // Releer los tres archivos antes de publicar la carpeta completa.
            Modelo verificacion = CrearModelo(preparada);
            verificacion.Cargar<Competencia>(true);
            verificacion.Cargar<Equipo>(true);
            verificacion.Cargar<Creador>(true);
            bool habiaActual = Directory.Exists(actual);
            if (habiaActual)
                Directory.Move(actual, respaldo);
            try
            {
                Directory.Move(preparada, actual);
            }
            catch
            {
                if (habiaActual)
                    Directory.Move(respaldo, actual);
                throw;
            }
        }

        public static Competencia Leer(string directorio)
        {
            throw new NotImplementedException("La lectura de datos está pendiente de implementación.");
        }

        private static Modelo CrearModelo(string directorio)
        {
            string anterior = directorioModelo;
            try
            {
                directorioModelo = Path.GetFullPath(directorio);
                return new ConstructorModelo().Agregar<Competencia>().Agregar<Equipo>()
                    .Agregar<Creador>().Construir();
            }
            finally
            {
                directorioModelo = anterior;
            }
        }
    }
}
