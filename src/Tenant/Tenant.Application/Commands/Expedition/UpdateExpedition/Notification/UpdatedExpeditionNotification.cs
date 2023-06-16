using Common.Entity;
using MediatR;

namespace Tenant.Application.Commands.Expedition.UpdateExpedition.Notification;

public class UpdatedExpeditionNotification : INotification
{
   public Guid Id { get; set; }
   public NotificationType Type { get; set; }
   public List<Field> Changes { get; set; }
}

public enum NotificationType
{
   Updated,
   Deleted
}