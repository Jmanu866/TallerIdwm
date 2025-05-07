# Taller1Idwm - E-Commerce API (.NET 9 + SQLite)

Este proyecto corresponde al Taller 1 de la asignatura **Introducción al Desarrollo Web Móvil**. Se trata de una API REST desarrollada en **.NET 9 Web API**, orientada al manejo de productos y operaciones típicas de un sistema E-Commerce. El almacenamiento de datos se realiza con **SQLite** y se utilizan herramientas modernas como **Husky.Net** para gestionar tareas automáticas pre-commit.

---

##  Tecnologías y Librerías Utilizadas

###  Framework y Acceso a Datos

- **.NET 9 Web API**
- **SQLite** como base de datos relacional local
- **Entity Framework Core 9.0.4**
- **Microsoft.EntityFrameworkCore.Design 9.0.3**  
  (incluye herramientas para migraciones y scaffolding)

###  Generación de Datos

- **Bogus 35.6.3**  
  Librería para generar datos falsos con fines de prueba.


###  Logging con Serilog

El proyecto utiliza **Serilog** como sistema de logging estructurado con múltiples fuentes de enriquecimiento y salidas:

- **Serilog 4.2.1-dev-02352**
- **Serilog.AspNetCore 9.0.0**

#### Enrichers:
- `Serilog.Enrichers.Environment 3.0.1`
- `Serilog.Enrichers.Process 3.0.0`
- `Serilog.Enrichers.Thread 4.0.0`
- `FromLogContext`, `WithMachineName`, `WithThreadId` (en configuración)

#### Sinks:
- `Serilog.Sinks.File 7.0.0-dev-02301`  
  Guarda archivos diarios de logs en formato `.txt`
- `Serilog.Sinks.Seq 9.0.0`  
  (opcional) Permite visualizar logs estructurados en [Seq](https://datalust.co/seq)


---

## Requisitos Previos

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- [Git](https://git-scm.com/)
- [Postman](https://www.postman.com/) (para pruebas)
- [Visual Code](https://code.visualstudio.com/) o IDE de preferencia


---

## Paso a Paso 
1. ### Clonar Repositorio 

```bash
git clone https://github.com/Jmanu866/TallerIdwm.git

# Navigate to the project directory
cd TallerIdwm/TallerIdwm
```
2. ### Configuración de `appsettings.json`

Este archivo es fundamental para el correcto funcionamiento de la aplicación. A continuación, se muestra un ejemplo mínimo funcional de `appsettings.json` utilizado en este proyecto, el cual usa **SQLite** como base de datos y **Serilog** para manejo estructurado de logs.

---

### Ejemplo de `appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=store.db"
  },
  "JWT": {
      "Key":"klXB1cdloHJNDwMYKJxCl4gjhHhvRe5QkdnKB+a37PFdsqeRA3tveXITszI6DUjnZ/LlghM6UZJi1m/jf+ZW8gexKHl1x5wg4M+Q4a1L8jk=",
      "Issuer": "https://localhost:7123", 
      "Audience": "https://localhost:7123"
      } ,    
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.AspNetCore.Hosting.Diagnostics": "Error",
        "Microsoft.Hosting.Lifetime": "Information",
        "System": "Error"
      }
    },
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ],
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.txt",
          "rollingInterval": "Day",
          "restrictedToMinimumLevel": "Information"
        }
      }
    ]
  },
  "AllowedHosts": "*"
}
```

3. ### Restaura los paquetes y herramientas

```bash
dotnet restore
```

4. ### Ejecuta la aplicación

```bash
dotnet run
```

La API estará disponible en: [https://localhost:7123](https://localhost:7123)  

---

## Integrantes

| Nombre Completo       | Correo Institucional         | RUT              |
|------------------------|------------------------------|------------------|
|      Josemanuel Espinoza                 |                    josemanuel.espinoza@alumnos.ucn.cl          |      21.228.513-7            |
|            Matias Palavicino           |        matias.palavicino@alumnos.ucn.cl                      |     21.206.337-1             |

---

## Licencia

Proyecto de uso académico para la Universidad Católica del Norte. Todos los derechos reservados.
