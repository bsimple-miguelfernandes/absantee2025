using Application.DTO.TrainingModule;
using Application.Services;
using Application.IPublisher;
using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class AddTest
{
    [Fact]
    public async Task Add_WhenSuccessful_ReturnsSuccessResult()
    {
        // Arrange
        var tmDTO = new AddTrainingModuleDTO
        {
            TrainingSubjectId = Guid.NewGuid(),
            Periods = new List<PeriodDateTime>
            {
                new PeriodDateTime(DateTime.UtcNow, DateTime.UtcNow.AddDays(1))
            }
        };

        var mockTrainingModule = new Mock<ITrainingModule>();
        mockTrainingModule.SetupGet(tm => tm.Id).Returns(Guid.NewGuid());
        mockTrainingModule.SetupGet(tm => tm.TrainingSubjectId).Returns(tmDTO.TrainingSubjectId);
        mockTrainingModule.SetupGet(tm => tm.Periods).Returns(tmDTO.Periods);

        var factoryMock = new Mock<ITrainingModuleFactory>();
        factoryMock.Setup(f => f.Create(tmDTO.TrainingSubjectId, tmDTO.Periods))
                   .ReturnsAsync(mockTrainingModule.Object);

        var repositoryMock = new Mock<ITrainingModuleRepository>();
        repositoryMock.Setup(r => r.AddAsync(mockTrainingModule.Object))
                      .ReturnsAsync(mockTrainingModule.Object);

        var mapperMock = new Mock<IMapper>();
        var expectedDto = new TrainingModuleDTO(); // cria um DTO vazio ou com valores desejados
        mapperMock.Setup(m => m.Map<TrainingModule, UpdatedTrainingModuleDTO>(It.IsAny<TrainingModule>()))
                  .Returns(expectedDto);

        var publisherMock = new Mock<IMessagePublisher>();
        publisherMock.Setup(p => p.PublishCreatedTrainingModuleMessageAsync(
            It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<List<PeriodDateTime>>()))
            .Returns(Task.CompletedTask);

        var service = new TrainingModuleService(repositoryMock.Object, factoryMock.Object, mapperMock.Object, publisherMock.Object);

        // Act
        var result = await service.Add(tmDTO);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedDto, result.Value);

        factoryMock.Verify(f => f.Create(tmDTO.TrainingSubjectId, tmDTO.Periods), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(mockTrainingModule.Object), Times.Once);
        publisherMock.Verify(p => p.PublishCreatedTrainingModuleMessageAsync(
            mockTrainingModule.Object.Id, tmDTO.TrainingSubjectId, tmDTO.Periods), Times.Once);
    }

    [Fact]
    public async Task Add_WhenFactoryThrows_ReturnsFailureResult()
    {
        // Arrange
        var tmDTO = new AddTrainingModuleDTO
        {
            TrainingSubjectId = Guid.NewGuid(),
            Periods = new List<PeriodDateTime>()
        };

        var factoryMock = new Mock<ITrainingModuleFactory>();
        factoryMock.Setup(f => f.Create(tmDTO.TrainingSubjectId, tmDTO.Periods))
                   .ThrowsAsync(new ArgumentException("Invalid arguments"));

        var repositoryMock = new Mock<ITrainingModuleRepository>();
        var mapperMock = new Mock<IMapper>();
        var publisherMock = new Mock<IMessagePublisher>();

        var service = new TrainingModuleService(repositoryMock.Object, factoryMock.Object, mapperMock.Object, publisherMock.Object);

        // Act
        var result = await service.Add(tmDTO);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Invalid arguments", result.Error.Message);

        factoryMock.Verify(f => f.Create(tmDTO.TrainingSubjectId, tmDTO.Periods), Times.Once);
        repositoryMock.VerifyNoOtherCalls();
        publisherMock.VerifyNoOtherCalls();
    }
}
