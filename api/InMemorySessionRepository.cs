// The concrete implementation — stores data in plain C# lists.
// Registered as a Singleton so the same instance (and same lists) is used
// across all requests. In production, swap this for EfSessionRepository or
// CosmosSessionRepository without changing a single line in Program.cs.
public class InMemorySessionRepository : ISessionRepository
{
    private readonly List<WorkoutSession> _sessions =
    [
        new(1, DateOnly.Parse("2026-03-01"), "Push Day A", "Felt strong"),
        new(2, DateOnly.Parse("2026-02-27"), "Pull Day A", "Grip gave out"),
    ];

    private readonly List<LoggedSet> _sets =
    [
        new(1, 1, "Bench Press",    "Chest",     1, 8,  80),
        new(2, 1, "Bench Press",    "Chest",     2, 8,  82.5m),
        new(3, 1, "Overhead Press", "Shoulders", 1, 10, 50),
        new(4, 2, "Deadlift",       "Back",      1, 5,  140),
        new(5, 2, "Barbell Row",    "Back",      1, 8,  80),
    ];

    public IReadOnlyList<WorkoutSession> GetAll() => _sessions.AsReadOnly();

    public WorkoutSession? GetById(int id) =>
        _sessions.FirstOrDefault(s => s.Id == id);

    public IReadOnlyList<LoggedSet> GetSets(int sessionId) =>
        _sets.Where(s => s.SessionId == sessionId).ToList().AsReadOnly();

    public LoggedSet AddSet(int sessionId, LogSetRequest request)
    {
        var newSet = new LoggedSet(
            Id: _sets.Count + 1,
            SessionId: sessionId,
            ExerciseName: request.ExerciseName,
            MuscleGroup: request.MuscleGroup ?? "",
            SetNumber: _sets.Count(s => s.SessionId == sessionId) + 1,
            Reps: request.Reps,
            WeightKg: request.WeightKg
        );
        _sets.Add(newSet);
        return newSet;
    }
}
