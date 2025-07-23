using Application.Services;
using MassTransit;
using Moq;
using WebApi;
using WebApi.Message;
using Xunit;

public class TrainingSubjectCreatedConsumerTests
{
    [Fact]
    public async Task Consume_WhenCalledAndSenderIsDifferent_CallsSubmitAsyncOnTrainingSubjectService()
    {
        // Arrange
        var mockService = new Mock<TrainingSubjectService>();
        var consumer = new TrainingSubjectCreatedConsumer(mockService.Object);

        var message = new TrainingSubjectMessage(
            Guid.NewGuid(),
            "Test Subject",
            "Test Description"
        );

        var mockContext = new Mock<ConsumeContext<TrainingSubjectMessage>>();
        mockContext.Setup(c => c.Message).Returns(message);

        // Simular header "SenderId" diferente do InstanceInfo.InstanceId
        mockContext.Setup(c => c.Headers.Get<string>("SenderId", null)).Returns("different-instance-id");

        // Act
        await consumer.Consume(mockContext.Object);

        // Assert
        mockService.Verify(s => s.SubmitAsync(message.Id, message.Subject, message.Description), Times.Once);
    }

    [Fact]
    public async Task Consume_WhenSenderIsSame_DoesNotCallSubmitAsync()
    {
        // Arrange
        var mockService = new Mock<TrainingSubjectService>();
        var consumer = new TrainingSubjectCreatedConsumer(mockService.Object);

        var message = new TrainingSubjectMessage(
            Guid.NewGuid(),
            "Test Subject",
            "Test Description"
        );

        var mockContext = new Mock<ConsumeContext<TrainingSubjectMessage>>();
        mockContext.Setup(c => c.Message).Returns(message);

        // Simular header "SenderId" igual ao InstanceInfo.InstanceId
        mockContext.Setup(c => c.Headers.Get<string>("SenderId", null)).Returns(InstanceInfo.InstanceId);

        // Act
        await consumer.Consume(mockContext.Object);

        // Assert
        mockService.Verify(s => s.SubmitAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}
