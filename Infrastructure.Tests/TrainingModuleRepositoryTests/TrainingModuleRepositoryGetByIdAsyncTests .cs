using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.TrainingModuleRepositoryTests;

public class TrainingModuleRepositoryGetByIdAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenPassingValidId_ThenReturnsTrainingModule()
    {
        // Arrange
        var mock = new Mock<ITrainingModule>();
        var id = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var periods = new List<PeriodDateTime>
        {
            new(DateTime.Now.AddDays(1), DateTime.Now.AddDays(3))
        };
        mock.Setup(m => m.Id).Returns(id);
        mock.Setup(m => m.TrainingSubjectId).Returns(subjectId);
        mock.Setup(m => m.Periods).Returns(periods);

        var dataModel = new TrainingModuleDataModel(mock.Object);
        context.TrainingModules.Add(dataModel);
        await context.SaveChangesAsync();

        _mapper.Setup(m => m.Map<TrainingModuleDataModel, TrainingModule>(It.Is<TrainingModuleDataModel>(tm => tm.Id == id)))
            .Returns(new TrainingModule(id, subjectId, periods));

        var repo = new TrainingModuleRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task WhenPassingInvalidId_ThenReturnsNull()
    {
        // Arrange
        var repo = new TrainingModuleRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }
}
