using Domain.Models;
namespace Domain.Messages;
//dto
public record TrainingSubjectMessage(Guid Id, string Subject, string Description);