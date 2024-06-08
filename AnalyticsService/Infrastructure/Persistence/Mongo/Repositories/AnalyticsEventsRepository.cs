using AnalyticsService.Infrastructure.Persistence.Mongo.Abstractions;
using AnalyticsService.Infrastructure.Persistence.Mongo.Configurations;
using AnalyticsService.Infrastructure.Persistence.Mongo.Entities;
using Microsoft.Extensions.Options;

namespace AnalyticsService.Infrastructure.Persistence.Mongo.Repositories;

internal class AnalyticsEventsRepository(IOptions<MongoDbOptions> settings)
    : MongoDbRepositoryBase<AnalyticsEvent>(settings), IAnalyticsEventsRepository;
