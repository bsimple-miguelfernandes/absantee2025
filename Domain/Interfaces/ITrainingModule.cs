using Domain.Models;

namespace Domain.Interfaces
{
    public interface ITrainingModule
    {
        Guid Id { get; }
        Guid TrainingSubjectId { get; }
        List<PeriodDateTime> Periods { get; }
        public void UpdateTrainingSubjectId(Guid newSubjectId);
        public void UpdatePeriods(List<PeriodDateTime> newPeriods);
    }
}
