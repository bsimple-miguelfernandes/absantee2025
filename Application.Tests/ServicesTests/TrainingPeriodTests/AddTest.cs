using Application.DTO;
using Application.Services;
using AutoMapper;
using Domain.Factory.TrainingPeriodFactory;
using Domain.IRepository;
using Domain.Models;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

public class AddTest
{
    [Fact]
    public async Task Add_WhenSuccessful_ReturnsMappedTrainingPeriodDTO()
    {
        // Arrange
        var dto = new CreateTrainingPeriodDTO
        {
            InitDate = DateOnly.FromDateTime(DateTime.UtcNow),
            FinalDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))
        };

        var trainingPeriod = new TrainingPeriod(dto.InitDate, dto.FinalDate);
        var trainingPeriodDto = new TrainingPeriodDTO();

        var factoryMock = new Mock<ITrainingPeriodFactory>();
        factoryMock.Setup(f => f.Create(dto.InitDate, dto.FinalDate)).Returns(trainingPeriod);

        var repositoryMock = new Mock<ITrainingPeriodRepository>();
        repositoryMock.Setup(r => r.AddAsync(trainingPeriod)).Returns(Task.CompletedTask);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<TrainingPeriod, TrainingPeriodDTO>(trainingPeriod)).Returns(trainingPeriodDto);

        var service = new TrainingPeriodService(repositoryMock.Object, factoryMock.Object, mapperMock.Object);

        // Act
        var result = await service.Add(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(trainingPeriodDto, result);

        factoryMock.Verify(f => f.Create(dto.InitDate, dto.FinalDate), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(trainingPeriod), Times.Once);
        mapperMock.Verify(m => m.Map<TrainingPeriod, TrainingPeriodDTO>(trainingPeriod), Times.Once);
    }

    [Fact]
    public async Task Add_WhenFactoryThrows_ReturnsNull()
    {
        // Arrange
        var dto = new CreateTrainingPeriodDTO
        {
            InitDate = DateOnly.FromDateTime(DateTime.UtcNow),
            FinalDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))
        };

        var factoryMock = new Mock<ITrainingPeriodFactory>();
        factoryMock.Setup(f => f.Create(dto.InitDate, dto.FinalDate)).Throws(new Exception("error"));

        var repositoryMock = new Mock<ITrainingPeriodRepository>();
        var mapperMock = new Mock<IMapper>();

        var service = new TrainingPeriodService(repositoryMock.Object, factoryMock.Object, mapperMock.Object);

        // Act
        var result = await service.Add(dto);

        // Assert
        Assert.Null(result);

        factoryMock.Verify(f => f.Create(dto.InitDate, dto.FinalDate), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(It.IsAny<TrainingPeriod>()), Times.Never);
        mapperMock.Verify(m => m.Map<TrainingPeriod, TrainingPeriodDTO>(It.IsAny<TrainingPeriod>()), Times.Never);
    }
}
