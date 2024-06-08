using AnalyticsService.Infrastructure.Persistence.Mongo.Entities;

namespace AnalyticsService.Infrastructure.Persistence.Mongo.Abstractions;

public interface IAnalyticsEventsRepository : IMongoRepository<AnalyticsEvent>;
