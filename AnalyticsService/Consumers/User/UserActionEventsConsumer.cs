using System.Text.Json;
using AnalyticsService.Infrastructure.Persistence.Mongo.Abstractions;
using AnalyticsService.Infrastructure.Persistence.Mongo.Entities;
using MassTransit;
using Trumpee.MassTransit.Messages.Analytics.Users.Payloads;

namespace AnalyticsService.Consumers.User;

public class UserActionEventsConsumer(
    IAnalyticsEventsRepository repository,
    ILogger<UserActionEventsConsumer> logger) : IConsumer<Trumpee.MassTransit.Messages.Analytics.AnalyticsEvent<UserActionPayload>>
{
    public async Task Consume(ConsumeContext<Trumpee.MassTransit.Messages.Analytics.AnalyticsEvent<UserActionPayload>> context)
    {
        try
        {
            var e = context.Message;
            var payload = context.Message.Payload!;
            logger.LogDebug($"User with email {payload.Email} took an action: {payload.Action}");

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
        catch (Exception e)
        {
            logger.LogError(e, "Error while processing user sign in event");
            throw;
        }
    }
}
