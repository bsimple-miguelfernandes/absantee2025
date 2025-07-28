using System;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Factory;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;
using Xunit;

namespace Infrastructure.Tests.TrainingSubjectRepositoryTests;

public class TrainingSubjectRepositoryGetByIdAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenPassingValidId_ThenReturnTrainingSubject()
    {
        // Arrange
        var guid1 = Guid.NewGuid();
        var trainingSubject1DM = new TrainingSubjectDataModel
        {
            Id = guid1,
            Subject = "Subject1",
            Description = "Description1"
        };
        context.TrainingSubjects.Add(trainingSubject1DM);
        await context.SaveChangesAsync();

        var mapper = new Mock<IMapper>();
        var factory = new Mock<ITrainingSubjectFactory>();

        mapper.Setup(m => m.Map<TrainingSubjectDataModel, TrainingSubject>(
            It.Is<TrainingSubjectDataModel>(t => t.Id == trainingSubject1DM.Id)))
            .Returns(new TrainingSubject(trainingSubject1DM.Id, trainingSubject1DM.Subject, trainingSubject1DM.Description));

        var repo = new TrainingSubjectRepositoryEF(context, mapper.Object, factory.Object);

        // Act
        var result = await repo.GetByIdAsync(guid1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(guid1, result.Id);
        Assert.Equal("Subject1", result.Subject);
        Assert.Equal("Description1", result.Description);
    }

    [Fact]
    public async Task WhenPassingInvalidId_ThenReturnNull()
    {
        // Arrange
        var validSubject = new TrainingSubjectDataModel
        {
            Id = Guid.NewGuid(),
            Subject = "Subject1",
            Description = "Description1"
        };
        context.TrainingSubjects.Add(validSubject);
        await context.SaveChangesAsync();

        var mapper = new Mock<IMapper>();
        var factory = new Mock<ITrainingSubjectFactory>();

        var repo = new TrainingSubjectRepositoryEF(context, mapper.Object, factory.Object);

        // Act
        var result = await repo.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }
}
