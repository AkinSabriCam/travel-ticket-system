using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Tenant.Infrastructure.RabbitMq;

namespace TravelTicket.Api.EventHandlers;

public class ExpeditionEventHandler : BackgroundService
{
    private readonly IModel _channel;
    private readonly ILogger<ExpeditionEventHandler> _logger;
    const string QueueName = "expedition_queue";

    public ExpeditionEventHandler(IServiceProvider serviceProvider)
    {
        var settings = serviceProvider.GetRequiredService<IOptions<RabbitMqSettings>>();
        _logger = serviceProvider.GetRequiredService<ILogger<ExpeditionEventHandler>>();

        var factory = new ConnectionFactory
        {
            HostName = settings.Value.Host,
            Port = settings.Value.Port,
            UserName = settings.Value.Username,
            Password = settings.Value.Password,
            AutomaticRecoveryEnabled = true,
        };

        _channel = factory.CreateConnection().CreateModel();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // here is a sample for scheduled process 
        // while (!stoppingToken.IsCancellationRequested)
        // {
        //     await Task.Delay(10000, stoppingToken);
        // }

        var consumer = new EventingBasicConsumer(_channel);
        _channel.QueueDeclare(queue: QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        consumer.Received += (_, @event) =>
        {
            var body = @event.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation(
                $"The Message is received from queue. The Queue : {QueueName} - The Payload: {message}");
            consumer.HandleBasicConsumeOk("received");
        };

        _channel.BasicConsume(queue: QueueName,
            autoAck: true,
            consumer: consumer);

        await Task.CompletedTask;
    }
}