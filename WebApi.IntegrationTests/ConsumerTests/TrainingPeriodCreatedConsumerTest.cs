/* using Application.Services;
using Domain;
using Domain.Models;
using Domain.Messages;

using WebApi;
using MassTransit;
using Moq;
using Xunit;
using Application.IPublisher;
using AutoMapper;
using Domain.IRepository;
using Domain.Factory;
using Domain.Factory.TrainingPeriodFactory;
using Application.DTO;
using Domain.Interfaces;

namespace InterfaceAdapters.IntegrationTests.ConsumerTests;

public class TrainingPeriodCreatedConsumerTests
{
    [Fact]
    public async Task Consume_WhenCalled_CallsSubmitAsyncOnTrainingPeriodService()
    {
        // Arrange - mocks das dependências
        var mockTrainingPeriodRepository = new Mock<ITrainingPeriodRepository>();
        var mockTrainingPeriodFactory = new Mock<ITrainingPeriodFactory>();
        var mockMapper = new Mock<IMapper>();
        var mockPublisher = new Mock<IMessagePublisher>();

        mockTrainingPeriodFactory
            .Setup(f => f.Create(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ReturnsAsync(new TrainingPeriod(Guid.NewGuid(), new PeriodDate(DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)))));

        mockTrainingPeriodRepository.Setup(r => r.ExistsAsync(It.IsAny<DateOnly>(), It.IsAny<DateOnly>())).ReturnsAsync(false);
        mockTrainingPeriodRepository.Setup(r => r.AddAsync(It.IsAny<ITrainingPeriod>()))
            .ReturnsAsync((ITrainingPeriodRepository tp) => tp);

        mockMapper.Setup(m => m.Map<TrainingPeriodDTO>(It.IsAny<ITrainingPeriodRepository>()))
            .Returns((ITrainingPeriod tp) => new TrainingPeriodDTO(tp.Id, tp.PeriodDate));

        mockPublisher.Setup(p => p.PublishCreatedTrainingPeriodMessageAsync(It.IsAny<PeriodDate>()))
            .Returns(Task.CompletedTask);

        var service = new TrainingPeriodService(
            mockTrainingPeriodRepository.Object,
            mockTrainingPeriodFactory.Object,
            mockMapper.Object,
            mockPublisher.Object);

        var consumer = new TrainingPeriodCreatedConsumer(service);

        var initDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var finalDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));

        var message = new TrainingPeriodMessage(Guid.NewGuid(), new PeriodDate(initDate, finalDate));

        var mockContext = new Mock<ConsumeContext<TrainingPeriodMessage>>();
        mockContext.Setup(c => c.Message).Returns(message);

        // Act
        await consumer.Consume(mockContext.Object);

        // Assert
        // Se o método SubmitAsync for interno, não consegues verificar diretamente.
        // Então aqui, em vez de verificar o service, verificas se o repositório foi chamado (efeito colateral)
        mockTrainingPeriodRepository.Verify(r => r.ExistsAsync(initDate, finalDate), Times.Once);
    }

    [Fact]
    public async Task Consume_WhenMessageIsNull_ThrowsException()
    {
        // Arrange
        var mockTrainingPeriodRepository = new Mock<ITrainingPeriodRepository>();
        var mockTrainingPeriodFactory = new Mock<ITrainingPeriodFactory>();
        var mockMapper = new Mock<IMapper>();
        var mockPublisher = new Mock<IMessagePublisher>();

        var service = new TrainingPeriodService(
            mockTrainingPeriodRepository.Object,
            mockTrainingPeriodFactory.Object,
            mockMapper.Object,
            mockPublisher.Object);

        var consumer = new TrainingPeriodCreatedConsumer(service);

        var mockContext = new Mock<ConsumeContext<TrainingPeriodMessage>>();
        mockContext.Setup(c => c.Message).Returns((TrainingPeriodMessage)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => consumer.Consume(mockContext.Object));
    }
}
 */