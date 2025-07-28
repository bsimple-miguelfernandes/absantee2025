using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.TrainingModuleRepositoryTests;

public class TrainingModuleRepositoryUpdateTests : RepositoryTestBase
{
    [Fact]
    public async Task UpdateTrainingModule_WhenModuleExists_UpdatesAndReturnsUpdatedModule()
    {
        // Arrange
        var originalPeriod = new PeriodDateTime(DateTime.Today.AddDays(-10), DateTime.Today.AddDays(-5));
        var updatedPeriod = new PeriodDateTime(DateTime.Today.AddDays(1), DateTime.Today.AddDays(10));
        var subjectId = Guid.NewGuid();

        var existingTrainingModuleDM = new TrainingModuleDataModel
        {
            Id = Guid.NewGuid(),
            TrainingSubjectId = subjectId,
            Periods = new List<PeriodDateTime> { originalPeriod }
        };

        context.TrainingModules.Add(existingTrainingModuleDM);
        await context.SaveChangesAsync();

        var updatedTrainingModule = new TrainingModule(
            existingTrainingModuleDM.Id,
            subjectId,
            new List<PeriodDateTime> { updatedPeriod }
        );

        // Setup mapper to map back from DataModel to Domain Model
        _mapper.Setup(m => m.Map<TrainingModuleDataModel, TrainingModule>(It.IsAny<TrainingModuleDataModel>()))
               .Returns((TrainingModuleDataModel dm) =>
                   new TrainingModule(dm.Id, dm.TrainingSubjectId, dm.Periods.ToList())
               );

        var repo = new TrainingModuleRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.UpdateTrainingModule(updatedTrainingModule);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(existingTrainingModuleDM.Id, result.Id);
        Assert.Equal(subjectId, result.TrainingSubjectId);
        Assert.Single(result.Periods);
        Assert.Equal(updatedPeriod._initDate, result.Periods.First()._initDate);
        Assert.Equal(updatedPeriod._finalDate, result.Periods.First()._finalDate);

        // Also verify the database was updated
        var updatedFromDb = await context.TrainingModules
                                         .Include(tm => tm.Periods)
                                         .FirstOrDefaultAsync(tm => tm.Id == existingTrainingModuleDM.Id);

        Assert.NotNull(updatedFromDb);
        Assert.Single(updatedFromDb.Periods);
        Assert.Equal(updatedPeriod._initDate, updatedFromDb.Periods.First()._initDate);
        Assert.Equal(updatedPeriod._finalDate, updatedFromDb.Periods.First()._finalDate);
    }

    [Fact]
    public async Task UpdateTrainingModule_WhenModuleDoesNotExist_ReturnsNull()
    {
        // Arrange
        var nonExistingModule = new TrainingModule(Guid.NewGuid(), Guid.NewGuid(), new List<PeriodDateTime>());

        var repo = new TrainingModuleRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.UpdateTrainingModule(nonExistingModule);

        // Assert
        Assert.Null(result);
    }
}
