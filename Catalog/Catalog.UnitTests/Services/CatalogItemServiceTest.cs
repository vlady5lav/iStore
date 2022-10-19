namespace Catalog.UnitTests.Services;

public class CatalogItemServiceTest
{
    private readonly Mock<ICatalogItemRepository> _catalogItemRepository;

    private readonly ICatalogItemService _catalogItemService;

    private readonly Mock<IDbContextTransaction> _dbContextTransaction;

    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;

    private readonly Mock<ILogger<CatalogItemService>> _logger;

    private readonly CatalogItem _testItem;

    public CatalogItemServiceTest()
    {
        _catalogItemRepository = new Mock<ICatalogItemRepository>();
        _dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogItemService>>();

        _testItem = new CatalogItem()
        {
            Id = 1,
            Name = "Name",
            AvailableStock = 100,
            Price = 1000,
            Warranty = 12,
            CatalogBrandId = 1,
            CatalogTypeId = 1,
            Description = "Description",
            PictureFileName = "1.png",
        };

        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_dbContextTransaction.Object);

        _catalogItemService = new CatalogItemService(
            _dbContextWrapper.Object,
            _logger.Object,
            _catalogItemRepository.Object);
    }

    [Fact]
    public async Task AddAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogItemRepository
            .Setup(
                s =>
                    s.AddAsync(
                        It.Is<string>(i => i == _testItem.Name),
                        It.Is<int>(i => i == _testItem.AvailableStock),
                        It.Is<decimal>(i => i == _testItem.Price),
                        It.Is<int>(i => i == _testItem.Warranty),
                        It.Is<int>(i => i == _testItem.CatalogBrandId),
                        It.Is<int>(i => i == _testItem.CatalogTypeId),
                        It.Is<string>(i => i == _testItem.Description),
                        It.Is<string>(i => i == _testItem.PictureFileName)))
            .ReturnsAsync(testResult);

        // act
        var result = await _catalogItemService.AddAsync(
            _testItem.Name,
            _testItem.AvailableStock,
            _testItem.Price,
            _testItem.Warranty,
            _testItem.CatalogBrandId,
            _testItem.CatalogTypeId,
            _testItem.Description,
            _testItem.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddAsync_Success()
    {
        // arrange
        _catalogItemRepository
            .Setup(
                s =>
                    s.AddAsync(
                        It.Is<string>(i => i == _testItem.Name),
                        It.Is<int>(i => i == _testItem.AvailableStock),
                        It.Is<decimal>(i => i == _testItem.Price),
                        It.Is<int>(i => i == _testItem.Warranty),
                        It.Is<int>(i => i == _testItem.CatalogBrandId),
                        It.Is<int>(i => i == _testItem.CatalogTypeId),
                        It.Is<string>(i => i == _testItem.Description),
                        It.Is<string>(i => i == _testItem.PictureFileName)))
            .ReturnsAsync(_testItem.Id);

        // act
        var result = await _catalogItemService.AddAsync(
            _testItem.Name,
            _testItem.AvailableStock,
            _testItem.Price,
            _testItem.Warranty,
            _testItem.CatalogBrandId,
            _testItem.CatalogTypeId,
            _testItem.Description,
            _testItem.PictureFileName);

        // assert
        result.Should().Be(_testItem.Id);
    }

    [Fact]
    public async Task DeleteAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogItemRepository
            .Setup(s => s.DeleteAsync(It.Is<int>(i => i == _testItem.Id)))
            .ReturnsAsync(testResult);

        // act
        var result = await _catalogItemService.DeleteAsync(_testItem.Id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task DeleteAsync_Success()
    {
        // arrange
        _catalogItemRepository
            .Setup(s => s.DeleteAsync(It.Is<int>(i => i == _testItem.Id)))
            .ReturnsAsync(_testItem.Id);

        // act
        var result = await _catalogItemService.DeleteAsync(_testItem.Id);

        // assert
        result.Should().Be(_testItem.Id);
    }

    [Fact]
    public async Task UpdateAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogItemRepository
            .Setup(
                s =>
                    s.UpdateAsync(
                        It.Is<int>(i => i == _testItem.Id),
                        It.Is<string>(i => i == _testItem.Name),
                        It.Is<int>(i => i == _testItem.AvailableStock),
                        It.Is<decimal>(i => i == _testItem.Price),
                        It.Is<int>(i => i == _testItem.Warranty),
                        It.Is<int>(i => i == _testItem.CatalogBrandId),
                        It.Is<int>(i => i == _testItem.CatalogTypeId),
                        It.Is<string>(i => i == _testItem.Description),
                        It.Is<string>(i => i == _testItem.PictureFileName)))
            .ReturnsAsync(testResult);

        // act
        var result = await _catalogItemService.UpdateAsync(
            _testItem.Id,
            _testItem.Name,
            _testItem.AvailableStock,
            _testItem.Price,
            _testItem.Warranty,
            _testItem.CatalogBrandId,
            _testItem.CatalogTypeId,
            _testItem.Description,
            _testItem.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateAsync_Success()
    {
        // arrange
        _catalogItemRepository
            .Setup(
                s =>
                    s.UpdateAsync(
                        It.Is<int>(i => i == _testItem.Id),
                        It.Is<string>(i => i == _testItem.Name),
                        It.Is<int>(i => i == _testItem.AvailableStock),
                        It.Is<decimal>(i => i == _testItem.Price),
                        It.Is<int>(i => i == _testItem.Warranty),
                        It.Is<int>(i => i == _testItem.CatalogBrandId),
                        It.Is<int>(i => i == _testItem.CatalogTypeId),
                        It.Is<string>(i => i == _testItem.Description),
                        It.Is<string>(i => i == _testItem.PictureFileName)))
            .ReturnsAsync(_testItem.Id);

        // act
        var result = await _catalogItemService.UpdateAsync(
            _testItem.Id,
            _testItem.Name,
            _testItem.AvailableStock,
            _testItem.Price,
            _testItem.Warranty,
            _testItem.CatalogBrandId,
            _testItem.CatalogTypeId,
            _testItem.Description,
            _testItem.PictureFileName);

        // assert
        result.Should().Be(_testItem.Id);
    }
}
