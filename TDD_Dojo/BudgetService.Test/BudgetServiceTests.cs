using BudgetService;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BudgetService.Test
{
    public class BudgetServiceTests
    {
        private MockRepository _mockRepository;
        private Mock<IBudgetRepo> _mockBudgetRepo;

        private BudgetService _budgetService;

        public BudgetServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockBudgetRepo = _mockRepository.Create<IBudgetRepo>();
        }

        internal void GivenBudgets(List<Budget> budgets)
        {
            if (budgets != null && budgets.Any()) _mockBudgetRepo.Setup(x => x.GetAll()).Returns(budgets);
            _budgetService = new BudgetService(_mockBudgetRepo.Object);
        }

        internal void ExpectedAmountShouldBe(decimal expectedAmount, DateTime start, DateTime end)
        {
            var result = _budgetService.Query(start, end);

            Assert.Equal(expectedAmount, result);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void StartTimeOverThanEndTime_ReturnZero()
        {
            GivenBudgets(new List<Budget>());

            ExpectedAmountShouldBe(0, new DateTime(2020, 05, 02), new DateTime(2020, 03, 15));
        }

        [Fact]
        public void SameDay()
        {
            GivenBudgets(new List<Budget>
            {
                new Budget(){YearMonth = "202004", Amount = 30}
            });

            ExpectedAmountShouldBe(1, new DateTime(2020, 04, 01), new DateTime(2020, 04, 01));
        }

        [Fact]
        public void SameMonthMultipleDay()
        {
            GivenBudgets(new List<Budget>
            {
                new Budget(){YearMonth = "202004", Amount = 30}
            });

            ExpectedAmountShouldBe(3, new DateTime(2020, 04, 01), new DateTime(2020, 04, 03));
        }

        [Fact]
        public void DifferentMonthMultipleDay()
        {
            GivenBudgets(new List<Budget>
            {
                new Budget(){YearMonth = "202004", Amount = 30},
                new Budget(){YearMonth = "202005", Amount = 310}
            });

            ExpectedAmountShouldBe(22, new DateTime(2020, 04, 29), new DateTime(2020, 05, 02));
        }

        [Fact]
        public void CrossMultipleMonths()
        {
            GivenBudgets(new List<Budget>
            {
                new Budget(){YearMonth = "202003", Amount = 3100},
                new Budget(){YearMonth = "202004", Amount = 30},
                new Budget(){YearMonth = "202005", Amount = 310}
            });

            ExpectedAmountShouldBe(140, new DateTime(2020, 03, 31), new DateTime(2020, 05, 01));
        }

        [Fact]
        public void MissingMonthData()
        {
            GivenBudgets(new List<Budget>
            {
                new Budget(){YearMonth = "202003", Amount = 3100},
                new Budget(){YearMonth = "202005", Amount = 310}
            });

            ExpectedAmountShouldBe(110, new DateTime(2020, 03, 31), new DateTime(2020, 05, 01));
        }

    }
}
