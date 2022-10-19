namespace Catalog.UnitTests.Services;

public class CatalogTypeServiceTest
{
    private readonly Mock<ICatalogTypeRepository> _catalogTypeRepository;

    private readonly ICatalogTypeService _catalogTypeService;

    private readonly Mock<IDbContextTransaction> _dbContextTransaction;

    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;

    private readonly Mock<ILogger<CatalogTypeService>> _logger;

    private readonly CatalogType _testItem;

    public CatalogTypeServiceTest()
    {
        _catalogTypeRepository = new Mock<ICatalogTypeRepository>();
        _dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogTypeService>>();

        _testItem = new CatalogType() { Id = 1, Name = "Type", };

        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_dbContextTransaction.Object);

        _catalogTypeService = new CatalogTypeService(
            _dbContextWrapper.Object,
            _logger.Object,
            _catalogTypeRepository.Object);
    }

    [Fact]
    public async Task AddAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogTypeRepository
            .Setup(s => s.AddAsync(It.Is<string>(i => i == _testItem.Name)))
            .ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.AddAsync(_testItem.Name);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddAsync_Success()
    {
        // arrange
        _catalogTypeRepository
            .Setup(s => s.AddAsync(It.Is<string>(i => i == _testItem.Name)))
            .ReturnsAsync(_testItem.Id);

        // act
        var result = await _catalogTypeService.AddAsync(_testItem.Name);

        // assert
        result.Should().Be(_testItem.Id);
    }

    [Fact]
    public async Task DeleteAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogTypeRepository
            .Setup(s => s.DeleteAsync(It.Is<int>(i => i == _testItem.Id)))
            .ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.DeleteAsync(_testItem.Id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task DeleteAsync_Success()
    {
        // arrange
        _catalogTypeRepository
            .Setup(s => s.DeleteAsync(It.Is<int>(i => i == _testItem.Id)))
            .ReturnsAsync(_testItem.Id);

        // act
        var result = await _catalogTypeService.DeleteAsync(_testItem.Id);

        // assert
        result.Should().Be(_testItem.Id);
    }

    [Fact]
    public async Task UpdateAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogTypeRepository
            .Setup(
                s =>
                    s.UpdateAsync(
                        It.Is<int>(i => i == _testItem.Id),
                        It.Is<string>(i => i == _testItem.Name)))
            .ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.UpdateAsync(_testItem.Id, _testItem.Name);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateAsync_Success()
    {
        // arrange
        _catalogTypeRepository
            .Setup(
                s =>
                    s.UpdateAsync(
                        It.Is<int>(i => i == _testItem.Id),
                        It.Is<string>(i => i == _testItem.Name)))
            .ReturnsAsync(_testItem.Id);

        // act
        var result = await _catalogTypeService.UpdateAsync(_testItem.Id, _testItem.Name);

        // assert
        result.Should().Be(_testItem.Id);
    }
}
