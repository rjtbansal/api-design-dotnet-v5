using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Node comparison: this line plus `builder.Configuration` together replace
// env.ts. ASP.NET Core auto-loads appsettings.json, then
// appsettings.{Environment}.json, then environment variables, in that order —
// no separate Zod-style validation step exists by default (see README.md).

builder.Services.AddControllers();
// Node comparison: nothing mounts routes yet at this point — this just
// registers the routing/model-binding pipeline that later scans
// Controllers/*.cs for [Route]/[Http*] attributes, the same way
// server.ts imports each router file before mounting it.

// Node comparison: app.use(cors()) with no options allows any origin/
// method/header. AddCors registers a named (here, default) policy; the
// actual "turn it on" call is app.UseCors() below, same two-step split
// [Http*] attributes go through (register with DI, then apply in the
// pipeline).
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Node comparison: app.use(morgan('dev', { skip: () => isTest() })).
// HttpLogging is part of the ASP.NET Core shared framework (no NuGet
// package needed, unlike morgan). Registration happens here; whether it
// actually runs is decided below with UseHttpLogging, gated the same way
// skip: () => isTest() gates morgan.
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestMethod
        | HttpLoggingFields.RequestPath
        | HttpLoggingFields.ResponseStatusCode;
});

var app = builder.Build();

// Node comparison: app.use(helmet()). ASP.NET Core has no built-in
// bundle-of-security-headers middleware like helmet, so this is a small
// hand-written stand-in covering helmet's most load-bearing defaults. For
// full parity (CSP, more header coverage), swap this for the
// NetEscapades.AspNetCore.SecurityHeaders NuGet package.
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("Referrer-Policy", "no-referrer");
    await next();
});

// Node comparison: app.use(cors())
app.UseCors();

// Node comparison: app.use(morgan('dev', { skip: () => isTest() })).
// IsEnvironment("Test") is the ASP.NET Core analogue of isTest() from
// env.ts — but it only returns true if something sets ASPNETCORE_ENVIRONMENT
// (or calls builder.UseEnvironment) to "Test". WebApplicationFactory in
// HabitTrackerApi.Tests defaults to "Development" until that's set explicitly.
if (!app.Environment.IsEnvironment("Test"))
{
    app.UseHttpLogging();
}

// Node comparison: express.json() / express.urlencoded({ extended: true }).
// No equivalent call needed — AddControllers() above already wires up
// System.Text.Json-based body parsing for [ApiController] actions via
// model binding, automatically, for both JSON and form bodies.

// Node comparison: app.get('/health', (req, res) => res.status(200).json({ status: 'ok' }))
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapControllers();
// Node comparison: the three
//   app.use('/api/auth', authRoutes)
//   app.use('/api/habits', habitRoutes)
//   app.use('/api/users', userRoutes)
// lines in server.ts. Here, each controller's own [Route("api/...")]
// attribute declares its mount point instead of it being wired up centrally.

app.Run();
// Node comparison: app.listen(env.PORT, ...) in index.ts.
// Listen address/port come from appsettings.json "Urls" or the
// ASPNETCORE_URLS env var instead of a custom PORT var.

// Node comparison: `export { app }` in server.ts, which lets supertest
// import the app without starting a real listener. This partial class
// declaration is what WebApplicationFactory<Program> hooks into for the
// same purpose in the test project.
public partial class Program { }
