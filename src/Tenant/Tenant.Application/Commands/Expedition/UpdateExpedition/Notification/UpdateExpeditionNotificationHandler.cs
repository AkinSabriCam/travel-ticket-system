using Common.Event;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Tenant.Application.Commands.Expedition.UpdateExpedition.Notification;

public class UpdateExpeditionNotificationHandler : INotificationHandler<UpdatedExpeditionNotification>
{
    private readonly IEventPublishService _eventPublishService;
    private readonly ILogger<UpdateExpeditionNotificationHandler> _logger;

    public UpdateExpeditionNotificationHandler(IEventPublishService eventPublishService,
        ILogger<UpdateExpeditionNotificationHandler> logger)
    {
        _eventPublishService = eventPublishService;
        _logger = logger;
    }

    public Task Handle(UpdatedExpeditionNotification notification, CancellationToken cancellationToken)
    {
        return _eventPublishService.Publish("expedition_queue", notification);
    }
}