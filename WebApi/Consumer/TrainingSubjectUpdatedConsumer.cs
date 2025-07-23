using Application.Services;
using MassTransit;
using Domain.Messages;

public class TrainingSubjectUpdatedConsumer : IConsumer<TrainingSubjectUpdatedMessage>
{
    private readonly TrainingSubjectService _trainingSubjectService;

    public TrainingSubjectUpdatedConsumer(TrainingSubjectService trainingSubjectService)
    {
        _trainingSubjectService = trainingSubjectService;
    }
    public async Task Consume(ConsumeContext<TrainingSubjectUpdatedMessage> context)
    {
        var senderId = context.Headers.Get<string>("SenderId");
        if (senderId == InstanceInfo.InstanceId)
            return;
        var msg = context.Message;
        await _trainingSubjectService.SubmitUpdateAsync(msg.Id, msg.Subject, msg.Description);
    }
}