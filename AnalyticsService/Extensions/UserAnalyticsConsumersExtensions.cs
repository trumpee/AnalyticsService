using AnalyticsService.Consumers.User;
using MassTransit;
using Trumpee.MassTransit;
using Trumpee.MassTransit.Messages.Analytics.Users.Payloads;

namespace AnalyticsService.Extensions;

public static class UserAnalyticsConsumersExtensions
{
    public static void AddUserSignInEndpoint(
        this  IRabbitMqBusFactoryConfigurator configurator,
        IBusRegistrationContext ctx)
    {
        var queueName = QueueNames.Analytics.Users(typeof(UserSignInPayload))
            .Replace("queue:", string.Empty);
        configurator.ReceiveEndpoint(queueName, e =>
        {
            e.BindQueue = true;
            e.PrefetchCount = 4;

            e.UseConcurrencyLimit(1);
            e.ConfigureConsumer<UserSignInEventsConsumer>(ctx);
        });
    }

    public static void AddUserSignUpEndpoint(
        this  IRabbitMqBusFactoryConfigurator configurator,
        IBusRegistrationContext ctx)
    {
        var queueName = QueueNames.Analytics.Users(typeof(UserSignUpPayload))
            .Replace("queue:", string.Empty);

        configurator.ReceiveEndpoint(queueName, e =>
        {
            e.BindQueue = true;
            e.PrefetchCount = 4;

            e.UseConcurrencyLimit(1);
            e.ConfigureConsumer<UserSignUpEventsConsumer>(ctx);
        });
    }

    public static void AddUserActionEventsEndpoint(
        this  IRabbitMqBusFactoryConfigurator configurator,
        IBusRegistrationContext ctx)
    {
        var queueName = QueueNames.Analytics.Users(typeof(UserActionPayload))
            .Replace("queue:", string.Empty);

        configurator.ReceiveEndpoint(queueName, e =>
        {
            e.BindQueue = true;
            e.PrefetchCount = 4;

            e.UseConcurrencyLimit(1);
            e.ConfigureConsumer<UserActionEventsConsumer>(ctx);
        });
    }
}
