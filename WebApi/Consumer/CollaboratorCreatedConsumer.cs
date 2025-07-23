using Application.Services;
using MassTransit;
using Domain.Messages;

public class CollaboratorCreatedConsumer : IConsumer<CollaboratorMessage>
{
    private readonly CollaboratorService _collaboratorService;

    public CollaboratorCreatedConsumer(CollaboratorService collaboratorService)
    {
        _collaboratorService = collaboratorService;
    }

    public async Task Consume(ConsumeContext<CollaboratorMessage> context)
    {
        var msg = context.Message;
        await _collaboratorService.SubmitAsync(msg.Id, msg.UserId, msg.PeriodDateTime);
    }
}
