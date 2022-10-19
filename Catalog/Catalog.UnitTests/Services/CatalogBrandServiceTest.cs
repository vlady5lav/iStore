namespace Catalog.UnitTests.Services;

public class CatalogBrandServiceTest
{
    private readonly Mock<ICatalogBrandRepository> _catalogBrandRepository;

    private readonly ICatalogBrandService _catalogBrandService;

    private readonly Mock<IDbContextTransaction> _dbContextTransaction;

    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;

    private readonly Mock<ILogger<CatalogBrandService>> _logger;

    private readonly CatalogBrand _testItem;

    public CatalogBrandServiceTest()
    {
        _catalogBrandRepository = new Mock<ICatalogBrandRepository>();
        _dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogBrandService>>();

        _testItem = new CatalogBrand() { Id = 1, Name = "Brand", };

        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_dbContextTransaction.Object);

        _catalogBrandService = new CatalogBrandService(
            _dbContextWrapper.Object,
            _logger.Object,
            _catalogBrandRepository.Object);
    }

    [Fact]
    public async Task AddAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogBrandRepository
            .Setup(s => s.AddAsync(It.Is<string>(i => i == _testItem.Name)))
            .ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.AddAsync(_testItem.Name);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddAsync_Success()
    {
        // arrange
        _catalogBrandRepository
            .Setup(s => s.AddAsync(It.Is<string>(i => i == _testItem.Name)))
            .ReturnsAsync(_testItem.Id);

        // act
        var result = await _catalogBrandService.AddAsync(_testItem.Name);

        // assert
        result.Should().Be(_testItem.Id);
    }

    [Fact]
    public async Task DeleteAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogBrandRepository
            .Setup(s => s.DeleteAsync(It.Is<int>(i => i == _testItem.Id)))
            .ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.DeleteAsync(_testItem.Id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task DeleteAsync_Success()
    {
        // arrange
        _catalogBrandRepository
            .Setup(s => s.DeleteAsync(It.Is<int>(i => i == _testItem.Id)))
            .ReturnsAsync(_testItem.Id);

        // act
        var result = await _catalogBrandService.DeleteAsync(_testItem.Id);

        // assert
        result.Should().Be(_testItem.Id);
    }

    [Fact]
    public async Task UpdateAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogBrandRepository
            .Setup(
                s =>
                    s.UpdateAsync(
                        It.Is<int>(i => i == _testItem.Id),
                        It.Is<string>(i => i == _testItem.Name)))
            .ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.UpdateAsync(_testItem.Id, _testItem.Name);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateAsync_Success()
    {
        // arrange
        _catalogBrandRepository
            .Setup(
                s =>
                    s.UpdateAsync(
                        It.Is<int>(i => i == _testItem.Id),
                        It.Is<string>(i => i == _testItem.Name)))
            .ReturnsAsync(_testItem.Id);

        // act
        var result = await _catalogBrandService.UpdateAsync(_testItem.Id, _testItem.Name);

        // assert
        result.Should().Be(_testItem.Id);
    }
}
