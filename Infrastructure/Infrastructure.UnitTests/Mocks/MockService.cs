namespace Infrastructure.UnitTests.Mocks;

public class MockService : BaseDataService<MockDbContext>
{
    public MockService(
        IDbContextWrapper<MockDbContext> dbContextWrapper,
        ILogger<MockService> logger)
        : base(dbContextWrapper, logger)
    {
    }

    public async Task<bool> RunWithReturnWithException()
    {
        return await ExecuteSafeAsync<bool>(() => throw new Exception());
    }

    public async Task<bool> RunWithReturnWithoutException()
    {
        return await ExecuteSafeAsync<bool>(() => Task.FromResult(true));
    }

    public async Task RunWithoutReturnWithException()
    {
        await ExecuteSafeAsync(() => throw new Exception());
    }

    public async Task RunWithoutReturnWithoutException()
    {
        await ExecuteSafeAsync(() => Task.CompletedTask);
    }
}
