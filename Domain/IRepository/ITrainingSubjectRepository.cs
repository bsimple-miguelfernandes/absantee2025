
using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.IRepository;

public interface ITrainingSubjectRepository : IGenericRepositoryEF<ITrainingSubject, TrainingSubject, ITrainingSubjectVisitor>
{
    Task<bool> IsDuplicated(string subject);
    Task<ITrainingSubject> UpdateAsync(ITrainingSubject trainingSubject);
}
