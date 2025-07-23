using Domain.Models;
namespace Domain.Messages;
//dto
public record TrainingPeriodMessage(Guid Id, PeriodDate PeriodDate);