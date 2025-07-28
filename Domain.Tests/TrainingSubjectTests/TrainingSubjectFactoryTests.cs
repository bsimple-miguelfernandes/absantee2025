/* using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Factory;
using Domain.Factory.TrainingPeriodFactory;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Domain.Tests.TrainingSubjectTests
{
    public class TrainingSubjectFactoryTests
    {
        [Fact]
        public void WhenPassingValidDataAndIsUnique_ThenTrainingSubjectIsCreated()
        {
            //Arrange
            var repository = new Mock<ITrainingSubjectRepository>();
            repository.Setup(r => r.IsDuplicated(It.IsAny<string>())).ReturnsAsync(false);

            var factory = new TrainingSubjectFactory(repository.Object);
            //act
            var result = factory.Create("Subject", "Description");

            //Arrange
            Assert.NotNull(result);
        }

        [Fact]
        public async Task WhenPassingValidDataAndIsDuplicated_ThenThrowsException()
        {
            //Arrange
            var repository = new Mock<ITrainingSubjectRepository>();
            repository.Setup(r => r.IsDuplicated(It.IsAny<string>())).ReturnsAsync(true);

            var factory = new TrainingSubjectFactory(repository.Object);
            //Arrange
            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                // act
                factory.Create("Subject", "Description")
            );
            Assert.Equal("Subject must be unique", exception.Message );
        }
    }
}
 */