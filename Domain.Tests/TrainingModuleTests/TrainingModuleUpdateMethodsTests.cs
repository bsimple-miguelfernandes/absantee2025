using System;
using System.Collections.Generic;
using Domain.Models;
using Xunit;

namespace Domain.Tests.TrainingModuleTests
{
    public class TrainingModuleUpdateMethodsTests
    {
        private TrainingModule CreateValidTrainingModule()
        {
            var periods = new List<PeriodDateTime>
            {
                new PeriodDateTime(DateTime.Now.AddDays(2), DateTime.Now.AddDays(5)),
                new PeriodDateTime(DateTime.Now.AddDays(6), DateTime.Now.AddDays(9))
            };

            return new TrainingModule(Guid.NewGuid(), periods);
        }

        [Fact]
        public void UpdateTrainingSubjectId_WithValidId_UpdatesSuccessfully()
        {
            // Arrange
            var trainingModule = CreateValidTrainingModule();
            var newSubjectId = Guid.NewGuid();

            // Act
            trainingModule.UpdateTrainingSubjectId(newSubjectId);

            // Assert
            Assert.Equal(newSubjectId, trainingModule.TrainingSubjectId);
        }

        [Fact]
        public void UpdateTrainingSubjectId_WithEmptyGuid_ThrowsException()
        {
            // Arrange
            var trainingModule = CreateValidTrainingModule();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                trainingModule.UpdateTrainingSubjectId(Guid.Empty));

            Assert.Equal("Training subject ID cannot be empty", exception.Message);
        }

        [Fact]
        public void UpdatePeriods_WithValidPeriods_UpdatesSuccessfully()
        {
            // Arrange
            var trainingModule = CreateValidTrainingModule();

            var newPeriods = new List<PeriodDateTime>
            {
                new PeriodDateTime(DateTime.Now.AddDays(10), DateTime.Now.AddDays(12)),
                new PeriodDateTime(DateTime.Now.AddDays(13), DateTime.Now.AddDays(15))
            };

            // Act
            trainingModule.UpdatePeriods(newPeriods);

            // Assert
            Assert.Equal(newPeriods, trainingModule.Periods);
        }

        [Fact]
        public void UpdatePeriods_WithNull_ThrowsException()
        {
            // Arrange
            var trainingModule = CreateValidTrainingModule();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                trainingModule.UpdatePeriods(null));

            Assert.Equal("Periods cannot be null or empty", exception.Message);
        }

        [Fact]
        public void UpdatePeriods_WithEmptyList_ThrowsException()
        {
            // Arrange
            var trainingModule = CreateValidTrainingModule();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                trainingModule.UpdatePeriods(new List<PeriodDateTime>()));

            Assert.Equal("Periods cannot be null or empty", exception.Message);
        }
    }
}
