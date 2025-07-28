using Application.Services;
using Domain.Messages;
using MassTransit;

public class TrainingModuleCreatedConsumer : IConsumer<TrainingModuleMessage>
{
    private readonly TrainingModuleService _trainingModuleService;

    public TrainingModuleCreatedConsumer(TrainingModuleService trainingModuleService)
    {
        _trainingModuleService = trainingModuleService;
    }
    public async Task Consume(ConsumeContext<TrainingModuleMessage> context)
    {
        var senderId = context.Headers.Get<string>("SenderId");
        if (senderId == InstanceInfo.InstanceId)
            return;
        var msg = context.Message;
        await _trainingModuleService.SubmitAsync(msg.Id, msg.SubjectId, msg.Periods);
    }
}