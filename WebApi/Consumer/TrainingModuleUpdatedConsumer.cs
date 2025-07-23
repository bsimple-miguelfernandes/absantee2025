using Application.Services;
using Domain.Messages;
using MassTransit;

public class TrainingModuleUpdatedConsumer : IConsumer<TrainingModuleUpdatedMessage>
{
    private readonly TrainingModuleService _trainingModuleService;

    public TrainingModuleUpdatedConsumer(TrainingModuleService trainingModuleService)
    {
        _trainingModuleService = trainingModuleService;
    }
    public async Task Consume(ConsumeContext<TrainingModuleUpdatedMessage> context)
    {
        var senderId = context.Headers.Get<string>("SenderId");
        if (senderId == InstanceInfo.InstanceId)
            return;
        var msg = context.Message;
        await _trainingModuleService.SubmitUpdateAsync(msg.Id, msg.SubjectId, msg.Periods);
    }
}