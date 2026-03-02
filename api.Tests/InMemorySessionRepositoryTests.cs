// xUnit: no [TestClass] needed — xUnit discovers public classes automatically.
// Each public method marked [Fact] is an independent test.
// [Theory] + [InlineData] runs the same test with multiple inputs.

public class InMemorySessionRepositoryTests
{
    // Arrange: shared setup — each test gets a fresh repo instance.
    // This is important: tests must be isolated, no shared mutable state.
    private readonly InMemorySessionRepository _repo = new();

    [Fact]
    public void GetAll_ReturnsSeededSessions()
    {
        // Act
        var sessions = _repo.GetAll();

        // Assert
        Assert.Equal(2, sessions.Count);
    }

    [Fact]
    public void GetById_WithValidId_ReturnsCorrectSession()
    {
        var session = _repo.GetById(1);

        Assert.NotNull(session);
        Assert.Equal("Push Day A", session.Name);
    }

    [Fact]
    public void GetById_WithInvalidId_ReturnsNull()
    {
        var session = _repo.GetById(999);

        Assert.Null(session);
    }

    [Fact]
    public void GetSets_ReturnsOnlySetsForThatSession()
    {
        var sets = _repo.GetSets(1);

        // Session 1 has 3 seeded sets — none from session 2 should appear
        Assert.Equal(3, sets.Count);
        Assert.All(sets, s => Assert.Equal(1, s.SessionId));
    }

    [Fact]
    public void AddSet_PersistsAndReturnsNewSet()
    {
        var request = new LogSetRequest("Squat", "Legs", 5, 100);

        var result = _repo.AddSet(1, request);

        Assert.Equal("Squat", result.ExerciseName);
        Assert.Equal(1, result.SessionId);
        // Verify it was actually saved
        Assert.Contains(_repo.GetSets(1), s => s.ExerciseName == "Squat");
    }

    [Fact]
    public void AddSet_NullMuscleGroup_DefaultsToEmptyString()
    {
        var request = new LogSetRequest("Pull-up", null, 10, 0);

        var result = _repo.AddSet(1, request);

        // Null muscle group should default to "" not null (our API contract)
        Assert.Equal("", result.MuscleGroup);
    }

    [Theory]
    [InlineData(1, 4)] // session 1 has 3 seeded sets → new set is #4
    [InlineData(2, 3)] // session 2 has 2 seeded sets → new set is #3
    public void AddSet_SetNumberIncrements(int sessionId, int expectedSetNumber)
    {
        var request = new LogSetRequest("Curl", "Biceps", 12, 20);

        var result = _repo.AddSet(sessionId, request);

        Assert.Equal(expectedSetNumber, result.SetNumber);
    }
}
