# Minimal APIs with PostgreSQL

A tiny C# minimal API project to learn minimal APIs.

## Prerequisites

- .NET 9
- Docker

## Setup & Run

1. **Clone the repository**
```bash
   git clone <url>
   cd minimal_apis
```

2. **Start PostgreSQL with Docker**
```bash
   docker compose up -d
```
   Creates a PostgreSQL container with mock data on port 5433.

3. **Run the application**
```bash
   dotnet run --project .\minimal_apis\minimal_apis\
```

4. **Access the API**
   - Base URL: `http://localhost:5155`
   - Swagger UI: `http://localhost:5155/swagger`

## Endpoints

- **Appointments**: `/appointments`
- **Doctors**: `/doctors`
- **Patients**: `/patients`
- **Locations**: `/locations`
- **Services**: `/services`

Endpoint:
- `GET /` - Get all
- `GET /{id}` - Get by ID
- `POST /` - Create
- `PUT /{id}` - Update
- `DELETE /{id}` - Delete

## Testing

Use the included `api-tests.http` file or import `minimal_apis_[app].json` into either Postman or Bruno.

## Database

The database resets on each `docker compose up`. Mock data is loaded from `mock-data.sql`.

EF Core models generated with:
```bash
dotnet ef dbcontext scaffold "Name=ConnectionStrings:DefaultConnection" Npgsql.EntityFrameworkCore.PostgreSQL -o Models -c MyDbContext --force --schema main
```

## Cleanup
```bash
docker compose down -v
```
