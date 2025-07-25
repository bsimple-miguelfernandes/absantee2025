using Application.DTO.TrainingModule;
using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
namespace Application.Tests.ServicesTests;

public class TrainingModuleService_SubmitUpdateAsync_Tests
{
    [Fact]
    public async Task SubmitUpdateAsync_NotFound_ReturnsNotFoundFailure()
    {
        // Arrange
        var id = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var periods = new List<PeriodDateTime>();

        var repositoryMock = new Mock<ITrainingModuleRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TrainingModule?)null);

        var factoryMock = new Mock<ITrainingModuleFactory>();
        var mapperMock = new Mock<AutoMapper.IMapper>();
        var publisherMock = new Mock<Application.IPublisher.IMessagePublisher>();

        var service = new TrainingModuleService(repositoryMock.Object, factoryMock.Object, mapperMock.Object, publisherMock.Object);

        // Act
        var result = await service.SubmitUpdateAsync(id, subjectId, periods);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("TrainingModule not found.", result.Error.Message);
    }

    [Fact]
    public async Task SubmitUpdateAsync_UpdateFails_ReturnsInternalServerError()
    {
        // Arrange
        var id = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var periods = new List<PeriodDateTime>();

        var trainingModule = new TrainingModule(subjectId, periods);

        var repositoryMock = new Mock<ITrainingModuleRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(trainingModule);
        repositoryMock.Setup(r => r.UpdateTrainingModule(trainingModule)).ReturnsAsync((TrainingModule?)null);

        var factoryMock = new Mock<ITrainingModuleFactory>();
        var mapperMock = new Mock<AutoMapper.IMapper>();
        var publisherMock = new Mock<Application.IPublisher.IMessagePublisher>();

        var service = new TrainingModuleService(repositoryMock.Object, factoryMock.Object, mapperMock.Object, publisherMock.Object);

        // Act
        var result = await service.SubmitUpdateAsync(id, subjectId, periods);

        // Assert
        Assert.False(result.IsSuccess);
    }
}
