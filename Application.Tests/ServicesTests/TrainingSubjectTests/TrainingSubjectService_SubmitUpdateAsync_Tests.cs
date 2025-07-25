using Application.DTO.TrainingSubject;
using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
namespace Application.Tests.ServicesTests;

public class TrainingSubjectService_SubmitUpdateAsync_Tests
{
    [Fact]
    public async Task SubmitUpdateAsync_WhenNotFound_ReturnsNotFoundFailure()
    {
        // Arrange
        var id = Guid.NewGuid();
        var subject = "Subject";
        var description = "Description";

        var repositoryMock = new Mock<ITrainingSubjectRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TrainingSubject?)null);

        var factoryMock = new Mock<ITrainingSubjectFactory>();
        var mapperMock = new Mock<AutoMapper.IMapper>();
        var publisherMock = new Mock<Application.IPublisher.IMessagePublisher>();

        var service = new TrainingSubjectService(repositoryMock.Object, factoryMock.Object, mapperMock.Object, publisherMock.Object);

        // Act
        var result = await service.SubmitUpdateAsync(id, subject, description);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("TrainingSubject not found.", result.Error.Message);

        repositoryMock.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task SubmitUpdateAsync_WhenUpdateFails_ReturnsInternalServerError()
    {
        // Arrange
        var id = Guid.NewGuid();
        var subject = "Subject";
        var description = "Description";
        var trainingSubject = new TrainingSubject("Old Subject", "Old Description");

        var repositoryMock = new Mock<ITrainingSubjectRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(trainingSubject);
        repositoryMock.Setup(r => r.UpdateTrainingSubject(trainingSubject)).ReturnsAsync((TrainingSubject?)null);

        var factoryMock = new Mock<ITrainingSubjectFactory>();
        var mapperMock = new Mock<AutoMapper.IMapper>();
        var publisherMock = new Mock<Application.IPublisher.IMessagePublisher>();

        var service = new TrainingSubjectService(repositoryMock.Object, factoryMock.Object, mapperMock.Object, publisherMock.Object);

        // Act
        var result = await service.SubmitUpdateAsync(id, subject, description);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to update TrainingSubject.", result.Error.Message);

        repositoryMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        repositoryMock.Verify(r => r.UpdateTrainingSubject(trainingSubject), Times.Once);
    }

    [Fact]
    public async Task SubmitUpdateAsync_WhenSuccessful_ReturnsSuccessResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var subject = "Updated Subject";
        var description = "Updated Description";

        var trainingSubject = new TrainingSubject("Old Subject", "Old Description");
        var updatedTrainingSubject = new TrainingSubject(subject, description);

        var repositoryMock = new Mock<ITrainingSubjectRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(trainingSubject);
        repositoryMock.Setup(r => r.UpdateTrainingSubject(trainingSubject)).ReturnsAsync(updatedTrainingSubject);

        var mapperMock = new Mock<AutoMapper.IMapper>();
        var updatedDto = new TrainingSubjectDTO();
        mapperMock.Setup(m => m.Map<TrainingSubjectDTO>(updatedTrainingSubject)).Returns(updatedDto);

        var factoryMock = new Mock<ITrainingSubjectFactory>();
        var publisherMock = new Mock<Application.IPublisher.IMessagePublisher>();

        var service = new TrainingSubjectService(repositoryMock.Object, factoryMock.Object, mapperMock.Object, publisherMock.Object);

        // Act
        var result = await service.SubmitUpdateAsync(id, subject, description);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(updatedDto, result.Value);

        repositoryMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        repositoryMock.Verify(r => r.UpdateTrainingSubject(trainingSubject), Times.Once);
        mapperMock.Verify(m => m.Map<TrainingSubjectDTO>(updatedTrainingSubject), Times.Once);
    }
}
