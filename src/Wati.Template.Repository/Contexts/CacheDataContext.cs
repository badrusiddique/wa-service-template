using EnsureThat;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Wati.Template.Repository.Repositories.Interfaces;

namespace Wati.Template.Repository.Repositories;

public class CacheDataContext : ICacheDataContext
{
    private readonly IRedisClient _redisClient;

    public CacheDataContext(IRedisClient redisClient)
    {
        _redisClient = redisClient;
    }

    public async ValueTask<T> GetByKeyAsync<T>(string key) where T : class
    {
        EnsureArg.IsNotNullOrEmpty(key, nameof(key), o => o.WithMessage($"invalid key: {key}"));

        try
        {
            var database = _redisClient.GetDefaultDatabase();
            return await database.GetAsync<T>(key);
        }
        catch (Exception ex)
        {
            throw new RedisException($"An error occurred while trying to retrieve data from Redis: {ex.Message}", ex);
        }
    }
}