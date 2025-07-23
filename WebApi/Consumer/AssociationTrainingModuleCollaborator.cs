using Application.Services;
using MassTransit;
using Domain.Messages;

public class AssociationTrainingModuleCollaboratorCreatedConsumer : IConsumer<AssociationTrainingModuleCollaboratorMessage>
{
    private readonly AssociationTrainingModuleCollaboratorService _service;

    public AssociationTrainingModuleCollaboratorCreatedConsumer(AssociationTrainingModuleCollaboratorService service)
    {
        _service = service;
    }

    public async Task Consume(ConsumeContext<AssociationTrainingModuleCollaboratorMessage> context)
    {
        var msg = context.Message;
        await _service.SubmitAsync(msg.CollaboratorId, msg.TrainingModuleId);
    }
}
