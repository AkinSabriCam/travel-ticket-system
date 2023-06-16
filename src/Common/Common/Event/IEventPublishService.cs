namespace Common.Event;

public interface IEventPublishService
{
    Task Publish(string queueName, object payload);
}