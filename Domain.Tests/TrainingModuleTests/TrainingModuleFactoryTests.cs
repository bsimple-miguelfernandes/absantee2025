using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Domain.Tests.TrainingModuleTests
{
    public class TrainingModuleFactoryTests
    {
        [Fact]
        public async Task WhenPassingValidData_ThenTrainingModuleIsCreated()
        {
            // Arrange
            var subjectId = Guid.NewGuid();
            var subject = new TrainingSubject(subjectId, "Mock Subject", "Mock Description");

            var periodDateTime1 = new PeriodDateTime(DateTime.Now.AddDays(1), DateTime.Now.AddDays(3));
            var periodDateTime2 = new PeriodDateTime(DateTime.Now.AddDays(5), DateTime.Now.AddDays(8));
            var periods = new List<PeriodDateTime>() { periodDateTime1, periodDateTime2 };

            var subjectRepository = new Mock<ITrainingSubjectRepository>();
            subjectRepository.Setup(s => s.GetByIdAsync(subjectId)).ReturnsAsync(subject);

            var moduleRepository = new Mock<ITrainingModuleRepository>();
            var factory = new TrainingModuleFactory(subjectRepository.Object, moduleRepository.Object);

            // Act
            var result = await factory.Create(subjectId, periods);

            // Assert
            Assert.NotNull(result);
        }


        [Fact]
        public async Task WhenTrainingSubjectDontExists_ThenThrowExeption()
        {
            //Arrange
            var subjectId = Guid.NewGuid();

            var periodDateTime1 = new PeriodDateTime(DateTime.Now.AddDays(1), DateTime.Now.AddDays(3));

            var periodDateTime2 = new PeriodDateTime(DateTime.Now.AddDays(5), DateTime.Now.AddDays(8));

            var periods = new List<PeriodDateTime>() { periodDateTime1, periodDateTime2 };

            var subjectRepository = new Mock<ITrainingSubjectRepository>();
            subjectRepository.Setup(s => s.GetByIdAsync(subjectId)).ReturnsAsync((TrainingSubject?)null);

            var moduleRepository = new Mock<ITrainingModuleRepository>();
            var factory = new TrainingModuleFactory(subjectRepository.Object, moduleRepository.Object);

            //Arrange
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                //Act
                factory.Create(subjectId, periods)
            );

            Assert.Equal("Training Subject must exists", exception.Message);
        }
    }
}
