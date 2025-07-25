/* using Domain.Models;
using MassTransit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApi;
using WebApi;
using Domain.Messages;
using Xunit;
using Application.IPublisher;

namespace InterfaceAdapters.IntegrationTests.PublisherTests
{
    public class MassTransitPublisherTests
    {
        [Fact]
        public async Task PublishCreatedTrainingModuleMessageAsync_PublishesTrainingModuleMessageWithHeaders()
        {
            // Arrange
            var mockEndpoint = new Mock<IPublishEndpoint>();
            var publisher = new MassTransitPublisher(mockEndpoint.Object);

            var id = Guid.NewGuid();
            var subjectId = Guid.NewGuid();
            var periods = new List<PeriodDateTime>
            {
                new PeriodDateTime(DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(1)),
                new PeriodDateTime(DateTime.UtcNow.Date.AddDays(2), DateTime.UtcNow.Date.AddDays(3))
            };

            // Act
            await publisher.PublishCreatedTrainingModuleMessageAsync(id, subjectId, periods);

            // Assert
            mockEndpoint.Verify(p => p.Publish(
                It.Is<TrainingModuleMessage>(msg =>
                    msg.Id == id &&
                    msg.SubjectId == subjectId &&
                    msg.Periods == periods
                ),
                It.IsAny<Action<PublishContext<TrainingModuleMessage>>>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task PublishCreatedTrainingSubjectMessageAsync_PublishesTrainingSubjectMessage()
        {
            // Arrange
            var mockEndpoint = new Mock<IPublishEndpoint>();
            var publisher = new MassTransitPublisher(mockEndpoint.Object);

            var id = Guid.NewGuid();
            var description = "Test description";
            var subject = "Test subject";

            // Act
            await publisher.PublishCreatedTrainingSubjectMessageAsync(id, description, subject);

            // Assert
            mockEndpoint.Verify(p => p.Publish(
                It.Is<TrainingSubjectMessage>(msg =>
                    msg.Id == id &&
                    msg.Description == description &&
                    msg.Subject == subject
                ),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task PublishCreatedTrainingPeriodMessageAsync_PublishesTrainingPeriodMessage()
        {
            // Arrange
            var mockEndpoint = new Mock<IPublishEndpoint>();
            var publisher = new MassTransitPublisher(mockEndpoint.Object);

            var id = Guid.NewGuid();
            var periodDate = new PeriodDate(DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)));

            // Act
            await publisher.PublishCreatedTrainingPeriodMessageAsync(id, periodDate);

            // Assert
            mockEndpoint.Verify(p => p.Publish(
                It.Is<TrainingPeriodMessage>(msg =>
                    msg.Id == id &&
                    msg.PeriodDate == periodDate
                ),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
 */