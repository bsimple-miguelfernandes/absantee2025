using System.Text.RegularExpressions;
using Domain.Interfaces;

namespace Domain.Models;

public class TrainingSubject : ITrainingSubject
{
    public Guid Id { get; }
    public string Subject { get; private set; }
    public string Description { get; private set; }

    public TrainingSubject(string subject, string description)
    {
        if (string.IsNullOrWhiteSpace(subject) || subject.Length > 20 || !Regex.IsMatch(subject, @"^[a-zA-Z0-9 ]+$"))
            throw new ArgumentException("Subject must be alphanumeric and no longer than 20 characters.");

        if (string.IsNullOrWhiteSpace(description) || description.Length > 100 || !Regex.IsMatch(description, @"^[a-zA-Z0-9 ]+$"))
            throw new ArgumentException("Description must be alphanumeric and no longer than 100 characters.");

        Id = Guid.NewGuid();
        Subject = subject;
        Description = description;
    }

    public TrainingSubject(Guid id, string subject, string description)
    {
        Id = id;
        Subject = subject;
        Description = description;
    }
    public void UpdateSubject(string subject)
    {
        Subject = subject;
    }

    public void UpdateDescription(string description)
    {
        Description = description;
    }
}
