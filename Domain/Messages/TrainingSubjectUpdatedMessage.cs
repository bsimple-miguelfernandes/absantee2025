using Domain.Models;
namespace Domain.Messages;
//dto
public record TrainingSubjectUpdatedMessage(Guid Id, string Subject, string Description);