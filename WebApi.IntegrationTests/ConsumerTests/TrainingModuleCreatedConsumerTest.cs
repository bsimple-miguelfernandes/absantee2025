using Application.Services;
using Domain;
using WebApi;
using MassTransit;
using Moq;
using Xunit;
using Domain.Models;

namespace InterfaceAdapters.IntegrationTests.ConsumerTests;

public class TrainingModuleCreatedConsumerTests
{
    [Fact]
    public async Task Consume_WithDifferentSenderId_CallsSubmitAsync()
    {
        // Arrange
        var mockService = new Mock<TrainingModuleService>();
        var consumer = new TrainingModuleCreatedConsumer(mockService.Object);

        var message = new TrainingModuleMessage(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new List<PeriodDateTime> {
                new PeriodDateTime(DateTime.UtcNow, DateTime.UtcNow.AddDays(3))
            });

        var mockContext = new Mock<ConsumeContext<TrainingModuleMessage>>();
        mockContext.Setup(c => c.Message).Returns(message);
        mockContext.Setup(c => c.Headers.Get<string>("SenderId", null)).Returns("some-other-instance");

        // Act
        await consumer.Consume(mockContext.Object);

        // Assert
        mockService.Verify(s => s.SubmitAsync(
            message.Id,
            message.SubjectId,
            message.Periods), Times.Once);
    }

    [Fact]
    public async Task Consume_WithSameSenderId_DoesNotCallSubmitAsync()
    {
        // Arrange
        var mockService = new Mock<TrainingModuleService>();
        var consumer = new TrainingModuleCreatedConsumer(mockService.Object);

        var message = new TrainingModuleMessage(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new List<PeriodDateTime> {
                new PeriodDateTime(DateTime.UtcNow, DateTime.UtcNow.AddDays(3))
            });

        var mockContext = new Mock<ConsumeContext<TrainingModuleMessage>>();
        mockContext.Setup(c => c.Message).Returns(message);
        mockContext.Setup(c => c.Headers.Get<string>("SenderId", null)).Returns(InstanceInfo.InstanceId);

        // Act
        await consumer.Consume(mockContext.Object);

        // Assert
        mockService.Verify(s => s.SubmitAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<List<PeriodDateTime>>()), Times.Never);
    }

    [Fact]
    public async Task Consume_WhenMessageIsNull_ThrowsException()
    {
        // Arrange
        var mockService = new Mock<TrainingModuleService>();
        var consumer = new TrainingModuleCreatedConsumer(mockService.Object);

        var mockContext = new Mock<ConsumeContext<TrainingModuleMessage>>();
        mockContext.Setup(c => c.Message).Returns((TrainingModuleMessage)null!);
        mockContext.Setup(c => c.Headers.Get<string>("SenderId", null)).Returns("another-id");

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => consumer.Consume(mockContext.Object));
    }
}
