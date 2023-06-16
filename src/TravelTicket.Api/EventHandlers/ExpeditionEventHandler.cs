using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Tenant.Infrastructure.RabbitMq;

namespace TravelTicket.Api.EventHandlers;

public class ExpeditionEventHandler : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public ExpeditionEventHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const string queueName = "expedition_queue";
        var settings = _serviceProvider.GetRequiredService<IOptions<RabbitMqSettings>>();
        var logger = _serviceProvider.GetRequiredService<ILogger<ExpeditionEventHandler>>();

        var factory = new ConnectionFactory
        {
            HostName = settings.Value.Host,
            Port = settings.Value.Port,
            UserName = settings.Value.Username,
            Password = settings.Value.Password,
            AutomaticRecoveryEnabled = true,
        };
        using var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            logger.LogInformation(
                $"The Message is received from queue. The Queue : {queueName} - The Payload: {message}");
        };
        channel.BasicConsume(queue: queueName,
            autoAck: true,
            consumer: consumer);


        return Task.CompletedTask;
    }
}