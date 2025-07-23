using Domain.Models;
namespace Domain.Messages;
//dto
public record AssociationTrainingModuleCollaboratorMessage(Guid Id, Guid TrainingModuleId, Guid CollaboratorId);