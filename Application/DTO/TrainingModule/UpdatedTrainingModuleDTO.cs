using Domain.Models;

namespace Application.DTO.TrainingModule;

public record UpdatedTrainingModuleDTO
{
    public Guid Id { get; set; }
    public Guid TrainingSubjectId { get; set; }
    public List<PeriodDateTime> Periods { get; set; }

    public UpdatedTrainingModuleDTO(Guid id, Guid trainingSubjectId, List<PeriodDateTime> periods)
    {
        Id = id;
        TrainingSubjectId = trainingSubjectId;
        Periods = periods;
    }
}