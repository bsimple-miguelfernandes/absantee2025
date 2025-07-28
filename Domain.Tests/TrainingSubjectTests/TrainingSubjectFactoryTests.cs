using System;
using System.Threading.Tasks;
using Domain.Factory;
using Domain.Models;
using Domain.Visitor;
using Moq;
using Xunit;

namespace Domain.Tests.TrainingSubjectTests
{
    public class TrainingSubjectFactoryPureTests
    {
        [Fact]
        public async Task WhenPassingValidData_ThenTrainingSubjectIsCreated()
        {
            // Arrange
            var factory = new TrainingSubjectFactory();
            var id = Guid.NewGuid();
            var subject = "Test Subject";
            var description = "Test Description";

            // Act
            var result = await factory.Create(id, subject, description);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(subject, result.Subject);
            Assert.Equal(description, result.Description);
        }

        [Fact]
        public void WhenUsingVisitor_ThenTrainingSubjectIsCreated()
        {
            // Arrange
            var visitor = new Mock<ITrainingSubjectVisitor>();
            var id = Guid.NewGuid();
            var subject = "Visitor Subject";
            var description = "Visitor Description";

            visitor.Setup(v => v.Id).Returns(id);
            visitor.Setup(v => v.Subject).Returns(subject);
            visitor.Setup(v => v.Description).Returns(description);

            var factory = new TrainingSubjectFactory();

            // Act
            var result = factory.Create(visitor.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(subject, result.Subject);
            Assert.Equal(description, result.Description);
        }
    }
}
