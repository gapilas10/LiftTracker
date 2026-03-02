// The interface defines the contract — what operations exist.
// Endpoints depend on this abstraction, not on a concrete implementation.
// That's the D in SOLID (Dependency Inversion Principle).
public interface ISessionRepository
{
    IReadOnlyList<WorkoutSession> GetAll();
    WorkoutSession? GetById(int id);
    IReadOnlyList<LoggedSet> GetSets(int sessionId);
    LoggedSet AddSet(int sessionId, LogSetRequest request);
}
