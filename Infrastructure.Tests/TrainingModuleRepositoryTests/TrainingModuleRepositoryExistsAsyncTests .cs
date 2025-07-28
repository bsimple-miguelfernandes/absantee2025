using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;
namespace Infrastructure.Tests.TrainingModuleRepositoryTests;

public class TrainingModuleRepositoryExistsAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenModuleExists_ThenReturnsTrue()
    {
        // Arrange
        var trainingSubject1 = new Mock<ITrainingSubject>();
        var guid1 = Guid.NewGuid();
        trainingSubject1.Setup(t => t.Id).Returns(guid1);
        trainingSubject1.Setup(t => t.Subject).Returns("Subject1");
        trainingSubject1.Setup(t => t.Description).Returns("Description1");
        var subject = new TrainingSubjectDataModel(trainingSubject1.Object
        );
        context.TrainingSubjects.Add(subject);
        await context.SaveChangesAsync();

        var repo = new TrainingModuleRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.ExistsAsync(guid1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task WhenModuleDoesNotExist_ThenReturnsFalse()
    {
        // Arrange
        var repo = new TrainingModuleRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.ExistsAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
    }
}
