using Domain.Models;
using WebApi;
using MassTransit;
using Application.IPublisher;
using Domain.Messages;

public class MassTransitPublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
    }

    public async Task PublishCreatedTrainingModuleMessageAsync(Guid id, Guid subjectId, List<PeriodDateTime> periods)
    {
        var eventMessage = new TrainingModuleMessage(id, subjectId, periods);
        await _publishEndpoint.Publish(eventMessage,
         context =>
            {
                context.Headers.Set("SenderId", InstanceInfo.InstanceId);
            });
    }
    public async Task PublishUpdatedTrainingModuleMessageAsync(Guid id, Guid subjectId, List<PeriodDateTime> periods)
    {
        var eventMessage = new TrainingModuleUpdatedMessage(id, subjectId, periods);
        await _publishEndpoint.Publish(eventMessage,
         context =>
            {
                context.Headers.Set("SenderId", InstanceInfo.InstanceId);
            });
    }
    public async Task PublishCreatedTrainingSubjectMessageAsync(Guid id, String description, String subject)
    {
        var eventMessage = new TrainingSubjectMessage(id, description, subject);
        await _publishEndpoint.Publish(eventMessage);
    }

    public async Task PublishCreatedTrainingPeriodMessageAsync(Guid id, PeriodDate periodDate)
    {
        var eventMessage = new TrainingPeriodMessage(id, periodDate);
        await _publishEndpoint.Publish(eventMessage);
    }
    public async Task PublishUpdatedTrainingSubjectMessageAsync(Guid id, string description, string subject)
    {

        var eventMessage = new TrainingSubjectUpdatedMessage(id, description, subject);
        await _publishEndpoint.Publish(eventMessage);
        Console.WriteLine("ENtrou no updated puliser do subject");
    }


}