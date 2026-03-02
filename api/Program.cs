var builder = WebApplication.CreateBuilder(args);

// CORS: browsers block cross-origin requests by default.
// SSR fetches (server-to-server) bypass CORS — only browser fetches need this.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Register the repository with the DI container.
// AddSingleton = one instance for the lifetime of the app (data persists across requests).
// To swap implementations: change this one line — endpoints are unaffected.
builder.Services.AddSingleton<ISessionRepository, InMemorySessionRepository>();

var app = builder.Build();

app.UseCors(); // must be before route mappings

// --- Endpoints ---
// ISessionRepository is injected automatically by the DI container.
// Minimal API parameter binding: if the type is registered in DI, it's resolved from there.

app.MapGet("/api/sessions", (ISessionRepository repo) =>
    repo.GetAll());

app.MapGet("/api/sessions/{id}", (int id, ISessionRepository repo) =>
{
    var session = repo.GetById(id);
    if (session is null) return Results.NotFound();
    return Results.Ok(new { session, sets = repo.GetSets(id) });
});

app.MapPost("/api/sessions/{sessionId}/sets", (int sessionId, LogSetRequest request, ISessionRepository repo) =>
{
    if (string.IsNullOrWhiteSpace(request.ExerciseName))
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["exerciseName"] = ["Exercise name is required"]
        });

    var newSet = repo.AddSet(sessionId, request);
    return Results.Created($"/api/sessions/{sessionId}", newSet);
});

app.Run();
