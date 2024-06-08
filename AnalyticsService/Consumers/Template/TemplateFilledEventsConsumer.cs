using System.Text.Json;
using AnalyticsService.Infrastructure.Persistence.Mongo.Abstractions;
using AnalyticsService.Infrastructure.Persistence.Mongo.Entities;
using MassTransit;
using Trumpee.MassTransit.Messages.Analytics;
using Trumpee.MassTransit.Messages.Analytics.Template.Payloads;

namespace AnalyticsService.Consumers.Template;

public class TemplateFilledEventsConsumer(
    IAnalyticsEventsRepository repository,
    ILogger<TemplateFilledEventsConsumer> logger
) : IConsumer<AnalyticsEvent<TemplateFilledPayload>>
{
    public async Task Consume(ConsumeContext<AnalyticsEvent<TemplateFilledPayload>> context)
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
            logger.LogError(ex, "Error while processing template filled event");
            throw;
        }
    }
}
