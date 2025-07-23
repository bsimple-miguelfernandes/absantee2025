using Application.Services;
using Domain.Factory.TrainingPeriodFactory;
using Domain.IRepository;
using Domain.Models;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

public class SubmitAsyncTest
{
    [Fact]
    public async Task SubmitAsync_AddsTrainingPeriodAndSavesChanges()
    {
        // Arrange
        var initDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var finalDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
        var trainingPeriod = new TrainingPeriod(initDate, finalDate);

        var factoryMock = new Mock<ITrainingPeriodFactory>();
        factoryMock.Setup(f => f.Create(initDate, finalDate)).Returns(trainingPeriod);

        var repositoryMock = new Mock<ITrainingPeriodRepository>();
        repositoryMock.Setup(r => r.AddAsync(trainingPeriod)).Returns(Task.CompletedTask);
        repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var mapperMock = new Mock<AutoMapper.IMapper>();

        var service = new TrainingPeriodService(repositoryMock.Object, factoryMock.Object, mapperMock);

        // Act
        await service.SubmitAsync(initDate, finalDate);

        // Assert
        factoryMock.Verify(f => f.Create(initDate, finalDate), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(trainingPeriod), Times.Once);
        repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
