using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Responses;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogService : BaseDataService<ApplicationDbContext>, ICatalogService
{
    private readonly ICatalogRepository _catalogRepository;

    private readonly IMapper _mapper;

    public CatalogService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        IMapper mapper,
        ICatalogRepository catalogRepository)
        : base(dbContextWrapper, logger)
    {
        _catalogRepository = catalogRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CatalogBrandDto>?> GetBrandsAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogRepository.GetBrandsAsync();

            if (result == null || !result.Any())
            {
                throw new BusinessException("Brands couldn't be fetched");
            }

            var mapped = result.Select(cb => _mapper.Map<CatalogBrandDto>(cb)).ToList();

            return mapped;

            // another "uncorrect" variant to map
            //return _mapper.Map<IEnumerable<CatalogBrandDto>?>(result);
        });
    }

    public async Task<PaginatedItemsResponse<CatalogItemDto>?> GetItemsByPageAsync(
        int pageSize = 10,
        int pageIndex = 0,
        int[]? brandIdFilter = null,
        int[]? typeIdFilter = null)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogRepository.GetByPageAsync(
            pageSize,
            pageIndex,
            brandIdFilter,
            typeIdFilter);

            if (result == null || !result.Data.Any())
            {
                throw new BusinessException("Catalog Items couldn't be fetched");
            }

            return new PaginatedItemsResponse<CatalogItemDto>()
            {
                Count = result.TotalCount,
                Data = result.Data.Select(ci => _mapper.Map<CatalogItemDto>(ci)).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
        });
    }

    public async Task<CatalogItemDto?> GetItemByIdAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogRepository.GetByIdAsync(id);

            if (result == null)
            {
                throw new BusinessException($"Catalog Item with id {id} couldn't be fetched");
            }

            return _mapper.Map<CatalogItemDto>(result);
        });
    }

    public async Task<IEnumerable<CatalogItemDto>?> GetAllItemsAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogRepository.GetAllItemsAsync();

            if (result == null || !result.Any())
            {
                throw new BusinessException("Products couldn't be fetched");
            }

            var mapped = result.Select(ci => _mapper.Map<CatalogItemDto>(ci)).ToList();

            return mapped;

            // another "uncorrect" variant to map
            //return _mapper.Map<IEnumerable<CatalogItemDto>?>(result);
        });
    }

    public async Task<IEnumerable<CatalogTypeDto>?> GetTypesAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogRepository.GetTypesAsync();

            if (result == null || !result.Any())
            {
                throw new BusinessException("Types couldn't be fetched");
            }

            var mapped = result.Select(ct => _mapper.Map<CatalogTypeDto>(ct)).ToList();

            return mapped;

            // another "uncorrect" variant to map
            //return _mapper.Map<IEnumerable<CatalogTypeDto>?>(result);
        });
    }
}
