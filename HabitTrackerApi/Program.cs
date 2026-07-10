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

var app = builder.Build();

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
