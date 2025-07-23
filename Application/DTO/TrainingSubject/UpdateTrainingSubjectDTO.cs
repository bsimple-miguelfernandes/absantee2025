namespace Application.DTO.TrainingSubject;

public record UpdateTrainingSubjectDTO
{
    public Guid Id { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
    public UpdateTrainingSubjectDTO() { }
    public UpdateTrainingSubjectDTO(Guid id, string subject, string description)
    {
        Id = id;
        Subject = subject;
        Description = description;
    }

}