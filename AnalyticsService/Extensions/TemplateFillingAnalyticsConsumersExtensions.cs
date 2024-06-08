using AnalyticsService.Consumers.Notification;
using AnalyticsService.Consumers.Template;
using MassTransit;
using Trumpee.MassTransit;
using Trumpee.MassTransit.Messages.Analytics.Notifications.Payloads;
using Trumpee.MassTransit.Messages.Analytics.Template.Payloads;

namespace AnalyticsService.Extensions;

public static class TemplateFillingAnalyticsConsumersExtensions
{
    public static void AddTemplateFilledEndpoint(
        this  IRabbitMqBusFactoryConfigurator configurator,
        IBusRegistrationContext ctx)
    {
        var queueName = QueueNames.Analytics.Users(typeof(TemplateFilledPayload))
            .Replace("queue:", string.Empty);
        configurator.ReceiveEndpoint(queueName, e =>
        {
            e.BindQueue = true;
            e.PrefetchCount = 4;

            e.UseConcurrencyLimit(1);
            e.ConfigureConsumer<TemplateFilledEventsConsumer>(ctx);
        });
    }

    public static void AddTemplateNotFilledEndpoint(
        this  IRabbitMqBusFactoryConfigurator configurator,
        IBusRegistrationContext ctx)
    {
        var queueName = QueueNames.Analytics.Users(typeof(TemplateNotFilledPayload))
            .Replace("queue:", string.Empty);
        configurator.ReceiveEndpoint(queueName, e =>
        {
            e.BindQueue = true;
            e.PrefetchCount = 4;

            e.UseConcurrencyLimit(1);
            e.ConfigureConsumer<TemplateNotFilledEventsConsumer>(ctx);
        });
    }
}
