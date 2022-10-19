using Catalog.Host.Models.Responses;

namespace Catalog.UnitTests.Services;

public class CatalogServiceTest
{
    private readonly ICatalogService _catalogService;

    private readonly Mock<ICatalogRepository> _catalogRepository;

    private readonly Mock<IDbContextTransaction> _dbContextTransaction;

    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;

    private readonly Mock<ILogger<CatalogService>> _logger;

    private readonly Mock<IMapper> _mapper;

    public CatalogServiceTest()
    {
        _catalogRepository = new Mock<ICatalogRepository>();
        _dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();
        _mapper = new Mock<IMapper>();

        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_dbContextTransaction.Object);

        _catalogService = new CatalogService(
            _dbContextWrapper.Object,
            _logger.Object,
            _mapper.Object,
            _catalogRepository.Object);
    }

    [Fact]
    public async Task GetBrandsAsync_Failed()
    {
        // arrange
        string? exMessage = null;
        IEnumerable<CatalogBrandDto>? result = null;

        _catalogRepository
            .Setup(s => s.GetBrandsAsync())
            .ReturnsAsync((Func<IEnumerable<CatalogBrand?>?>)null!);

        // act
        try
        {
            result = await _catalogService.GetBrandsAsync();
        }
        catch (BusinessException ex)
        {
            exMessage = ex.Message;
        }

        // assert
        result?.Should().BeNull();
        exMessage?.Should().Match("Brands couldn't be fetched");
    }

    [Fact]
    public async Task GetBrandsAsync_Success()
    {
        // arrange
        var catalogBrand = new CatalogBrand() { Id = 1, Name = "Brand", };

        var catalogBrandDto = new CatalogBrandDto() { Id = 1, Name = "Brand", };

        IEnumerable<CatalogBrand> catalogBrandsSuccess = new List<CatalogBrand>()
        {
            catalogBrand,
        };

        IEnumerable<CatalogBrandDto> catalogBrandsDtoSuccess = new List<CatalogBrandDto>()
        {
            catalogBrandDto,
        };

        _catalogRepository.Setup(s => s.GetBrandsAsync()).ReturnsAsync(catalogBrandsSuccess);

        _mapper
            .Setup(
                s =>
                    s.Map<CatalogBrandDto>(
                        It.Is<CatalogBrand>(ct => ct.Equals(catalogBrand))))
            .Returns(catalogBrandDto);

        /*
        _mapper
            .Setup(
                s =>
                    s.Map<IEnumerable<CatalogBrandDto>?>(
                        It.Is<IEnumerable<CatalogBrand>?>(i => i!.Equals(catalogBrandsSuccess))))
            .Returns(catalogBrandsDtoSuccess);
        */

        // act
        var result = await _catalogService.GetBrandsAsync();

        // assert
        result?.Should().BeEquivalentTo(catalogBrandsDtoSuccess);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;
        var brandIdFilter = Array.Empty<int>();
        var typeIdFilter = Array.Empty<int>();
        PaginatedItems<CatalogItem?>? item = null!;

        string? exMessage = null;
        PaginatedItemsResponse<CatalogItemDto>? result = null;

        _catalogRepository
            .Setup(
                s =>
                    s.GetByPageAsync(
                        It.Is<int>(i => i == testPageSize),
                        It.Is<int>(i => i == testPageIndex),
                        It.Is<int[]>(i => i == brandIdFilter),
                        It.Is<int[]>(i => i == typeIdFilter)))
            .ReturnsAsync(item);

        // act
        try
        {
            result = await _catalogService.GetItemsByPageAsync(testPageSize, testPageIndex, brandIdFilter, typeIdFilter);
        }
        catch (BusinessException ex)
        {
            exMessage = ex.Message;
        }

        // assert
        result?.Should().BeNull();
        exMessage?.Should().Match("Catalog Items couldn't be fetched");
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;
        var brandIdFilter = new int[2];
        var typeIdFilter = new int[2];

        var catalogItemSuccess = new CatalogItem() { Id = 0, Name = "Product", };

        var catalogItemDtoSuccess = new CatalogItemDto() { Id = 0, Name = "Product", };

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem?>()
        {
            Data = new List<CatalogItem?>() { catalogItemSuccess, },
            TotalCount = testTotalCount,
        };

        _catalogRepository
            .Setup(
                s =>
                    s.GetByPageAsync(
                        It.Is<int>(i => i == testPageSize),
                        It.Is<int>(i => i == testPageIndex),
                        It.Is<int[]?>(i => i == brandIdFilter),
                        It.Is<int[]?>(i => i == typeIdFilter)))
            .ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper
            .Setup(
                s =>
                    s.Map<CatalogItemDto>(
                        It.Is<CatalogItem?>(i => i!.Equals(catalogItemSuccess))))
            .Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetItemsByPageAsync(testPageSize, testPageIndex, brandIdFilter, typeIdFilter);

        // assert
        result?.Should().NotBeNull();
        result?
            .Data.Should()
            .BeEquivalentTo(new List<CatalogItemDto>() { catalogItemDtoSuccess });
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetProductByIdAsync_Failed()
    {
        // arrange
        string? exMessage = null;
        CatalogItemDto? result = null;

        var catalogItemFailed = new CatalogItem()
        {
            Id = 1,
            Name = "Name",
            AvailableStock = 100,
            Price = 1000,
            Warranty = 12,
            Description = "Description",
        };

        _catalogRepository
            .Setup(s => s.GetByIdAsync(It.Is<int>(i => i == catalogItemFailed.Id)))
            .ReturnsAsync((Func<CatalogItem?>)null!);

        // act
        try
        {
            result = await _catalogService.GetItemByIdAsync(catalogItemFailed.Id);
        }
        catch (BusinessException ex)
        {
            exMessage = ex.Message;
        }

        // assert
        result?.Should().BeNull();
        exMessage?.Should().Match($"Catalog Item with id {catalogItemFailed.Id} couldn't be fetched");
    }

    [Fact]
    public async Task GetProductByIdAsync_Success()
    {
        // arrange
        var catalogItemSuccess = new CatalogItem()
        {
            Id = 1,
            Name = "Name",
            AvailableStock = 100,
            Price = 1000,
            Warranty = 12,
            Description = "Description",
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Id = 1,
            Name = "Name",
            AvailableStock = 100,
            Price = 1000,
            Warranty = 12,
            Description = "Description",
        };

        _catalogRepository
            .Setup(s => s.GetByIdAsync(It.Is<int>(i => i == catalogItemSuccess.Id)))
            .ReturnsAsync(catalogItemSuccess);

        // act
        var result = await _catalogService.GetItemByIdAsync(catalogItemSuccess.Id);

        // assert
        result?.Should().BeSameAs(catalogItemDtoSuccess);
    }

    [Fact]
    public async Task GetProductsAsync_Failed()
    {
        // arrange
        string? exMessage = null;
        IEnumerable<CatalogItemDto>? result = null;

        _catalogRepository
            .Setup(s => s.GetAllItemsAsync())
            .ReturnsAsync((Func<IEnumerable<CatalogItem?>?>)null!);

        // act
        try
        {
            result = await _catalogService.GetAllItemsAsync();
        }
        catch (BusinessException ex)
        {
            exMessage = ex.Message;
        }

        // assert
        result?.Should().BeNull();
        exMessage?.Should().Match("Products couldn't be fetched");
    }

    [Fact]
    public async Task GetProductsAsync_Success()
    {
        // arrange

        var catalogItem = new CatalogItem()
        {
            Id = 1,
            Name = "Name",
            AvailableStock = 100,
            Price = 1000,
            Warranty = 12,
            Description = "Description",
        };

        var catalogItemDto = new CatalogItemDto()
        {
            Id = 1,
            Name = "Name",
            AvailableStock = 100,
            Price = 1000,
            Warranty = 12,
            Description = "Description",
        };

        IEnumerable<CatalogItem> catalogItemsSuccess = new List<CatalogItem>()
        {
            catalogItem,
        };

        IEnumerable<CatalogItemDto> catalogItemsDtoSuccess = new List<CatalogItemDto>()
        {
            catalogItemDto,
        };

        _catalogRepository
            .Setup(s => s.GetAllItemsAsync())
            .ReturnsAsync(catalogItemsSuccess);

        _mapper
            .Setup(
                s =>
                s.Map<CatalogItemDto>(
                        It.Is<CatalogItem>(ct => ct.Equals(catalogItem))))
            .Returns(catalogItemDto);

        /*
        _mapper
            .Setup(
                s =>
                    s.Map<IEnumerable<CatalogItemDto>?>(
                        It.Is<IEnumerable<CatalogItem>?>(i => i!.Equals(catalogItemsSuccess))))
            .Returns(catalogItemsDtoSuccess);
        */

        // act
        var result = await _catalogService.GetAllItemsAsync();

        // assert
        result?.Should().BeEquivalentTo(catalogItemsDtoSuccess);
    }

    [Fact]
    public async Task GetTypesAsync_Failed()
    {
        // arrange
        string? exMessage = null;
        IEnumerable<CatalogTypeDto>? result = null;

        _catalogRepository
            .Setup(s => s.GetTypesAsync())
            .ReturnsAsync((Func<IEnumerable<CatalogType?>?>)null!);

        // act
        try
        {
            result = await _catalogService.GetTypesAsync();
        }
        catch (BusinessException ex)
        {
            exMessage = ex.Message;
        }

        // assert
        result?.Should().BeNull();
        exMessage?.Should().Match("Types couldn't be fetched");
    }

    [Fact]
    public async Task GetTypesAsync_Success()
    {
        // arrange

        var catalogType = new CatalogType() { Id = 1, Name = "Type", };

        var catalogTypeDto = new CatalogTypeDto() { Id = 1, Name = "Type", };

        IEnumerable<CatalogType> catalogTypesSuccess = new List<CatalogType>()
        {
            catalogType,
        };

        IEnumerable<CatalogTypeDto> catalogTypesDtoSuccess = new List<CatalogTypeDto>()
        {
            catalogTypeDto,
        };

        _catalogRepository.Setup(s => s.GetTypesAsync()).ReturnsAsync(catalogTypesSuccess);

        _mapper
            .Setup(
                s =>
                    s.Map<CatalogTypeDto>(
                        It.Is<CatalogType>(ct => ct.Equals(catalogType))))
            .Returns(catalogTypeDto);

        /*
        _mapper
            .Setup(
                s =>
                    s.Map<IEnumerable<CatalogTypeDto>?>(
                        It.Is<IEnumerable<CatalogType>?>(i => i!.Equals(catalogTypesSuccess))))
            .Returns(catalogTypesDtoSuccess);
        */

        // act
        var result = await _catalogService.GetTypesAsync();

        // assert
        result?.Should().BeEquivalentTo(catalogTypesDtoSuccess);
    }
}
