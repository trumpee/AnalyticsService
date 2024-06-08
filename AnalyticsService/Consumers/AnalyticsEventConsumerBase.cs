using System.Text.Json;
using AnalyticsService.Infrastructure.Persistence.Mongo.Abstractions;
using AnalyticsService.Infrastructure.Persistence.Mongo.Entities;
using MassTransit;
using Trumpee.MassTransit.Messages.Analytics;

namespace AnalyticsService.Consumers;

public class AnalyticsEventConsumerBase<TPayload>(
    IAnalyticsEventsRepository repository,
    ILogger<AnalyticsEventConsumerBase<TPayload>> logger)
    : IConsumer<TPayload> where TPayload : AnalyticsEvent<TPayload>
{
    public async Task Consume(ConsumeContext<TPayload> context)
    {
        var e = context.Message;

        try
        {
            var entity = new AnalyticsEvent
            {
                NotificationId = e.Id,
                StreamId = e.StreamId,
                Action = e.Action,
                Source = e.Source,
                Version = e.Version,
                Timestamp = e.Timestamp,
                Metadata = e.Metadata,
                Payload = JsonSerializer.Serialize(e.Payload)
            };

            await repository.InsertOne(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while processing analytics event ({TPayload})", typeof(TPayload).Name);
            throw;
        }
    }
}
