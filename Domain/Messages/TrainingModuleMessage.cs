using Domain.Models;
namespace Domain.Messages;
//dto
public record TrainingModuleMessage(Guid Id, Guid SubjectId, List<PeriodDateTime> Periods);