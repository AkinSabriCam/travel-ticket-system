namespace Tenant.Domain.Expedition.Dtos;

public class UpdateExpeditionDto : CreateExpeditionDto
{
    public Guid Id { get; private set; }

    public UpdateExpeditionDto SetId(Guid id)
    {
        Id = id;
        return this;
    }
}