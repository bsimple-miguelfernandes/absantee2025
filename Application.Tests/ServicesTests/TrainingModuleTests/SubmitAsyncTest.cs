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

public class SubmitAsyncTest
{
    [Fact]
    public async Task SubmitAsync_WhenNotExists_AddsTrainingModule()
    {
        // Arrange
        var id = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var periods = new List<PeriodDateTime>
        {
            new PeriodDateTime(DateTime.UtcNow, DateTime.UtcNow.AddDays(1))
        };

        var mockTrainingModule = new Mock<ITrainingModule>();

        var repositoryMock = new Mock<ITrainingModuleRepository>();
        repositoryMock.Setup(r => r.ExistsAsync(id)).ReturnsAsync(false);
        repositoryMock.Setup(r => r.AddAsync(It.IsAny<ITrainingModule>())).ReturnsAsync(mockTrainingModule.Object);
        repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.FromResult(1));

        var factoryMock = new Mock<ITrainingModuleFactory>();
        factoryMock.Setup(f => f.Create(subjectId, periods)).ReturnsAsync(mockTrainingModule.Object);

        var mapperMock = new Mock<IMapper>();
        var publisherMock = new Mock<IMessagePublisher>();

        var service = new TrainingModuleService(repositoryMock.Object, factoryMock.Object, mapperMock.Object, publisherMock.Object);

        // Act
        await service.SubmitAsync(id, subjectId, periods);

        // Assert
        repositoryMock.Verify(r => r.ExistsAsync(id), Times.Once);
        factoryMock.Verify(f => f.Create(subjectId, periods), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(mockTrainingModule.Object), Times.Once);
        repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task SubmitAsync_WhenExists_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var periods = new List<PeriodDateTime>();

        var repositoryMock = new Mock<ITrainingModuleRepository>();
        repositoryMock.Setup(r => r.ExistsAsync(id)).ReturnsAsync(true);

        var factoryMock = new Mock<ITrainingModuleFactory>();
        var mapperMock = new Mock<IMapper>();
        var publisherMock = new Mock<IMessagePublisher>();

        var service = new TrainingModuleService(repositoryMock.Object, factoryMock.Object, mapperMock.Object, publisherMock.Object);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.SubmitAsync(id, subjectId, periods));
        Assert.Equal($"Training subject with name {subjectId} already exists.", ex.Message);

        repositoryMock.Verify(r => r.ExistsAsync(id), Times.Once);
        factoryMock.Verify(f => f.Create(It.IsAny<Guid>(), It.IsAny<List<PeriodDateTime>>()), Times.Never);
        repositoryMock.Verify(r => r.AddAsync(It.IsAny<ITrainingModule>()), Times.Never);
    }
}
