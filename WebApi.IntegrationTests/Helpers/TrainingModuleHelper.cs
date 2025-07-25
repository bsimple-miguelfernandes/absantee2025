/* using Application.DTO.TrainingModule;
using Domain.Models;

namespace WebApi.IntegrationTests.Helpers
{
    public static class TrainingModuleHelper
    {
        public static AddTrainingModuleDTO GenerateAddTrainingModuleDTORandomDates(Guid trainingSubjectId)
        {
            var random = new Random();
            var periods = new List<PeriodDateTime>();

            int numberOfPeriods = random.Next(1, 6); // Entre 1 e 5 períodos
            DateTime currentStart = DateTime.UtcNow.Date.AddDays(1); // Começa a partir de amanhã

            for (int i = 0; i < numberOfPeriods; i++)
            {
                int durationDays = random.Next(1, 6); // duração entre 1 e 5 dias
                DateTime end = currentStart.AddDays(durationDays - 1);

                periods.Add(new PeriodDateTime(currentStart, end));

                currentStart = end.AddDays(random.Next(1, 4)); // intervalo de 1 a 3 dias
            }

            return new AddTrainingModuleDTO(trainingSubjectId, periods);
        }
    }
}
 */