// Positional records = concise immutable data shapes (C# 9+).
// The compiler generates constructor, properties, Equals, ToString automatically.
public record WorkoutSession(int Id, DateOnly Date, string Name, string Notes);
public record LoggedSet(int Id, int SessionId, string ExerciseName, string MuscleGroup,
                        int SetNumber, int Reps, decimal WeightKg);
public record LogSetRequest(string ExerciseName, string? MuscleGroup, int Reps, decimal WeightKg);
