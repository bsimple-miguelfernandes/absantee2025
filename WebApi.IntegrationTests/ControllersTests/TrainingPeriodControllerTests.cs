using Application.DTO;
using WebApi.IntegrationTests.Helpers;
using Xunit;

namespace WebApi.IntegrationTests.Tests;

public class TrainingPeriodControllerTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    public TrainingPeriodControllerTests(IntegrationTestsWebApplicationFactory<Program> factory)
        : base(factory.CreateClient())
    {
    }

    [Fact]
    public async Task PostTrainingPeriod_Returns201Created()
    {
        // Arrange
        var dto = TrainingPeriodHelper.GenerateCreateTrainingPeriodDTO();

        // Act
        var createdPeriod = await PostAndDeserializeAsync<TrainingPeriodDTO>("/api/trainingperiod", dto);

        // Assert
        Assert.NotNull(createdPeriod);
        Assert.Equal(dto.InitDate, createdPeriod.PeriodDate.InitDate);
        Assert.Equal(dto.FinalDate, createdPeriod.PeriodDate.FinalDate);
    }
}
