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


public class TrainingSubjectService_UpdateTrainingSubject_Tests
{
    [Fact]
    public async Task UpdateTrainingSubject_WhenNotFound_ReturnsNotFoundFailure()
    {
        // Arrange
        var dto = new UpdateTrainingSubjectDTO(Guid.NewGuid(), "Subject", "Description");

        var repositoryMock = new Mock<ITrainingSubjectRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync((TrainingSubject?)null);

        var factoryMock = new Mock<ITrainingSubjectFactory>();
        var mapperMock = new Mock<AutoMapper.IMapper>();
        var publisherMock = new Mock<Application.IPublisher.IMessagePublisher>();

        var service = new TrainingSubjectService(repositoryMock.Object, factoryMock.Object, mapperMock.Object, publisherMock.Object);

        // Act
        var result = await service.UpdateTrainingSubject(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("TrainingSubject not found.", result.Error.Message);

        repositoryMock.Verify(r => r.GetByIdAsync(dto.Id), Times.Once);
    }

    [Fact]
    public async Task UpdateTrainingSubject_WhenUpdateFails_ReturnsInternalServerError()
    {
        // Arrange
        var dto = new UpdateTrainingSubjectDTO(Guid.NewGuid(), "Subject", "Description");
        var trainingSubject = new TrainingSubject("Old Subject", "Old Description");

        var repositoryMock = new Mock<ITrainingSubjectRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync(trainingSubject);
        repositoryMock.Setup(r => r.UpdateTrainingSubject(trainingSubject)).ReturnsAsync((TrainingSubject?)null);

        var factoryMock = new Mock<ITrainingSubjectFactory>();
        var mapperMock = new Mock<AutoMapper.IMapper>();
        var publisherMock = new Mock<Application.IPublisher.IMessagePublisher>();

        var service = new TrainingSubjectService(repositoryMock.Object, factoryMock.Object, mapperMock.Object, publisherMock.Object);

        // Act
        var result = await service.UpdateTrainingSubject(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to update TrainingSubject.", result.Error.Message);

        repositoryMock.Verify(r => r.GetByIdAsync(dto.Id), Times.Once);
        repositoryMock.Verify(r => r.UpdateTrainingSubject(trainingSubject), Times.Once);
    }

    [Fact]
    public async Task UpdateTrainingSubject_WhenSuccessful_ReturnsSuccessResult()
    {
        // Arrange
        var dto = new UpdateTrainingSubjectDTO(Guid.NewGuid(), "Updated Subject", "Updated Description");
        var trainingSubject = new TrainingSubject("Old Subject", "Old Description");

        var updatedTrainingSubject = new TrainingSubject(dto.Subject, dto.Description);

        var repositoryMock = new Mock<ITrainingSubjectRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync(trainingSubject);
        repositoryMock.Setup(r => r.UpdateTrainingSubject(trainingSubject)).ReturnsAsync(updatedTrainingSubject);

        var mapperMock = new Mock<AutoMapper.IMapper>();
        var updatedDto = new UpdatedTrainingSubjectDTO();
        mapperMock.Setup(m => m.Map<UpdatedTrainingSubjectDTO>(updatedTrainingSubject)).Returns(updatedDto);

        var publisherMock = new Mock<Application.IPublisher.IMessagePublisher>();
        publisherMock.Setup(p => p.PublishUpdatedTrainingSubjectMessageAsync(updatedTrainingSubject.Id, updatedTrainingSubject.Subject, updatedTrainingSubject.Description))
                     .Returns(Task.CompletedTask);

        var factoryMock = new Mock<ITrainingSubjectFactory>();

        var service = new TrainingSubjectService(repositoryMock.Object, factoryMock.Object, mapperMock.Object, publisherMock.Object);

        // Act
        var result = await service.UpdateTrainingSubject(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(updatedDto, result.Value);

        repositoryMock.Verify(r => r.GetByIdAsync(dto.Id), Times.Once);
        repositoryMock.Verify(r => r.UpdateTrainingSubject(trainingSubject), Times.Once);
        publisherMock.Verify(p => p.PublishUpdatedTrainingSubjectMessageAsync(updatedTrainingSubject.Id, updatedTrainingSubject.Subject, updatedTrainingSubject.Description), Times.Once);
        mapperMock.Verify(m => m.Map<UpdatedTrainingSubjectDTO>(updatedTrainingSubject), Times.Once);
    }
}
