# StreamORT — reconstrucción

Aplicación de consola C# con destino .NET 8. El proyecto está en desarrollo: el dominio y el menú principal están implementados; las operaciones del menú y la persistencia continúan según [IA/Plan.md](IA/Plan.md).

## Obtener y ejecutar el proyecto

Instalar el SDK de .NET 8, o un SDK compatible y el runtime .NET 8. Clonar incluyendo las bibliotecas:

```powershell
git clone --recurse-submodules https://github.com/51215927-spec/ACTIVIDAD-ESPECIAL.git
cd ACTIVIDAD-ESPECIAL
dotnet build "ACTIVIDAD ESPECIAL.sln"
dotnet run --project "ACTIVIDAD ESPECIAL/ACTIVIDAD ESPECIAL.csproj"
```

Si ya existe una copia clonada, inicializar las bibliotecas antes de compilar:

```powershell
git submodule update --init --recursive
```

`Mis Librerias` es un submódulo con una versión concreta fijada por el proyecto. La [guía de submódulos y distribución](IA/Submodulos-y-distribucion.md) explica cómo actualizarlo y crear un ZIP que incluya sus archivos. El ZIP de comprobación de la etapa 3.5 no es todavía la entrega final.
