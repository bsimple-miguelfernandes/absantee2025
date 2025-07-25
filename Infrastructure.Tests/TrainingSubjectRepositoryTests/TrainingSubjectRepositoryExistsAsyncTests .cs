using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.TrainingSubjectRepositoryTests;

public class TrainingSubjectRepositoryExistsAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenIdExists_ThenReturnsTrue()
    {
        // Arrange
        var trainingSubject1 = new Mock<ITrainingSubject>();
        var guid1 = Guid.NewGuid();
        trainingSubject1.Setup(t => t.Id).Returns(guid1);
        trainingSubject1.Setup(t => t.Subject).Returns("Subject1");
        trainingSubject1.Setup(t => t.Description).Returns("Description1");
        var trainingSubjectDM = new TrainingSubjectDataModel(trainingSubject1.Object);
        context.TrainingSubjects.Add(trainingSubjectDM);
        await context.SaveChangesAsync();

        var repo = new TrainingSubjectRepositoryEF(context, _mapper.Object);

        // Act
        var exists = await repo.ExistsAsync(guid1);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task WhenIdDoesNotExist_ThenReturnsFalse()
    {
        // Arrange
        var repo = new TrainingSubjectRepositoryEF(context, _mapper.Object);

        // Act
        var exists = await repo.ExistsAsync(Guid.NewGuid());

        // Assert
        Assert.False(exists);
    }
}
