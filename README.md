# MovieManagementAPI

### Multi-project ASP.NET Core solution with a REST API, MVC client, EF Core / SQL Server persistence, repository-based data access, and automated tests

MovieManagementAPI is a **.NET 10** solution that separates movie management into an ASP.NET Core Web API, an independent MVC client, a persistence library, and an automated test project.

The API provides CRUD operations together with search, sorting, and pagination. The MVC application consumes the API over HTTP through a configured `IHttpClientFactory` client, while data access is isolated behind a repository abstraction using Entity Framework Core and SQL Server.

---

## Solution Flow

```text
Browser
   ↓
Movies.Client
ASP.NET Core MVC
   ↓
IHttpClientFactory
named MoviesApi client
   ↓
Movies.API
ASP.NET Core Web API
   ↓
IMovieRepository
   ↓
MovieRepository
   ↓
Entity Framework Core
   ↓
SQL Server
```

The solution is divided into four projects:

```text
MovieManagementAPI
│
├── Movies.API
│   └── REST API and application configuration
│
├── Movies.Client
│   └── MVC user interface and API consumption
│
├── Movies.Data
│   └── EF Core models, DbContext and repository
│
└── Movies.Tests
    └── xUnit tests with isolated test data
```

This keeps HTTP client concerns, API behavior, persistence, and testing separated without introducing additional architectural layers that the project does not require.

---

## API Capabilities

`Movies.API` exposes endpoints for:

* retrieving all movies
* retrieving a movie by ID
* creating a movie
* updating a movie
* deleting a movie
* searching movies by title
* sorting results by title
* paginating query results

Example search request:

```http
GET /api/Movies/search?s=Matrix&orderby=asc&per_page=10&page=1
```

Search, sorting, and pagination are composed against an EF Core `IQueryable` before the query is materialized.

```text
query parameters
      ↓
IQueryable<Movie>
      ↓
filter
      ↓
sort
      ↓
Skip / Take
      ↓
SQL query
      ↓
result
```

This avoids loading the complete movie table into application memory before applying pagination.

The repository also includes `Movies.API/Movies.API.http` with example requests for testing the API directly.

---

## MVC Client

`Movies.Client` is a separate ASP.NET Core MVC application that communicates with the API over HTTP.

It currently provides UI workflows for:

* listing movies
* creating a movie
* viewing movie details

API communication is performed through a named `HttpClient` created by `IHttpClientFactory`:

```text
Movies.Client
      ↓
IHttpClientFactory
      ↓
MoviesApi
      ↓
Movies.API
```

The API base address is stored in configuration:

```json
"MoviesApi": {
  "BaseUrl": "https://localhost:7253/"
}
```

Controllers therefore do not contain hardcoded API URLs.

JSON communication uses `System.Net.Http.Json`, including methods such as:

```csharp
GetFromJsonAsync<T>()
PostAsJsonAsync()
```

HTTP communication from the MVC client is asynchronous.

---

## Data Access

Persistence is implemented in the `Movies.Data` class library.

```text
IMovieRepository
        ↓
MovieRepository
        ↓
MovieManagementContext
        ↓
SQL Server
```

`MovieRepository` encapsulates CRUD and query operations performed through Entity Framework Core.

Read-only operations use:

```csharp
AsNoTracking()
```

where entity tracking is not required.

Repository contracts use nullable return types for lookup, update, and delete operations that may not find a matching movie.

SQL Server configuration belongs to the API host through:

```text
ConnectionStrings:MoviesDB
```

rather than being hardcoded inside `MovieManagementContext`.

---

## Error Handling

Expected API conditions are mapped to appropriate HTTP responses:

* `200 OK` for successful reads and updates
* `201 Created` after creating a movie
* `400 Bad Request` for invalid requests
* `404 Not Found` when a movie does not exist

Unexpected exceptions are handled centrally through the ASP.NET Core exception-handling pipeline.

The API registers `ProblemDetails` instead of returning raw exception messages from individual controller actions.

```text
unexpected exception
        ↓
Exception Handler
        ↓
ProblemDetails
        ↓
500 Internal Server Error
```

This keeps unexpected error handling outside normal controller flow and avoids exposing internal exception details to API clients.

---

## Testing

`Movies.Tests` uses **xUnit v3** and contains controller tests using two isolated persistence strategies.

### Repository with EF Core InMemory

```text
test
  ↓
MoviesController
  ↓
MovieRepository
  ↓
unique EF Core InMemory database
```

Each test instance receives its own InMemory database and explicitly seeded movie data.

This makes database-dependent tests reproducible without relying on records stored in a developer's local SQL Server instance.

### Lightweight Test Repository

Additional controller tests use an `IMovieRepository` test implementation backed by an in-memory `List<Movie>`:

```text
test
  ↓
MoviesController
  ↓
TestRepo
  ↓
List<Movie>
```

This allows controller behavior to be tested independently of Entity Framework Core.

Run the complete test suite with:

```bash
dotnet test
```

---

## Database Setup

Local SQL Server setup is provided through:

```text
database/setup.sql
```

The script creates the `MovieManagementDb` database and `Movie` table when they do not already exist.

The default development configuration expects:

```text
Server: .\SQLEXPRESS
Database: MovieManagementDb
Authentication: Windows Integrated Security
```

The connection can be changed through:

```text
ConnectionStrings:MoviesDB
```

without modifying the EF Core `DbContext`.

---

## Running Locally

### Prerequisites

* .NET 10 SDK
* SQL Server or SQL Server Express

### 1. Prepare the database

Run:

```text
database/setup.sql
```

against the local SQL Server instance.

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Start the API

```bash
dotnet run --project Movies.API/Movies.API.csproj
```

Default development HTTPS address:

```text
https://localhost:7253
```

### 4. Start the MVC client

In a second terminal:

```bash
dotnet run --project Movies.Client/Movies.Client.csproj
```

Default development HTTPS address:

```text
https://localhost:7008
```

The MVC client is configured to communicate with the API at the API address above.

In Development, the API also exposes its ASP.NET Core OpenAPI document.

---

## Technology Stack

**Backend**
C# · .NET 10 · ASP.NET Core Web API · REST

**Client**
ASP.NET Core MVC · Razor · `IHttpClientFactory` · `System.Net.Http.Json`

**Data**
Entity Framework Core · SQL Server · LINQ · Repository Pattern

**API Behavior**
Dependency Injection · ProblemDetails · OpenAPI

**Testing**
xUnit v3 · EF Core InMemory · Test Repository

---

## Design Scope

MovieManagementAPI is intentionally a **focused multi-project application** rather than a full enterprise platform.

Key design choices include:

* separate ASP.NET Core API and MVC applications;
* configuration-driven API consumption through `IHttpClientFactory`;
* repository-based EF Core persistence;
* database-side filtering, sorting, and pagination;
* centralized handling of unexpected API exceptions;
* reproducible tests using isolated in-memory data sources;
* a lightweight SQL script for local database setup.

The API provides full movie CRUD plus query functionality, while the MVC client intentionally implements the smaller list, create, and details workflow.

For this project, API controllers work directly with the repository and movie model. A larger system could introduce service and DTO layers, authentication and authorization, database migrations, richer validation, integration testing, and independent deployment infrastructure as those additional boundaries become justified.

---

## License

This project is licensed under the [MIT License](LICENSE).
