using AnalyticsService.Consumers.Notification;
using MassTransit;
using Trumpee.MassTransit;
using Trumpee.MassTransit.Messages.Analytics.Notifications.Payloads;

namespace AnalyticsService.Extensions;

public static class NotificationAnalyticsConsumersExtensions
{
    public static void AddNotificationCreatedEndpoint(
        this  IRabbitMqBusFactoryConfigurator configurator,
        IBusRegistrationContext ctx)
    {
        var queueName = QueueNames.Analytics.Users(typeof(NotificationCreatedPayload))
            .Replace("queue:", string.Empty);
        configurator.ReceiveEndpoint(queueName, e =>
        {
            e.BindQueue = true;
            e.PrefetchCount = 4;

            e.UseConcurrencyLimit(1);
            e.ConfigureConsumer<NotificationCreatedEventsConsumer>(ctx);
        });
    }
}
