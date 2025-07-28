using Application.DTO.TrainingSubject;

namespace InterfaceAdapters.IntegrationTests.Helpers;

public static class TrainingSubjectHelper
{
    private static readonly Random _random = new();

    // Method to generate random DTO for creating a new Training Subject
    public static AddTrainingSubjectDTO GenerateRandomAddTrainingSubjectDTO()
    {
        return new AddTrainingSubjectDTO
        {
            Subject = GenerateRandomString(1, 20), // Subject: 1–20 characters
            Description = GenerateRandomString(1, 100) // Description: 1–100 characters
        };
    }

    // Method to generate a DTO for creating a new Training Subject
    public static AddTrainingSubjectDTO GenerateAddTrainingSubjectDTO(string subject, string description)
    {
        return new AddTrainingSubjectDTO
        {
            Subject = subject,
            Description = description
        };
    }

    private static string GenerateRandomString(int minLength, int maxLength)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ";
        int length = _random.Next(minLength, maxLength + 1);

        return new string(Enumerable.Range(0, length)
            .Select(_ => chars[_random.Next(chars.Length)])
            .ToArray());
    }
}