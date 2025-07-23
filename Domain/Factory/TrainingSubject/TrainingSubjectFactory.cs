using Domain.IRepository;
using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory;

public class TrainingSubjectFactory : ITrainingSubjectFactory
{

    public TrainingSubjectFactory()
    {
    }

    public async Task<TrainingSubject> Create(Guid id, string subject, string description)
    {

        return new TrainingSubject(id, subject, description);
    }

    public TrainingSubject Create(ITrainingSubjectVisitor trainingSubjectVisitor)
    {
        return new TrainingSubject(trainingSubjectVisitor.Id, trainingSubjectVisitor.Subject, trainingSubjectVisitor.Description);
    }
}
