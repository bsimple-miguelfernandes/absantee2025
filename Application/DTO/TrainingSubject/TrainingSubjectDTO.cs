namespace Application.DTO.TrainingSubject;

public record TrainingSubjectDTO
{
    public Guid Id { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
    public TrainingSubjectDTO()
    {

    }
    public TrainingSubjectDTO(Guid id, string subject, string description)
    {
        Id = id;
        Subject = subject;
        Description = description;
    }
}