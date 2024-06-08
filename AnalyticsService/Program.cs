using AnalyticsService.Consumers.Notification;
using AnalyticsService.Consumers.Template;
using AnalyticsService.Consumers.User;
using AnalyticsService.Consumers.Validation;
using AnalyticsService.Extensions;
using AnalyticsService.Infrastructure.Persistence.Mongo.Abstractions;
using AnalyticsService.Infrastructure.Persistence.Mongo.Configurations;
using AnalyticsService.Infrastructure.Persistence.Mongo.Repositories;
using AnalyticsService.Infrastructure.Persistence.Mongo.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Serilog;
using Serilog.Events;
using Trumpee.MassTransit;
using Trumpee.MassTransit.Configuration;
using Trumpee.MassTransit.Messages.Analytics.Users.Payloads;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("MassTransit", LogEventLevel.Debug)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var host = CreateHostBuilder(args).Build();

Console.WriteLine("Done!");
await host.RunAsync();
return;

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureHostConfiguration(config =>
        {
            config.AddEnvironmentVariables();
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
        })
        .UseSerilog()
        .ConfigureServices((host, services) =>
        {
            AddMongoDb(services, host.Configuration);

            var rabbitTopologyBuilder = new RabbitMqTransportConfigurator();
            rabbitTopologyBuilder.AddExternalConfigurations(x =>
            {
                x.AddConsumer<UserSignInEventsConsumer>();
                x.AddConsumer<UserSignUpEventsConsumer>();
                x.AddConsumer<UserActionEventsConsumer>();

                x.AddConsumer<NotificationCreatedEventsConsumer>();

                x.AddConsumer<TemplateFilledEventsConsumer>();
                x.AddConsumer<TemplateNotFilledEventsConsumer>();

                x.AddConsumer<ValidationPassedEventsConsumer>();
                x.AddConsumer<ValidationFailedEventsConsumer>();
            });

            rabbitTopologyBuilder.UseExternalConfigurations((ctx, cfg) =>
            {
                cfg.AddUserSignInEndpoint(ctx);
                cfg.AddUserSignUpEndpoint(ctx);
                cfg.AddUserActionEventsEndpoint(ctx);

                cfg.AddNotificationCreatedEndpoint(ctx);

                cfg.AddTemplateFilledEndpoint(ctx);
                cfg.AddTemplateNotFilledEndpoint(ctx);

                cfg.AddValidationPassedEndpoint(ctx);
                cfg.AddValidationFailedEndpoint(ctx);
            });

            services.AddConfiguredMassTransit(host.Configuration, rabbitTopologyBuilder);
        });
}

static void AddMongoDb(IServiceCollection services, IConfiguration config)
{
    services.AddScoped<IAnalyticsEventsRepository, AnalyticsEventsRepository>();

    services.Configure<MongoDbOptions>(config.GetSection(MongoDbOptions.ConfigurationSectionName));

    BsonSerializer.RegisterSerializer(new CustomDateTimeOffsetSerializer());
}
