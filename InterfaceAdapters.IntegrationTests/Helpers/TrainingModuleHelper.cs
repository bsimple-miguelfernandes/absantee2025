using Application.DTO.TrainingModule;
using Domain.Models;

namespace InterfaceAdapters.IntegrationTests.Helpers;

public static class TrainingModuleHelper
{
    public static AddTrainingModuleDTO GenerateAddTrainingModuleDTORandomDates(Guid trainingSubjectId)
    {
        var random = new Random();
        var periods = new List<PeriodDateTime>();

        int numberOfPeriods = random.Next(1, 6); // Generate between 1 and 5 periods
        DateTime currentStart = DateTime.UtcNow.Date.AddDays(1); // Start from tomorrow

        for (int i = 0; i < numberOfPeriods; i++)
        {
            // Random duration between 1 and 5 days
            int durationDays = random.Next(1, 6);
            DateTime end = currentStart.AddDays(durationDays - 1);

            periods.Add(new PeriodDateTime(currentStart, end));

            // Add buffer between periods to avoid overlap (1–3 days)
            currentStart = end.AddDays(random.Next(1, 4));
        }

        return new AddTrainingModuleDTO
        {
            TrainingSubjectId = trainingSubjectId,
            Periods = periods
        };
    }
}