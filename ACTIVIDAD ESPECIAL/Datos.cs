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
            string raiz = Path.GetFullPath(directorio);
            string actual = Path.Combine(raiz, "actual");
            if (Directory.Exists(actual) || File.Exists(actual))
                return LeerConjunto(actual);

            if (!Directory.Exists(raiz))
                throw new DirectoryNotFoundException("No existe el directorio de datos.");

            // Solo recuperar respaldos si falta actual; un actual corrupto debe informar error.
            string[] respaldos = Directory.GetDirectories(raiz, "respaldo-*");
            Array.Sort(respaldos, (a, b) => Directory.GetLastWriteTimeUtc(b)
                .CompareTo(Directory.GetLastWriteTimeUtc(a)));
            foreach (string respaldo in respaldos)
            {
                try
                {
                    return LeerConjunto(respaldo);
                }
                catch (Exception excepcion) when (excepcion is IOException ||
                    excepcion is ArgumentException || excepcion is RegistroException ||
                    excepcion is System.Reflection.TargetInvocationException)
                {
                    // Probar el siguiente respaldo completo sin modificar los archivos.
                }
            }
            throw new IOException("No hay un conjunto actual ni un respaldo válido para recuperar.");
        }

        private static Competencia LeerConjunto(string directorio)
        {
            string[] archivos = { "competencia.csv", "equipos.csv", "creadores.csv" };
            foreach (string archivo in archivos)
            {
                string ruta = Path.Combine(directorio, archivo);
                if (!File.ReadLines(ruta).Any(linea => !string.IsNullOrWhiteSpace(linea)))
                    throw new IOException($"El archivo {archivo} está vacío.");
            }

            Modelo modelo = CrearModelo(directorio);
            Tabla<Competencia> competencias = modelo.Cargar<Competencia>(true);
            Tabla<Equipo> equipos = modelo.Cargar<Equipo>(true);
            Tabla<Creador> creadores = modelo.Cargar<Creador>(true);
            if (competencias.Count != 1)
                throw new IOException("Debe haber exactamente una competencia en el archivo.");

            Competencia recuperada = competencias[0];
            foreach (Equipo equipo in equipos)
            {
                if (!string.Equals(equipo.nombreCompetencia, recuperada.nombre,
                    StringComparison.OrdinalIgnoreCase))
                    throw new IOException("Hay un equipo asociado a otra competencia.");
                recuperada.RegistrarEquipo(equipo);
            }
            foreach (Creador creador in creadores)
            {
                Equipo equipo = recuperada.BuscarEquipo(creador.nombreEquipo)
                    ?? throw new IOException("Hay un creador asociado a un equipo inexistente.");
                equipo.RegistrarCreador(creador);
            }
            return recuperada;
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
