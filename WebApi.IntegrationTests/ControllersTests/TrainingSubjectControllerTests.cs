using Application.DTO.TrainingSubject;
using WebApi.IntegrationTests.Helpers;
using Xunit;

namespace WebApi.IntegrationTests.Tests;

public class TrainingSubjectControllerTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    public TrainingSubjectControllerTests(IntegrationTestsWebApplicationFactory<Program> factory)
        : base(factory.CreateClient())
    {
    }

    [Fact]
    public async Task CreateTrainingSubject_Return201Created()
    {
        // Arrange : Create a random training subject payload
        var trainingSubjectDTO =
            TrainingSubjectHelper.GenerateRandomAddTrainingSubjectDTO();

        // Act : Send POST request to create the training subject
        var createdTrainingSubjectDTO =
            await PostAndDeserializeAsync<TrainingSubjectDTO>("/api/trainingsubjects", trainingSubjectDTO);

        // Assert
        Assert.NotNull(createdTrainingSubjectDTO);
        Assert.Equal(trainingSubjectDTO.Subject, createdTrainingSubjectDTO.Subject);
        Assert.Equal(trainingSubjectDTO.Description, createdTrainingSubjectDTO.Description);
    }
}