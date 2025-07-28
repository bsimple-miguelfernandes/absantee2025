using Application.Services;
using MassTransit;
using Domain.Messages;

public class TrainingSubjectCreatedConsumer : IConsumer<TrainingSubjectMessage>
{
    private readonly TrainingSubjectService _trainingSubjectService;
    private static readonly string InstanceId = InstanceInfo.InstanceId;

    public TrainingSubjectCreatedConsumer(TrainingSubjectService trainingSubjectService)
    {
        _trainingSubjectService = trainingSubjectService;
    }
    public async Task Consume(ConsumeContext<TrainingSubjectMessage> context)
    {
        Console.WriteLine(">>> Mensagem recebida no consumer");

        var senderId = context.Headers.Get<string>("SenderId");
        if (senderId == InstanceInfo.InstanceId)
            return;
        var msg = context.Message;
        await _trainingSubjectService.SubmitAsync(msg.Id, msg.Subject, msg.Description);
    }
}