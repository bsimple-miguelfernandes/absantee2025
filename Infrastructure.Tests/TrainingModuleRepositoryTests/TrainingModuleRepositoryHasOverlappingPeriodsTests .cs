using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.TrainingModuleRepositoryTests;

public class TrainingModuleRepositoryHasOverlappingPeriodsTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenPeriodsOverlap_ThenReturnsTrue()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var existingPeriod = new PeriodDateTime(DateTime.Today, DateTime.Today.AddDays(5));

        var trainingModule = new Mock<ITrainingModule>();
        trainingModule.Setup(m => m.TrainingSubjectId).Returns(subjectId);
        trainingModule.Setup(m => m.Periods).Returns(new List<PeriodDateTime> { existingPeriod });

        var dataModel = new TrainingModuleDataModel(trainingModule.Object);
        context.TrainingModules.Add(dataModel);
        await context.SaveChangesAsync();

        var repo = new TrainingModuleRepositoryEF(context, _mapper.Object);
        var newPeriods = new List<PeriodDateTime>
        {
            new(DateTime.Today.AddDays(4), DateTime.Today.AddDays(10)) // Overlaps
        };

        // Act
        var result = await repo.HasOverlappingPeriodsAsync(subjectId, newPeriods);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task WhenPeriodsDoNotOverlap_ThenReturnsFalse()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var existingPeriod = new PeriodDateTime(DateTime.Today, DateTime.Today.AddDays(3));

        var trainingModule = new Mock<ITrainingModule>();
        trainingModule.Setup(m => m.TrainingSubjectId).Returns(subjectId);
        trainingModule.Setup(m => m.Periods).Returns(new List<PeriodDateTime> { existingPeriod });

        var dataModel = new TrainingModuleDataModel(trainingModule.Object);
        context.TrainingModules.Add(dataModel);
        await context.SaveChangesAsync();

        var repo = new TrainingModuleRepositoryEF(context, _mapper.Object);
        var newPeriods = new List<PeriodDateTime>
        {
            new(DateTime.Today.AddDays(4), DateTime.Today.AddDays(6)) // No overlap
        };

        // Act
        var result = await repo.HasOverlappingPeriodsAsync(subjectId, newPeriods);

        // Assert
        Assert.False(result);
    }
}
