using Application.DTO.TrainingModule;
using Application.DTO.TrainingSubject;
using WebApi.IntegrationTests.Helpers;
using Xunit;

namespace WebApi.IntegrationTests.Tests;

public class TrainingModuleControllerTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    public TrainingModuleControllerTests(IntegrationTestsWebApplicationFactory<Program> factory)
        : base(factory.CreateClient())
    {
    }

    [Fact]
    public async Task CreateTrainingModule_Returns201Created()
    {
        // Arrange: criar um assunto primeiro
        var subject = await PostAndDeserializeAsync<TrainingSubjectDTO>("/api/trainingsubjects", new AddTrainingSubjectDTO
        {
            Subject = "Segurança Industrial"
        });

        // Criar módulo com base no ID do assunto
        var trainingModuleDTO =
            TrainingModuleHelper.GenerateAddTrainingModuleDTORandomDates(subject.Id);

        // Act
        var createdTrainingModuleDTO =
            await PostAndDeserializeAsync<UpdatedTrainingModuleDTO>("/api/trainingmodules", trainingModuleDTO);

        // Assert
        Assert.NotNull(createdTrainingModuleDTO);
        Assert.Equal(subject.Id, createdTrainingModuleDTO.TrainingSubjectId);
        Assert.NotEmpty(createdTrainingModuleDTO.Periods);
        Assert.Equal(trainingModuleDTO.Periods.Count, createdTrainingModuleDTO.Periods.Count);
    }

}
