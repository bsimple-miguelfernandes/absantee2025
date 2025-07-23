using Application.Services;
using Domain.Messages;
using MassTransit;

public class TrainingPeriodCreatedConsumer : IConsumer<TrainingPeriodMessage>
{
    private readonly TrainingPeriodService _trainingPeriodService;

    public TrainingPeriodCreatedConsumer(TrainingPeriodService trainingPeriodService)
    {
        _trainingPeriodService = trainingPeriodService;
    }
    public async Task Consume(ConsumeContext<TrainingPeriodMessage> context)
    {
        var msg = context.Message;
        await _trainingPeriodService.SubmitAsync(msg.PeriodDate.InitDate, msg.PeriodDate.FinalDate);
    }
}