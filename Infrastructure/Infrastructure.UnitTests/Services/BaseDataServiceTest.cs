using Infrastructure.UnitTests.Mocks;

namespace Infrastructure.UnitTests.Services;

public class BaseDataServiceTest
{
    private readonly Mock<IDbContextTransaction> _dbContextTransaction;

    private readonly Mock<IDbContextWrapper<MockDbContext>> _dbContextWrapper;

    private readonly Mock<ILogger<MockService>> _logger;

    private readonly MockService _mockService;

    public BaseDataServiceTest()
    {
        _dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper = new Mock<IDbContextWrapper<MockDbContext>>();
        _logger = new Mock<ILogger<MockService>>();

        _dbContextWrapper.Setup(
            s => s
            .BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_dbContextTransaction.Object);

        _mockService = new MockService(_dbContextWrapper.Object, _logger.Object);
    }

    [Fact]
    public async Task ExecuteSafeAsyncWithResult_Failed()
    {
        // arrange
        var result = false;

        // act
        try
        {
            result = await _mockService.RunWithReturnWithException();
        }
        catch (Exception)
        {
            // ignore
        }

        // assert
        result.Should().BeFalse();
        _dbContextTransaction.Verify(t => t.CommitAsync(CancellationToken.None), Times.Never);
        _dbContextTransaction.Verify(t => t.RollbackAsync(CancellationToken.None), Times.Once);

        _logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!
                    .Contains($"Transaction is rolled back")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteSafeAsyncWithResult_Success()
    {
        // arrange
        var result = false;

        // act
        result = await _mockService.RunWithReturnWithoutException();

        // assert
        result.Should().BeTrue();
        _dbContextTransaction.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _dbContextTransaction.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteSafeAsyncWithoutResult_Failed()
    {
        // arrange

        // act
        try
        {
            await _mockService.RunWithoutReturnWithException();
        }
        catch (Exception)
        {
            // ignore
        }

        // assert
        _dbContextTransaction.Verify(t => t.CommitAsync(CancellationToken.None), Times.Never);
        _dbContextTransaction.Verify(t => t.RollbackAsync(CancellationToken.None), Times.Once);

        _logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!
                    .Contains($"Transaction is rolled back")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteSafeAsyncWithoutResult_Success()
    {
        // arrange

        // act
        await _mockService.RunWithoutReturnWithoutException();

        // assert
        _dbContextTransaction.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _dbContextTransaction.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
