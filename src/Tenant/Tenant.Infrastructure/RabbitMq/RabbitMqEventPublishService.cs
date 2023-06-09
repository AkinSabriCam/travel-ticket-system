﻿using System.Text;
using System.Text.Json;
using Common.Event;
using Common.User;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Tenant.Infrastructure.RabbitMq;

public class RabbitMqEventPublishService : IEventPublishService
{
    private readonly RabbitMqSettings _settings;
    private readonly IConnection _connection;
    private readonly IUser _user;
    private readonly ILogger<RabbitMqEventPublishService> _logger;

    public RabbitMqEventPublishService(IOptions<RabbitMqSettings> settings, IConnection connection, IUser user,
        ILogger<RabbitMqEventPublishService> logger)
    {
        _user = user;
        _logger = logger;
        _settings = settings.Value;
        _connection = connection;
    }

    public Task Publish(string queueName, object payload)
    {
        var bodyString = JsonSerializer.Serialize(payload);
        try
        {
            using (var rabbitMqChannel = _connection.CreateModel())
            {
                rabbitMqChannel.QueueDeclare(queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var body = Encoding.UTF8.GetBytes(bodyString);

                var properties = rabbitMqChannel.CreateBasicProperties();
                var headers = new Dictionary<string, object>()
                {
                    { "userId", _user.UserId.ToString() },
                    { "tenantId", _user.TenantId.ToString() },
                    { "tenantCode", _user.TenantCode },
                };
                properties.Headers = headers;

                rabbitMqChannel.BasicPublish(exchange: string.Empty,
                    routingKey: queueName,
                    basicProperties: properties,
                    body: body);
                
                rabbitMqChannel.Close();
                _connection.Close();
            }

            _logger.LogInformation(
                $"Message was sent to the queue successfully - The Queue :{queueName} - The Payload: {bodyString}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError(
                $"Could not publish the message - The Queue :{queueName} - Payload: {bodyString} - The Error : {e.Message}");
        }

        return Task.CompletedTask;
    }
}