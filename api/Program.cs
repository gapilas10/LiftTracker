var builder = WebApplication.CreateBuilder(args);

// CORS: browsers block cross-origin requests by default.
// This allows our Next.js dev server (port 3000) to call this API.
// SSR fetches (server-to-server) don't need CORS — only browser fetches do.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors(); // must be before any route mappings

// --- In-memory data (replaces a real DB for now) ---
// Using static lists so data persists across requests during the session.
// In production this would be EF Core + SQL Server or Cosmos DB.
var sessions = new List<WorkoutSession>
{
    new(1, DateOnly.Parse("2026-03-01"), "Push Day A", "Felt strong"),
    new(2, DateOnly.Parse("2026-02-27"), "Pull Day A", "Grip gave out"),
};

var sets = new List<LoggedSet>
{
    new(1, 1, "Bench Press", "Chest", 1, 8, 80),
    new(2, 1, "Bench Press", "Chest", 2, 8, 82.5m),
    new(3, 1, "Overhead Press", "Shoulders", 1, 10, 50),
    new(4, 2, "Deadlift", "Back", 1, 5, 140),
    new(5, 2, "Barbell Row", "Back", 1, 8, 80),
};

// --- Endpoints ---

// GET /api/sessions — list all sessions
app.MapGet("/api/sessions", () => sessions);

// GET /api/sessions/{id} — single session with its sets
app.MapGet("/api/sessions/{id}", (int id) =>
{
    var session = sessions.FirstOrDefault(s => s.Id == id);
    if (session is null) return Results.NotFound();

    var sessionSets = sets.Where(s => s.SessionId == id).ToList();
    return Results.Ok(new { session, sets = sessionSets });
});

// POST /api/sessions/{sessionId}/sets — log a new set
app.MapPost("/api/sessions/{sessionId}/sets", (int sessionId, LogSetRequest request) =>
{
    // Basic validation — return RFC 7807 problem details on failure
    if (string.IsNullOrWhiteSpace(request.ExerciseName))
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["exerciseName"] = ["Exercise name is required"]
        });

    var newSet = new LoggedSet(
        Id: sets.Count + 1,
        SessionId: sessionId,
        ExerciseName: request.ExerciseName,
        MuscleGroup: request.MuscleGroup ?? "",
        SetNumber: sets.Count(s => s.SessionId == sessionId) + 1,
        Reps: request.Reps,
        WeightKg: request.WeightKg
    );

    sets.Add(newSet);

    // 201 Created with a Location header pointing to the parent session
    return Results.Created($"/api/sessions/{sessionId}", newSet);
});

app.Run();

// --- Records (C# 9+ positional records = concise immutable DTOs) ---
record WorkoutSession(int Id, DateOnly Date, string Name, string Notes);
record LoggedSet(int Id, int SessionId, string ExerciseName, string MuscleGroup,
                 int SetNumber, int Reps, decimal WeightKg);
record LogSetRequest(string ExerciseName, string? MuscleGroup, int Reps, decimal WeightKg);
