using ACTIVIDAD_ESPECIAL;

if (args.Length == 3 && args[0] == "guardar-prueba")
{
    PruebaIdaVuelta.Guardar(args[1], args[2]);
    return;
}
if (args.Length == 3 && args[0] == "leer-prueba")
{
    PruebaIdaVuelta.Leer(args[1]);
    return;
}
PruebaIdaVuelta.Ejecutar();
PruebaDecimales.Ejecutar();

string raiz = Path.Combine(Path.GetTempPath(), "StreamORT-" + Guid.NewGuid().ToString("N"));
Competencia competencia = new Competencia("Prueba", "Patrocinador", 2026, 2);
Equipo equipo = new Equipo("Musica", new DateOnly(2020, 1, 1), Categoria.Musica);
equipo.RegistrarCreador(new Creador("Ana", "a", equipo.nombre, 20, int.MaxValue, 8.004m, Especialidad.Streamer));
competencia.RegistrarEquipo(equipo);
Datos.competencia = competencia;
Datos.Guardar(raiz);
string actual = Path.Combine(raiz, "actual");
string[] archivos = { "competencia.csv", "equipos.csv", "creadores.csv" };
string[] anteriores = archivos.Select(a => File.ReadAllText(Path.Combine(actual, a))).ToArray();
if (!anteriores[2].Contains("8.004"))
    throw new Exception("Se perdio precision decimal.");
Datos.Guardar(raiz);
if (Directory.GetDirectories(raiz, "respaldo-*").Length != 1)
    throw new Exception("Falta el respaldo completo.");
foreach (string archivo in archivos)
    if (File.ReadAllText(Path.Combine(actual, archivo)) != anteriores[Array.IndexOf(archivos, archivo)])
        throw new Exception("El guardado altero los campos.");
string bloqueada = Path.Combine(raiz, "bloqueada");
Directory.CreateDirectory(bloqueada);
File.WriteAllText(Path.Combine(bloqueada, "actual"), "obstaculo");
try
{
    Datos.Guardar(bloqueada);
    throw new Exception("Debio fallar la publicacion.");
}
catch (IOException)
{
    if (File.ReadAllText(Path.Combine(bloqueada, "actual")) != "obstaculo")
        throw new Exception("Se modifico el destino bloqueado.");
}
Console.WriteLine("Correcto: tres CSV, precision decimal, respaldo y rechazo de destino bloqueado.");
Console.WriteLine("Datos de prueba conservados en: " + raiz);

Competencia leida = Datos.Leer(raiz);
if (leida.equipos.Count != 1 || leida.equipos[0].creadores.Count != 1 ||
    leida.equipos[0].creadores[0].valoracion != 8.004m ||
    !ReferenceEquals(Datos.competencia, competencia))
    throw new Exception("La lectura no reconstruye datos o modifica el estado activo.");
string rutaCreadores = Path.Combine(actual, "creadores.csv");
File.WriteAllText(rutaCreadores, "contenido invalido");
bool rechazo = false;
try { Datos.Leer(raiz); }
catch (DataBases.CsvFormatException) { rechazo = true; }
if (!rechazo || !ReferenceEquals(Datos.competencia, competencia))
    throw new Exception("Un actual corrupto debe rechazarse sin cambiar memoria.");
File.WriteAllText(rutaCreadores, anteriores[2]);
string copiaActual = Path.Combine(raiz, "actual-apartado");
Directory.Move(actual, copiaActual);
if (Datos.Leer(raiz).equipos[0].creadores[0].valoracion != 8.004m)
    throw new Exception("No recupera el respaldo.");
Directory.Move(copiaActual, actual);
Console.WriteLine("Correcto: lectura temporal, asociaciones, rechazo de corrupcion y recuperacion del respaldo.");
