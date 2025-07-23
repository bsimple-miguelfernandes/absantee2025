using Domain.Models;

namespace Application.DTO.TrainingModule;

public record UpdateTrainingModuleDTO
{
    public Guid Id { get; set; }
    public Guid TrainingSubjectId { get; set; }
    public List<PeriodDateTime> Periods { get; set; }

    public UpdateTrainingModuleDTO(Guid id, Guid trainingSubjectId, List<PeriodDateTime> periods)
    {
        Id = id;
        TrainingSubjectId = trainingSubjectId;
        Periods = periods;
    }
}