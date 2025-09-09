# TodoApi

Simple REST API for managing to-dos.  
Built with **.NET 8**, **Minimal API**, **EF Core (PostgreSQL)**, **AutoMapper**, and **FluentValidation**.  
Includes unit and integration tests with **xUnit**.

## Requirements

- .NET SDK 8.x  
- PostgreSQL 13+ (only if you run with a real database)

## Build & test

```bash
dotnet restore
dotnet build -c Release
dotnet test
```

## API endpoints

- `GET    /api/todos` – list all  
- `GET    /api/todos/{id}` – get by id  
- `GET    /api/todos/incoming?range=Today|Tomorrow|Week` – incoming (UTC, week = Mon–Sun)  
- `POST   /api/todos` – create  
- `PUT    /api/todos/{id}` – update  
- `PATCH  /api/todos/{id}/percent` – set completion (0..100)  
- `POST   /api/todos/{id}/done` – mark as done  
- `DELETE /api/todos/{id}` – delete

## Notes

- Validation: FluentValidation + endpoint filters → invalid data = HTTP 400.  
- Mapping: AutoMapper, DTOs are exposed instead of EF entities.  
- Database: SQLite in-memory in Testing, PostgreSQL otherwise.  
- Migrations: included in repo (`Migrations/`), applied automatically at startup.  