namespace Application.DTO.TrainingSubject;

public record UpdatedTrainingSubjectDTO
{
    public Guid Id { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
    public UpdatedTrainingSubjectDTO() { }
    public UpdatedTrainingSubjectDTO(Guid id, string subject, string description)
    {
        Id = id;
        Subject = subject;
        Description = description;
    }

}