using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace Domain.Tests.PeriodDateTests
{
    public class PeriodDateContainsDateTests
    {
        public static IEnumerable<object[]> ContainingDate()
        {
            yield return new object[] { new DateOnly(2020, 1, 1) };
            yield return new object[] { new DateOnly(2021, 1, 1) };
            yield return new object[] { new DateOnly(2020, 1, 2) };
        }


        [Theory]
        [MemberData(nameof(ContainingDate))]
        public void WhenPassingContainingDate_ThenReturnTrue(DateOnly containedDate)
        {
            //arrange
            DateOnly initDate = new DateOnly(2020, 1, 1);
            DateOnly finalDate = new DateOnly(2021, 1, 1);

            PeriodDate periodDate = new PeriodDate(initDate, finalDate);

            //act
            var result = periodDate.ContainsDate(containedDate);

            //assert
            Assert.True(result);
        }

        public static IEnumerable<object[]> NonContainingDate()
        {
            yield return new object[] { new DateOnly(2019, 1, 1) };
            yield return new object[] { new DateOnly(2022, 1, 1) };
        }


        [Theory]
        [MemberData(nameof(NonContainingDate))]
        public void WhenPassingNonContainingDate_ThenReturnFalse(DateOnly nonContainedDate)
        {
            //arrange
            DateOnly initDate = new DateOnly(2020, 1, 1);
            DateOnly finalDate = new DateOnly(2021, 1, 1);

            PeriodDate periodDate = new PeriodDate(initDate, finalDate);

            //act
            var result = periodDate.ContainsDate(nonContainedDate);

            //assert
            Assert.False(result);
        }
    }
}
