using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

public class SubmitAsyncTest
{
    [Fact]
    public async Task SubmitAsync_WhenNotExists_AddsTrainingSubjectAndSavesChanges()
    {
        // Arrange
        var id = Guid.NewGuid();
        var subject = "New Subject";
        var description = "New Description";
        var trainingSubject = new TrainingSubject(subject, description);

        var factoryMock = new Mock<ITrainingSubjectFactory>();
        factoryMock.Setup(f => f.Create(subject, description))
                   .ReturnsAsync(trainingSubject);

        var repositoryMock = new Mock<ITrainingSubjectRepository>();
        repositoryMock.Setup(r => r.ExistsAsync(id)).ReturnsAsync(false);
        repositoryMock.Setup(r => r.AddAsync(trainingSubject)).Returns(Task.CompletedTask);
        repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var mapperMock = new Mock<AutoMapper.IMapper>();
        var publisherMock = new Mock<Application.IPublisher.IMessagePublisher>();

        var service = new TrainingSubjectService(repositoryMock.Object, factoryMock.Object, mapperMock.Object, publisherMock.Object);

        // Act
        await service.SubmitAsync(id, subject, description);

        // Assert
        repositoryMock.Verify(r => r.ExistsAsync(id), Times.Once);
        factoryMock.Verify(f => f.Create(subject, description), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(trainingSubject), Times.Once);
        repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task SubmitAsync_WhenExists_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var subject = "Existing Subject";
        var description = "Some description";

        var repositoryMock = new Mock<ITrainingSubjectRepository>();
        repositoryMock.Setup(r => r.ExistsAsync(id)).ReturnsAsync(true);

        var factoryMock = new Mock<ITrainingSubjectFactory>();
        var mapperMock = new Mock<AutoMapper.IMapper>();
        var publisherMock = new Mock<Application.IPublisher.IMessagePublisher>();

        var service = new TrainingSubjectService(repositoryMock.Object, factoryMock.Object, mapperMock.Object, publisherMock.Object);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.SubmitAsync(id, subject, description));
        Assert.Equal($"Training subject with name {subject} already exists.", ex.Message);

        repositoryMock.Verify(r => r.ExistsAsync(id), Times.Once);
        factoryMock.Verify(f => f.Create(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        repositoryMock.Verify(r => r.AddAsync(It.IsAny<TrainingSubject>()), Times.Never);
    }
}
