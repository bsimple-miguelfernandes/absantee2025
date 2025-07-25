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
namespace Application.Tests.ServicesTests;

public class AddTestTrainingPeriod
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

        var periodDate = new PeriodDate(dto.InitDate, dto.FinalDate);
        var trainingPeriod = new TrainingPeriod(Guid.NewGuid(), periodDate);
        var trainingPeriodDto = new TrainingPeriodDTO(trainingPeriod.Id, trainingPeriod.PeriodDate);

        var factoryMock = new Mock<ITrainingPeriodFactory>();
        factoryMock.Setup(f => f.Create(dto.InitDate, dto.FinalDate))
                   .Returns(trainingPeriod);  // síncrono

        var repositoryMock = new Mock<ITrainingPeriodRepository>();
        repositoryMock.Setup(r => r.AddAsync(trainingPeriod))
                      .ReturnsAsync(trainingPeriod); // async ok

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<TrainingPeriod, TrainingPeriodDTO>(trainingPeriod))
                  .Returns(trainingPeriodDto);

        var service = new TrainingPeriodService(repositoryMock.Object, factoryMock.Object, mapperMock.Object);

        // Act
        var result = await service.Add(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(trainingPeriodDto.Id, result.Id);
        Assert.Equal(trainingPeriodDto.PeriodDate, result.PeriodDate);

        factoryMock.Verify(f => f.Create(dto.InitDate, dto.FinalDate), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(trainingPeriod), Times.Once);
        mapperMock.Verify(m => m.Map<TrainingPeriod, TrainingPeriodDTO>(trainingPeriod), Times.Once);
    }
}
