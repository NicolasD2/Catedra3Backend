# Catedra3 IDWM

Este es el proyecto backend para Catedra 3, construido con C# y .NET. El proyecto proporciona endpoints API para gestionar Post de los usuarios.

## Características

## Tecnologías

- **ASP.NET Core**: Framework backend.
- **Entity Framework Core**: ORM para interacción con la base de datos.
- **JWT**: Autenticación segura con tokens.
- **Sqlite**: Base de datos (configurable en `appsettings.json`).
- **Cloudinary** Plataforma que ofrece servicios de gestion de imagen

## Requisitos

Antes de comenzar, asegúrate de haber cumplido con los siguientes requisitos:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0.4)
- [Visual Studio o Visual Studio Code](https://visualstudio.microsoft.com/es/downloads/)
- [Base de datos SQL (SQL Server u otro)]()

## Instalación

1. Clona el repositorio:

   ```bash
   git clone https://github.com/xSharkz/TallerIDWMBackend.git
   ```

2. Navega al directorio del proyecto:

   ```bash
   cd PostCatedraApi
   ```

3. Restaura las dependencias:

   ```bash
   dotnet restore
   ```

4. Configura la conexión a la base de datos en `appsettings.json`.
 ```bash
            {
        "ConnectionStrings": {
            "DefaultConnection": "Data Source=app.db"
        },
        "CloudinarySettings": {
            "CloudName": "tu-cloud-name",
            "ApiKey": "tu-api-key",
            "ApiSecret": "tu-api-secret"
        }
    }
```
5. Aplica migraciones a la base de datos:

   ```bash
   dotnet ef database update
   ```

6. ejecuta la aplicacion

   ```bash
   dotnet run
   ```