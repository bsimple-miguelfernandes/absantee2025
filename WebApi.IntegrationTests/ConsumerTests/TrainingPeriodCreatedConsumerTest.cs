using Application.Services;
using Domain;
using Domain.Models;
using WebApi;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ConsumerTests;

public class TrainingPeriodCreatedConsumerTests
{
    [Fact]
    public async Task Consume_WhenCalled_CallsSubmitAsyncOnTrainingPeriodService()
    {
        // Arrange
        var mockService = new Mock<TrainingPeriodService>();
        var consumer = new TrainingPeriodCreatedConsumer(mockService.Object);

        var initDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var finalDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));

        var message = new TrainingPeriodMessage(Guid.NewGuid(), new PeriodDate(initDate, finalDate));
        // Create a mock ConsumeContext
        // and set up the Message property to return the message
        var mockContext = new Mock<ConsumeContext<TrainingPeriodMessage>>();
        mockContext.Setup(c => c.Message).Returns(message);

        // Act
        await consumer.Consume(mockContext.Object);

        // Assert
        mockService.Verify(s => s.SubmitAsync(initDate, finalDate), Times.Once);
    }

    [Fact]
    public async Task Consume_WhenMessageIsNull_ThrowsException()
    {
        // Arrange
        var mockService = new Mock<TrainingPeriodService>();
        var consumer = new TrainingPeriodCreatedConsumer(mockService.Object);

        var mockContext = new Mock<ConsumeContext<TrainingPeriodMessage>>();
        mockContext.Setup(c => c.Message).Returns((TrainingPeriodMessage)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => consumer.Consume(mockContext.Object));
    }
}
