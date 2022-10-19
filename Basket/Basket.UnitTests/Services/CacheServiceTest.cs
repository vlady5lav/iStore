using Basket.Host.Configurations;
using Basket.Host.Services;
using Basket.Host.Services.Interfaces;

namespace Basket.UnitTests.Services;

public class CacheServiceTest
{
    private readonly ICacheService _cacheService;

    private readonly Mock<IOptionsMonitor<RedisConfig>> _config;

    private readonly Mock<IConnectionMultiplexer> _connectionMultiplexer;

    private readonly Mock<IJsonSerializer> _jsonSerializer;

    private readonly Mock<ILogger<CacheService>> _logger;

    private readonly Mock<IRedisCacheConnectionService> _redisCacheConnectionService;

    private readonly Mock<IDatabase> _redisDataBase;

    public CacheServiceTest()
    {
        _config = new Mock<IOptionsMonitor<RedisConfig>>();
        _logger = new Mock<ILogger<CacheService>>();

        _config.Setup(x => x.CurrentValue).Returns(new RedisConfig() { CacheTimeout = TimeSpan.Zero });

        _redisCacheConnectionService = new Mock<IRedisCacheConnectionService>();
        _connectionMultiplexer = new Mock<IConnectionMultiplexer>();
        _redisDataBase = new Mock<IDatabase>();

        _redisCacheConnectionService
            .Setup(x => x.Connection)
            .Returns(_connectionMultiplexer.Object);

        _connectionMultiplexer
            .Setup(x => x.GetDatabase(
                It.IsAny<int>(),
                It.IsAny<object>()))
            .Returns(_redisDataBase.Object);

        _jsonSerializer = new Mock<IJsonSerializer>();

        _cacheService =
            new CacheService(
                _logger.Object,
                _redisCacheConnectionService.Object,
                _config.Object,
                _jsonSerializer.Object);
    }

    [Fact]
    public async Task AddOrUpdateAsync_Add_Success()
    {
        // arrange
        var testEntity = new
        {
            UserId = "id",
            Data = "data",
        };

        _redisDataBase.Setup(x => x.KeyExistsAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<CommandFlags>()))
                .ReturnsAsync(false);

        _redisDataBase.Setup(x => x.StringSetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<bool>(),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        // act
        await _cacheService.AddOrUpdateAsync(testEntity.UserId, testEntity.Data);

        // assert
        _logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!
                    .Contains($"Value for the key \"{testEntity.UserId}\" is cached!")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    [Fact]
    public async Task AddOrUpdateAsync_Update_Success()
    {
        // arrange
        var testEntity = new
        {
            UserId = "id",
            Data = "data",
        };

        _redisDataBase.Setup(x => x.KeyExistsAsync(
        It.IsAny<RedisKey>(),
        It.IsAny<CommandFlags>()))
        .ReturnsAsync(true);

        _redisDataBase.Setup(x => x.StringSetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<bool>(),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        // act
        await _cacheService.AddOrUpdateAsync(testEntity.UserId, testEntity.Data);

        // assert
        _logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!
                    .Contains($"Cached value for the key \"{testEntity.UserId}\" is updated!")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    [Fact]
    public async Task AddOrUpdateAsync_Failed()
    {
        // arrange
        var testEntity = new
        {
            UserId = "id",
            Data = "data",
        };

        _redisDataBase.Setup(x => x.KeyExistsAsync(
        It.IsAny<RedisKey>(),
        It.IsAny<CommandFlags>()))
        .ReturnsAsync(false);

        _redisDataBase.Setup(x => x.StringSetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<bool>(),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(false);

        // act
        await _cacheService.AddOrUpdateAsync(testEntity.UserId, testEntity.Data);

        // assert
        _logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!
                    .Contains($"Value for the key \"{testEntity.UserId}\" cannot be cached!")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    [Fact]
    public async Task GetAsync_Failed()
    {
        // arrange
        var testEntity = new
        {
            UserId = "id",
            Data = "data",
        };

        _jsonSerializer.Setup(x => x.Deserialize<string>(It.IsAny<string>())).Returns(testEntity.Data);

        _redisDataBase.Setup(x => x.StringGetAsync(
                It.Is<RedisKey>(rk => rk.Equals(testEntity.UserId)),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(string.Empty);

        // act
        var result = await _cacheService.GetAsync<string>(testEntity.UserId);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAsync_Success()
    {
        // arrange
        var testEntity = new
        {
            UserId = "id",
            Data = "data",
        };

        _jsonSerializer.Setup(x => x.Deserialize<string>(It.IsAny<string>())).Returns(testEntity.Data);

        _redisDataBase.Setup(x => x.StringGetAsync(
                It.Is<RedisKey>(rk => rk.Equals(testEntity.UserId)),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(testEntity.Data);

        // act
        var result = await _cacheService.GetAsync<string>(testEntity.UserId);

        // assert
        result.Should().Be(testEntity.Data);
    }

    [Fact]
    public async Task DeleteAsync_Failed()
    {
        // arrange
        var testEntity = new
        {
            UserId = "id",
            Data = "data",
        };

        _jsonSerializer.Setup(x => x.Deserialize<string>(It.IsAny<string>())).Returns(testEntity.Data);

        _redisDataBase.Setup(x => x.StringSetAsync(
        It.Is<RedisKey>(x => x.Equals(testEntity.UserId)),
        It.IsAny<RedisValue>(),
        It.IsAny<TimeSpan?>(),
        It.IsAny<bool>(),
        It.IsAny<When>(),
        It.IsAny<CommandFlags>()))
        .ReturnsAsync(false);

        _redisDataBase.Setup(x => x.StringGetAsync(
        It.Is<RedisKey>(x => x.Equals(testEntity.UserId)),
        It.IsAny<CommandFlags>()))
        .ReturnsAsync(testEntity.Data);

        // act
        await _cacheService.DeleteAsync(testEntity.UserId);

        var result = await _cacheService.GetAsync<string>(testEntity.UserId);

        // assert
        result.Should().Be(testEntity.Data);
    }

    [Fact]
    public async Task DeleteAsync_Success()
    {
        // arrange
        var testEntity = new
        {
            UserId = "id",
            Data = "data",
        };

        _jsonSerializer.Setup(x => x.Deserialize<string>(It.IsAny<string>())).Returns(testEntity.Data);

        _redisDataBase.Setup(x => x.StringSetAsync(
        It.Is<RedisKey>(x => x.Equals(testEntity.UserId)),
        It.IsAny<RedisValue>(),
        It.IsAny<TimeSpan?>(),
        It.IsAny<bool>(),
        It.IsAny<When>(),
        It.IsAny<CommandFlags>()))
        .ReturnsAsync(true);

        _redisDataBase.Setup(x => x.StringGetAsync(
        It.Is<RedisKey>(x => x.Equals(testEntity.UserId)),
        It.IsAny<CommandFlags>()))
        .ReturnsAsync(string.Empty);

        // act
        await _cacheService.DeleteAsync(testEntity.UserId);

        var result = await _cacheService.GetAsync<string>(testEntity.UserId);

        // assert
        result.Should().BeNull();
    }
}
