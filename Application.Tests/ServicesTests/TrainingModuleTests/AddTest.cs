using Application.DTO.TrainingModule;
using Application.Services;
using Application.IPublisher;
using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;
using System.Reflection;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
namespace Application.Tests.ServicesTests;

public class AddTest
{
    [Fact]
    public async Task Add_WhenSuccessful_ReturnsSuccessResult()
    {
        // Arrange
        var trainingModuleId = Guid.NewGuid();
        var trainingSubjectId = Guid.NewGuid();

        // Datas no futuro para evitar falha de validação
        var futureStart = new DateTime(2050, 1, 1, 8, 0, 0, DateTimeKind.Utc);
        var futureEnd = new DateTime(2050, 1, 2, 8, 0, 0, DateTimeKind.Utc);

        var periods = new List<PeriodDateTime>
    {
        new PeriodDateTime(futureStart, futureEnd)
    };

        var tmDTO = new AddTrainingModuleDTO(trainingSubjectId, periods);

        // Cria instância concreta
        var trainingModule = new TrainingModule(trainingSubjectId, periods);

        // Define o Id via campo privado gerado pelo compilador
        var idField = typeof(TrainingModule).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
        idField?.SetValue(trainingModule, trainingModuleId);

        var factoryMock = new Mock<ITrainingModuleFactory>();
        factoryMock.Setup(f => f.Create(trainingSubjectId, periods))
                   .ReturnsAsync(trainingModule);

        var repositoryMock = new Mock<ITrainingModuleRepository>();
        repositoryMock.Setup(r => r.AddAsync(trainingModule))
                      .ReturnsAsync(trainingModule);

        var expectedDto = new TrainingModuleDTO(trainingModuleId, trainingSubjectId, periods);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<TrainingModule, TrainingModuleDTO>(trainingModule))
                  .Returns(expectedDto);

        var publisherMock = new Mock<IMessagePublisher>();
        publisherMock.Setup(p => p.PublishCreatedTrainingModuleMessageAsync(
            trainingModuleId, trainingSubjectId, periods))
            .Returns(Task.CompletedTask);

        var service = new TrainingModuleService(repositoryMock.Object, factoryMock.Object, mapperMock.Object, publisherMock.Object);

        // Act
        var result = await service.Add(tmDTO);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedDto, result.Value);

        factoryMock.Verify(f => f.Create(trainingSubjectId, periods), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(trainingModule), Times.Once);
        publisherMock.Verify(p => p.PublishCreatedTrainingModuleMessageAsync(
            trainingModuleId, trainingSubjectId, periods), Times.Once);
    }





    [Fact]
    public async Task Add_WhenFactoryThrows_ReturnsFailureResult()
    {
        // Arrange
        var trainingSubjectId = Guid.NewGuid();
        var periods = new List<PeriodDateTime>();

        var tmDTO = new AddTrainingModuleDTO(trainingSubjectId, periods);

        var factoryMock = new Mock<ITrainingModuleFactory>();
        factoryMock.Setup(f => f.Create(trainingSubjectId, periods))
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

        factoryMock.Verify(f => f.Create(trainingSubjectId, periods), Times.Once);
        repositoryMock.VerifyNoOtherCalls();
        publisherMock.VerifyNoOtherCalls();
    }

}
