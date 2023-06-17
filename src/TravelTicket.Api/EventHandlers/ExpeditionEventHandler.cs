using System.Text;
using System.Text.Json;
using Common.User;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Tenant.Application.Commands.Expedition.UpdateExpedition.Notification;
using Tenant.Domain;
using Tenant.Domain.Expedition;
using Tenant.Domain.Ticket;
using Tenant.Infrastructure.RabbitMq;

namespace TravelTicket.Api.EventHandlers;

public class ExpeditionEventHandler : BackgroundService
{
    private readonly IModel _channel;
    private readonly ILogger<ExpeditionEventHandler> _logger;
    private const string QueueName = "expedition_queue";
    private readonly IServiceProvider _serviceProvider;

    public ExpeditionEventHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
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

        consumer.Received += async (_, @event) =>
        {
            try
            {
                var body = @event.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation(
                    $"The Message is received from queue. The Queue : {QueueName} - The Payload: {message}");
                var model = JsonSerializer.Deserialize<UpdatedExpeditionNotification>(message);

                var departureDate = model.Changes.First(x => x.FieldName.Equals(nameof(Expedition.DepartureDate)));
                var price = model.Changes.First(x => x.FieldName.Equals(nameof(Expedition.UnitPrice)));

                if (departureDate.NewValue.Equals(departureDate.OldValue) && price.NewValue.Equals(price.OldValue))
                    return;
            
                var userId = @event.BasicProperties.Headers.First(x => x.Key == "userId");
                var tenantId = @event.BasicProperties.Headers.First(x => x.Key == "tenantId");
                var tenantCode = @event.BasicProperties.Headers.First(x => x.Key == "tenantCode");

                await LocalUserContext.SetUser(new LocalUser()
                {
                    UserId = Guid.Parse(Encoding.UTF8.GetString((byte[]) userId.Value)),
                    TenantId = Guid.Parse(Encoding.UTF8.GetString((byte[]) tenantId.Value)),
                    TenantCode = tenantCode.Value.ToString()
                });

                await using var scope = _serviceProvider.CreateAsyncScope();
                var ticketDomainService = scope.ServiceProvider.GetRequiredService<ITicketDomainService>();
                var tenantUnitOfWork = scope.ServiceProvider.GetRequiredService<ITenantUnitOfWork>();
                await ticketDomainService.UpdateByExpeditionId(model.Id, model.Changes);
                await tenantUnitOfWork.SaveChangesAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            // we will send information to passengers who bought ticket by mail
        };

        _channel.BasicConsume(queue: QueueName,
            autoAck: true,
            consumer: consumer);

        await Task.CompletedTask;
    }
}