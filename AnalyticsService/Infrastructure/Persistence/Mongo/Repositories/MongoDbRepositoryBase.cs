using AnalyticsService.Infrastructure.Persistence.Mongo.Abstractions;
using AnalyticsService.Infrastructure.Persistence.Mongo.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AnalyticsService.Infrastructure.Persistence.Mongo.Repositories;

internal abstract class MongoDbRepositoryBase<T> : IMongoRepository<T>
    where T : Entities.MongoBaseEntity
{
    private readonly IMongoCollection<T> _collection;

    protected MongoDbRepositoryBase(
        IOptions<MongoDbOptions> settings)
    {
        var settingValue = settings.Value;
        var database = new MongoClient(settingValue.ConnectionString).GetDatabase(settingValue.DatabaseName);
        _collection = database.GetCollection<T>(typeof(T).Name);
    }

    public Task InsertOne(T document)
        => _collection.InsertOneAsync(document);

    public Task InsertMany(IEnumerable<T> documents)
        => _collection.InsertManyAsync(documents);
}
