using Domain.Models;
namespace Domain.Messages;
//dto
public record TrainingModuleUpdatedMessage(Guid Id, Guid SubjectId, List<PeriodDateTime> Periods);