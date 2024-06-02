namespace AnalyticsService.Infrastructure.Persistence.Mongo.Abstractions;

public interface IMongoRepository<in TEntity>
{
    Task InsertOne(TEntity document);
    Task InsertMany(IEnumerable<TEntity> documents);
}
