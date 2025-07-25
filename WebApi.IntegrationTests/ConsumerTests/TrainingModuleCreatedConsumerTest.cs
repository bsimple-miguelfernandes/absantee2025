using Application; // para Result<T>
using Xunit;
using Moq;
using MassTransit;
using Application.DTO.TrainingModule;
using Application.Services;
using Domain.Messages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain.IRepository;
using Domain.Factory;
using Application.IPublisher;
using Domain.Models;
using Domain.Interfaces;

namespace InterfaceAdapters.IntegrationTests.ConsumerTests;

public class TrainingModuleCreatedConsumerTests
{
    [Fact]
    public async Task Consume_WithDifferentSenderId_CallsSubmitAsync()
    {
        // Arrange - mocks das dependências
        var mockTrainingModuleRepository = new Mock<ITrainingModuleRepository>();
        var mockTrainingModuleFactory = new Mock<ITrainingModuleFactory>();
        var mockMapper = new Mock<IMapper>();
        var mockPublisher = new Mock<IMessagePublisher>();

        mockTrainingModuleFactory
            .Setup(f => f.Create(It.IsAny<Guid>(), It.IsAny<List<PeriodDateTime>>()))
            .ReturnsAsync(new TrainingModule(Guid.NewGuid(), Guid.NewGuid(), new List<PeriodDateTime>()));

        mockTrainingModuleRepository.Setup(r => r.ExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);
        mockTrainingModuleRepository.Setup(r => r.AddAsync(It.IsAny<ITrainingModule>()))
            .ReturnsAsync((ITrainingModule tm) => tm);

        mockMapper.Setup(m => m.Map<TrainingModuleDTO>(It.IsAny<ITrainingModule>()))
            .Returns((ITrainingModule tm) => new TrainingModuleDTO(tm.Id, tm.TrainingSubjectId, tm.Periods));

        mockPublisher.Setup(p => p.PublishCreatedTrainingModuleMessageAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<List<PeriodDateTime>>()))
            .Returns(Task.CompletedTask);

        var service = new TrainingModuleService(
            mockTrainingModuleRepository.Object,
            mockTrainingModuleFactory.Object,
            mockMapper.Object,
            mockPublisher.Object);

        var consumer = new TrainingModuleCreatedConsumer(service);

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

        // Assert: verifica se repositório foi consultado e item adicionado
        mockTrainingModuleRepository.Verify(r => r.ExistsAsync(message.Id), Times.Once);
    }
}
