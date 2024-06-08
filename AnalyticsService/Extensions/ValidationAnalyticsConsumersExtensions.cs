using AnalyticsService.Consumers.Notification;
using AnalyticsService.Consumers.Template;
using AnalyticsService.Consumers.Validation;
using MassTransit;
using Trumpee.MassTransit;
using Trumpee.MassTransit.Messages.Analytics.Notifications.Payloads;
using Trumpee.MassTransit.Messages.Analytics.Template.Payloads;
using Trumpee.MassTransit.Messages.Analytics.Validation.Payloads;

namespace AnalyticsService.Extensions;

public static class ValidationAnalyticsConsumersExtensions
{
    public static void AddValidationPassedEndpoint(
        this  IRabbitMqBusFactoryConfigurator configurator,
        IBusRegistrationContext ctx)
    {
        var queueName = QueueNames.Analytics.Users(typeof(ValidationPassedPayload))
            .Replace("queue:", string.Empty);
        configurator.ReceiveEndpoint(queueName, e =>
        {
            e.BindQueue = true;
            e.PrefetchCount = 4;

            e.UseConcurrencyLimit(1);
            e.ConfigureConsumer<ValidationPassedEventsConsumer>(ctx);
        });
    }

    public static void AddValidationFailedEndpoint(
        this  IRabbitMqBusFactoryConfigurator configurator,
        IBusRegistrationContext ctx)
    {
        var queueName = QueueNames.Analytics.Users(typeof(ValidationFailedPayload))
            .Replace("queue:", string.Empty);
        configurator.ReceiveEndpoint(queueName, e =>
        {
            e.BindQueue = true;
            e.PrefetchCount = 4;

            e.UseConcurrencyLimit(1);
            e.ConfigureConsumer<ValidationFailedEventsConsumer>(ctx);
        });
    }
}
