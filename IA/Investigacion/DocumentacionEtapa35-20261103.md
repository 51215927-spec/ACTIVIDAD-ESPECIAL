# Documentación junto a cada biblioteca y cierre de etapa 3.5

Fecha: 03/11/2026. Pedido: continuar con la documentación, mantener el README de entrada corto y ubicar la guía de cada biblioteca en su directorio como README.

## Organización resultante

```text
Mis Librerias/
├── README.md                  introducción, propósito y dos enlaces
├── Utilidades/
│   └── README.md              guía completa de la biblioteca
└── DataBases/
    └── README.md              modelo, relaciones, CSV y errores
```

No existe carpeta Documentacion ni guías duplicadas dentro de DataBases. El README de entrada de ACTIVIDAD-ESPECIAL también queda corto, con introducción y enlaces. Sus instrucciones de clonación/compilación siguen disponibles en [Submodulos-y-distribucion.md](../Submodulos-y-distribucion.md).

Antes de editar se copiaron y verificaron por SHA-256 seis documentos originales en `IA/Verificacion/DocumentacionEtapa35/Respaldo`. Al consolidar DataBases se eligieron las versiones más actuales que estaban junto a su código: incluyen bloqueo entre procesos, CsvConcurrenciaException y conservación de instancia de tabla al recargar; las versiones de Documentacion eran anteriores. Se conserva el contenido detallado en el README combinado, distinguiendo la verificación de esta revisión de los antecedentes de ensayos reportados por las guías previas. Los originales también son recuperables en el historial Git.

La eliminación se limitó a los cinco Markdown reemplazados. Las rutas se comprobaron dentro del submódulo y el directorio Documentacion se eliminó únicamente cuando quedó vacío. No se borran código, datos, artefactos ni la carpeta histórica DataBases viejo.

## Actualización de contenido

- Utilidades: ColoresMenu y FormatoMenu, firmas públicas, compatibilidad de dos/tres/cuatro argumentos, diez colores y ocho opciones de formato, ejemplos de menú y selección numérica, diferencias de navegación/historial entre firmas anteriores y nuevas, entrada redirigida, EOF y salida UTF-8.
- Se corrigen afirmaciones antiguas de «sin compilar» y reglas generales que ya no aplicaban a FormatoMenu; se conserva el comportamiento real de MostrarTabla con bordes y de los mensajes con etiquetas. No se modifica la biblioteca para hacerla coincidir con la guía.
- DataBases: API actual basada en IEntidad/ConstructorModelo/Modelo/Tabla y CsvManager interno; configuración, relaciones, formatos y escape propios de CSV, bloqueos, excepciones y límites. Se explicita que CargarTodo/GuardarTodo no son transacciones, aunque bloqueen todos los archivos, y que la aplicación debe preparar su propio estado temporal si necesita reemplazo integral.
- README de Mis-Librerias: ocho líneas, solo introducción, propósito y enlaces. README principal del proyecto: introducción y enlaces, sin instrucciones extensas. Las guías completas quedan en los README de las bibliotecas según el pedido.
- AGENTS.md actualiza las rutas y registra la preferencia documental del usuario. Plan y arquitectura reflejan el estado final y el próximo paso de la etapa 4; no se implementan aún sus flujos.

## Verificación

1. Revisión de 40 enlaces relativos e índices de los cuatro README: todos resuelven a archivos o encabezados existentes; no se comprueban sitios externos. Los README de entrada no superan 14 líneas y no hay carpeta Documentacion.
2. Compilación de Mis Librerias.slnx y ACTIVIDAD ESPECIAL.sln con SDK 10.0.401, net8.0 y artefactos aislados. Cero errores; CA1416 previa en Utilidades y CA1416/CS0219 previas en la solución principal. NuGetAudit=false solo durante estas compilaciones.
3. Tres fragmentos se extraen directamente de las guías, sin reescribir a mano sus cuerpos: menú completo con formato; selección numérica con un using y salida de control; programa completo de guardado/lectura de una nota. Compilan y se ejecutan sobre runtime local .NET 8.0.33, cada uno en una carpeta temporal separada.
4. Entrada redirigida del menú: elegir 2 devuelve índice 1. Selección numérica: x y 5 se rechazan, 3 devuelve índice 2. Ejemplo CSV: guarda y recarga una nota; salida `Notas cargadas: 1`, archivo con decimal `8.5` y texto esperado.
5. Diff de código C# y proyectos respecto de f4af93a: vacío. Solo cambia documentación en el repositorio de bibliotecas. Los auxiliares, compilaciones y respaldos se mantienen fuera del proyecto académico.

Resultados y hashes de respaldos en [DocumentacionEtapa35-20261103.json](DocumentacionEtapa35-20261103.json). Los scripts auxiliares de extracción/comprobación quedan en IA/Verificacion/DocumentacionEtapa35; las guías contienen los ejemplos ejecutables para repetirlos en un proyecto de consola compatible. No se agregan pruebas unitarias ni se certifican todas las operaciones de persistencia, concurrencia o sistemas operativos en este paso.

## Publicación

Cuenta gh `51215927-spec`, permiso ADMIN comprobado en ambos repositorios. Primero se publicó en Mis-Librerias el commit `097f0a5ca02c0d1d0bdb01c03fd5a8fd5bb56eb6`, con README corto, guías reubicadas y duplicados retirados. Después se prepara/publica la referencia correspondiente y la documentación del principal.

La etapa 3.5 queda completa para su alcance vigente. MostrarTabla no fue adaptado porque el usuario retiró esa tarea; la etapa 4 continúa pendiente para conectar las operaciones y las fichas. El ZIP final, los tests unitarios y el documento de entrega siguen en sus etapas previstas.
