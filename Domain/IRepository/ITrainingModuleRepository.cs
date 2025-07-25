
using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.IRepository
{
    public interface ITrainingModuleRepository : IGenericRepositoryEF<ITrainingModule, TrainingModule, ITrainingModuleVisitor>
    {
        Task<IEnumerable<TrainingModule>> GetBySubjectIdAndFinished(Guid subjectId, DateTime period);
        public Task<bool> HasOverlappingPeriodsAsync(Guid trainingSubjectId, List<PeriodDateTime> newPeriods);
        Task<bool> ExistsAsync(Guid id);
        Task<IEnumerable<TrainingModule>> GetBySubjectAndAfterDateFinished(Guid subjectId, DateTime date);
        Task<ITrainingModule?> UpdateTrainingModule(TrainingModule trainingModule);

    }
}
