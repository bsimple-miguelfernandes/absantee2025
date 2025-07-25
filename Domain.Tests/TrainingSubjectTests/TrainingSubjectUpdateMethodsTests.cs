using System;
using Domain.Models;
using Xunit;

namespace Domain.Tests.TrainingSubjectTests
{
    public class TrainingSubjectUpdateMethodsTests
    {
        private TrainingSubject CreateValidTrainingSubject()
        {
            return new TrainingSubject("CSharp Basics", "Introduction to CSharp fundamentals");
        }

        [Fact]
        public void UpdateSubject_WithValidSubject_UpdatesSuccessfully()
        {
            var subject = CreateValidTrainingSubject();
            string newSubject = "Advanced CSharp";

            subject.UpdateSubject(newSubject);

            Assert.Equal(newSubject, subject.Subject);
        }

        [Fact]
        public void UpdateDescription_WithValidDescription_UpdatesSuccessfully()
        {
            var subject = CreateValidTrainingSubject();
            string newDescription = "Deep dive into advanced CSharp features";

            subject.UpdateDescription(newDescription);

            Assert.Equal(newDescription, subject.Description);
        }

    }
}
