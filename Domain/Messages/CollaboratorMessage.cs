using Domain.Models;
namespace Domain.Messages;
//dto
public record CollaboratorMessage(Guid Id, Guid UserId, PeriodDateTime PeriodDateTime);