/* using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;
using Domain.Models;


namespace Infrastructure.Tests.TrainingSubjectRepositoryTests;

public class TrainingSubjectRepositoryIsDuplicatedTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenSearchingIfTrainingSubjectIsDuplicatedWhenIsDuplicated_ThenReturnsTrue()
    {
        //Assert
        var trainingSubject1 = new Mock<ITrainingSubject>();
        var guid1 = Guid.NewGuid();
        trainingSubject1.Setup(t => t.Id).Returns(guid1);
        trainingSubject1.Setup(t => t.Subject).Returns("Subject1");
        trainingSubject1.Setup(t => t.Description).Returns("Description1");
        var trainingSubject1DM = new TrainingSubjectDataModel(trainingSubject1.Object);
        context.TrainingSubjects.Add(trainingSubject1DM);

        await context.SaveChangesAsync();

        _mapper.Setup(m => m.Map<TrainingSubjectDataModel, TrainingSubject>(
                   It.Is<TrainingSubjectDataModel>(t =>
                       t.Id == trainingSubject1DM.Id
                       )))
                       .Returns(new TrainingSubject(trainingSubject1DM.Id, trainingSubject1DM.Subject, trainingSubject1DM.Description));

        var trainingSubjectRepo = new TrainingSubjectRepositoryEF(context, _mapper.Object);

        //Act
        var result = await trainingSubjectRepo.IsDuplicated("Subject1");

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task WhenSearchingIfTrainingSubjectIsDuplicatedWhenIsNotDuplicated_ThenReturnsFalse()
    {
        //Assert
        var trainingSubject1 = new Mock<ITrainingSubject>();
        var guid1 = Guid.NewGuid();
        trainingSubject1.Setup(t => t.Id).Returns(guid1);
        trainingSubject1.Setup(t => t.Subject).Returns("Subject1");
        trainingSubject1.Setup(t => t.Description).Returns("Description1");
        var trainingSubject1DM = new TrainingSubjectDataModel(trainingSubject1.Object);
        context.TrainingSubjects.Add(trainingSubject1DM);

        await context.SaveChangesAsync();

        _mapper.Setup(m => m.Map<TrainingSubjectDataModel, TrainingSubject>(
                   It.Is<TrainingSubjectDataModel>(t =>
                       t.Id == trainingSubject1DM.Id
                       )))
                       .Returns(new TrainingSubject(trainingSubject1DM.Id, trainingSubject1DM.Subject, trainingSubject1DM.Description));

        var trainingSubjectRepo = new TrainingSubjectRepositoryEF(context, _mapper.Object);

        //Act
        var result = await trainingSubjectRepo.IsDuplicated("Subject2");

        //Assert
        Assert.False(result);
    }
}

 */