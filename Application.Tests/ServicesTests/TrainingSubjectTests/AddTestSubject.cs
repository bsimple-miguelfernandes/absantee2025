using Application.DTO.TrainingSubject;
using Application.Services;
using Application.IPublisher;
using AutoMapper;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
namespace Application.Tests.ServicesTests;

public class AddTestSubject
{
    [Fact]
    public async Task Add_WhenSuccessful_ReturnsSuccessResult()
    {
        // Arrange
        var tsDTO = new AddTrainingSubjectDTO("Subject A", "Description A");

        var trainingSubject = new TrainingSubject(tsDTO.Subject, tsDTO.Description);
        var trainingSubjectDto = new TrainingSubjectDTO();

        var factoryMock = new Mock<ITrainingSubjectFactory>();
        factoryMock.Setup(f => f.Create(tsDTO.Subject, tsDTO.Description))
                   .ReturnsAsync(trainingSubject);

        var repositoryMock = new Mock<ITrainingSubjectRepository>();
        repositoryMock.Setup(r => r.AddAsync(trainingSubject))
                      .ReturnsAsync(trainingSubject); // <-- Aqui

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<TrainingSubject, TrainingSubjectDTO>(trainingSubject))
                  .Returns(trainingSubjectDto);

        var publisherMock = new Mock<IMessagePublisher>();
        publisherMock.Setup(p => p.PublishCreatedTrainingSubjectMessageAsync(
            trainingSubject.Id, tsDTO.Subject, tsDTO.Description))
            .Returns(Task.CompletedTask);

        var service = new TrainingSubjectService(repositoryMock.Object, factoryMock.Object, mapperMock.Object, publisherMock.Object);

        // Act
        var result = await service.Add(tsDTO);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(trainingSubjectDto, result.Value);

        factoryMock.Verify(f => f.Create(tsDTO.Subject, tsDTO.Description), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(trainingSubject), Times.Once);
        publisherMock.Verify(p => p.PublishCreatedTrainingSubjectMessageAsync(
            trainingSubject.Id, tsDTO.Subject, tsDTO.Description), Times.Once);
        mapperMock.Verify(m => m.Map<TrainingSubject, TrainingSubjectDTO>(trainingSubject), Times.Once);
    }

    [Fact]
    public async Task Add_WhenFactoryThrowsOtherException_ReturnsFailureInternalServerError()
    {
        // Arrange
        var tsDTO = new AddTrainingSubjectDTO("Subject C", "Description C");

        var factoryMock = new Mock<ITrainingSubjectFactory>();
        factoryMock.Setup(f => f.Create(tsDTO.Subject, tsDTO.Description))
                   .ThrowsAsync(new Exception("Unexpected error"));

        var repositoryMock = new Mock<ITrainingSubjectRepository>();
        var mapperMock = new Mock<IMapper>();
        var publisherMock = new Mock<IMessagePublisher>();

        var service = new TrainingSubjectService(repositoryMock.Object, factoryMock.Object, mapperMock.Object, publisherMock.Object);

        // Act
        var result = await service.Add(tsDTO);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Unexpected error", result.Error.Message);

        factoryMock.Verify(f => f.Create(tsDTO.Subject, tsDTO.Description), Times.Once);
        repositoryMock.VerifyNoOtherCalls();
        publisherMock.VerifyNoOtherCalls();
        mapperMock.VerifyNoOtherCalls();
    }
}
