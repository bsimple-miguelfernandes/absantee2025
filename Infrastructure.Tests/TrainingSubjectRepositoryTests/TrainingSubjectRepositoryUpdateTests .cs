using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.TrainingSubjectRepositoryTests;

public class TrainingSubjectRepositoryUpdateTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenUpdatingValidTrainingSubject_ThenReturnsUpdated()
    {
        // Arrange
        var guid = Guid.NewGuid();

        var originalMock = new Mock<ITrainingSubject>();
        originalMock.Setup(t => t.Id).Returns(guid);
        originalMock.Setup(t => t.Subject).Returns("Original");
        originalMock.Setup(t => t.Description).Returns("Original description");

        var originalDM = new TrainingSubjectDataModel(originalMock.Object);
        context.TrainingSubjects.Add(originalDM);
        await context.SaveChangesAsync();

        var updatedMock = new Mock<ITrainingSubject>();
        updatedMock.Setup(t => t.Id).Returns(guid);
        updatedMock.Setup(t => t.Subject).Returns("Updated");
        updatedMock.Setup(t => t.Description).Returns("Updated description");

        _mapper.Setup(m => m.Map<TrainingSubjectDataModel, TrainingSubject>(
            It.Is<TrainingSubjectDataModel>(t => t.Id == guid)))
            .Returns(new TrainingSubject(guid, "Updated", "Updated description"));

        var repo = new TrainingSubjectRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.UpdateTrainingSubject(updatedMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated", result.Subject);
        Assert.Equal("Updated description", result.Description);
    }

    [Fact]
    public async Task WhenTrainingSubjectDoesNotExist_ThenReturnsNull()
    {
        // Arrange
        var mock = new Mock<ITrainingSubject>();
        mock.Setup(t => t.Id).Returns(Guid.NewGuid());
        mock.Setup(t => t.Subject).Returns("Any");
        mock.Setup(t => t.Description).Returns("Any");

        var repo = new TrainingSubjectRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.UpdateTrainingSubject(mock.Object);

        // Assert
        Assert.Null(result);
    }
}
