using Application.DTO;

namespace WebApi.IntegrationTests.Helpers;

public static class TrainingPeriodHelper
{
    public static CreateTrainingPeriodDTO GenerateCreateTrainingPeriodDTO()
    {
        var start = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
        var end = start.AddDays(3);

        return new CreateTrainingPeriodDTO
        {
            InitDate = start,
            FinalDate = end
        };
    }
}
