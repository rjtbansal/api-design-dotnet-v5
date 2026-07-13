# Habit Tracker API — .NET

A .NET (ASP.NET Core Web API) mirror of [`api-design-node-v5`](../api-design-node-v5), the starter project for Frontend Masters' "API Design with Node.js, v5" course. Every route, verb, and response body here is a direct copy of that repo's dummy Express routes — this exists so a Node/Express concept can be looked up side-by-side with its .NET equivalent while going through the course.

Like the Node repo, this is a **starter/stub scaffold**: controllers return static JSON, and there's no database, auth, or tests wired up yet.

## Concept mapping (Node/Express → .NET)

| Node repo | This repo | Notes |
|---|---|---|
| `src/index.ts` + `src/server.ts` | `HabitTrackerApi/Program.cs` | Builds the app, registers middleware, mounts routes, starts listening — all in one file in .NET's minimal hosting model |
| `src/routes/authRoutes.ts`, `habitRoutes.ts`, `userRoutes.ts` | `Controllers/AuthController.cs`, `HabitsController.cs`, `UsersController.cs` | Express: one `Router` per file, mounted with `app.use('/api/x', xRouter)`. .NET: one class per resource; `[Route("api/x")]` on the class replaces the manual mount line, `[HttpGet]`/`[HttpPost]`/etc. replace `router.get(...)`/`router.post(...)` |
| `req.params.id` | method parameter named `id` | ASP.NET Core binds route template placeholders (`{id}`) straight to same-named method parameters — no manual destructuring |
| `env.ts` (Zod-validated env vars, `.env`/`.env.example`) | `appsettings.json` + `appsettings.Development.json` | ASP.NET Core layers config automatically: `appsettings.json` → `appsettings.{Environment}.json` → environment variables → command-line args, later sources overriding earlier ones. There's no built-in Zod-style "fail fast on invalid config" — add that yourself with an Options class + `ValidateDataAnnotations()`/`ValidateOnStart()` if you want the same fail-fast behavior `env.ts` gives the Node app |
| `drizzle.config.ts` + (planned) `src/db/schema.ts` | (planned) `Data/AppDbContext.cs` + entity classes in `Models/` | Neither this repo nor the Node one has the DB layer built yet. When you get there: `dotnet ef migrations add <Name>` ≈ `drizzle-kit generate`, `dotnet ef database update` ≈ running the generated migration |
| Postgres via `pg`/Drizzle | Postgres via `Npgsql.EntityFrameworkCore.PostgreSQL` | Same database, so schema/query decisions from the course transfer directly |
| `jose` (JWT) + `bcrypt` | `Microsoft.AspNetCore.Authentication.JwtBearer` + `BCrypt.Net-Next` | Both installed as package references, neither wired up yet — same "installed but unused" state as the Node repo's `package.json` |
| `helmet()` | hand-written header middleware in `Program.cs` | No built-in bundle-of-security-headers equivalent exists; sets the same handful of headers helmet defaults to. `NetEscapades.AspNetCore.SecurityHeaders` is a NuGet drop-in if full parity is ever needed |
| `cors()` | `AddCors()` + `UseCors()` | Built-in, two-step (register policy with DI, then apply in the pipeline) the same way `[Http*]` attributes go through `AddControllers()` |
| `express.json()`, `express.urlencoded()` | *(nothing — automatic)* | `AddControllers()` already parses JSON/form bodies via model binding; Express requires opting in, ASP.NET Core doesn't |
| `morgan('dev', { skip: () => isTest() })` | `AddHttpLogging()` + `UseHttpLogging()`, gated by `app.Environment.IsEnvironment("Test")` | Built into the shared framework, no package needed. The `Microsoft.AspNetCore.HttpLogging` category needs its own `Information` log-level override in `appsettings.Development.json`, since the base `appsettings.json` sets `Microsoft.AspNetCore` to `Warning` |
| Vitest + Supertest (`vitest.config.ts`, planned `tests/`) | xUnit + `WebApplicationFactory<Program>` (`HabitTrackerApi.Tests/`) | `WebApplicationFactory<Program>` spins up the app in-memory the same way importing `app` from `server.ts` lets Supertest hit it without a real port. The test project is scaffolded but empty — same "configured, not yet used" state as the Node repo's test setup |
| `npm run dev` | `dotnet watch run` | Auto-restart on file change |
| `npm start` | `dotnet run` | Run once, no watch |
| `npm test` | `dotnet test` | Run the test suite |

## Setup

Requires the .NET SDK (this project targets `net10.0`, matching the installed SDK — package references like EF Core are pinned to their `9.0.x` releases since that's the current stable line for those libraries, but that's independent of the app's own target framework).

```bash
dotnet restore
dotnet build
```

From this directory:

```bash
dotnet restore
dotnet build
dotnet run --project HabitTrackerApi
```

The API listens on the port ASP.NET Core's default `launchSettings`/`ASPNETCORE_URLS` assigns (typically `http://localhost:5000` or `:5080` depending on SDK version — printed to the console on startup). Hit it the same way as the Node API:

```bash
curl http://localhost:5000/health
curl -X POST http://localhost:5000/api/auth/login
```

Run tests:

```bash
dotnet test
```

## Structure

```
HabitTrackerApi.sln
HabitTrackerApi/
  Program.cs                  # app startup — see Node comparison comments inside
  appsettings.json            # base config (≈ .env.example structure)
  appsettings.Development.json
  Controllers/
    AuthController.cs         # ≈ authRoutes.ts
    HabitsController.cs       # ≈ habitRoutes.ts
    UsersController.cs        # ≈ userRoutes.ts
  HabitTrackerApi.csproj
HabitTrackerApi.Tests/
  HabitTrackerApi.Tests.csproj
```

Each controller file has inline comments pointing back to the exact line/pattern it mirrors in the Node repo's corresponding route file.
