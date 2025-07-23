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
        var msg = context.Message;
        await _trainingModuleService.SubmitAsync(msg.SubjectId, msg.Periods);
    }
}