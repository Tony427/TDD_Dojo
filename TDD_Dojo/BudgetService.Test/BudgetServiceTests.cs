using BudgetService;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace BudgetService.Test
{
    public class BudgetServiceTests
    {
        private MockRepository _mockRepository;
        private Mock<IBudgetRepo> _mockBudgetRepo;

        public BudgetServiceTests()
        {

        }

        [Fact]
        public void StartTimeOverThanEndTime_ReturnZero()
        {
            // Arrange
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockBudgetRepo = _mockRepository.Create<IBudgetRepo>();

            var service = new BudgetService(_mockBudgetRepo.Object);
            DateTime start = new DateTime(2020, 05, 02);
            DateTime endTime = new DateTime(2020, 03, 15);

            // Act
            var result = service.Query(
                start,
                endTime);

            // Assert
            Assert.Equal(0, result);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void SameDay()
        {
            // Arrange
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockBudgetRepo = _mockRepository.Create<IBudgetRepo>();
            _mockBudgetRepo.Setup(x => x.GetAll()).Returns(new List<Budget>
            {
                new Budget(){YearMonth = "202004", Amount = 30}
            });

            var service = new BudgetService(_mockBudgetRepo.Object);
            DateTime start = new DateTime(2020, 04, 01);
            DateTime endTime = new DateTime(2020, 04, 01);

            // Act
            var result = service.Query(
                start,
                endTime);

            // Assert
            Assert.Equal(1, result);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void SameMonthMultipleDay()
        {
            // Arrange
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockBudgetRepo = _mockRepository.Create<IBudgetRepo>();
            _mockBudgetRepo.Setup(x => x.GetAll()).Returns(new List<Budget>
            {
                new Budget(){YearMonth = "202004", Amount = 30}
            });

            var service = new BudgetService(_mockBudgetRepo.Object);
            DateTime start = new DateTime(2020, 04, 01);
            DateTime endTime = new DateTime(2020, 04, 03);

            // Act
            var result = service.Query(
                start,
                endTime);

            // Assert
            Assert.Equal(3, result);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void DifferentMonthMultipleDay()
        {
            // Arrange
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockBudgetRepo = _mockRepository.Create<IBudgetRepo>();
            _mockBudgetRepo.Setup(x => x.GetAll()).Returns(new List<Budget>
            {
                new Budget(){YearMonth = "202004", Amount = 30},
                new Budget(){YearMonth = "202005", Amount = 310}
            });

            var service = new BudgetService(_mockBudgetRepo.Object);
            DateTime start = new DateTime(2020, 04, 15);
            DateTime endTime = new DateTime(2020, 05, 25);

            // Act
            var result = service.Query(
                start,
                endTime);

            // Assert
            Assert.Equal(266, result);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void CrossMultipleMonths()
        {
            // Arrange
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockBudgetRepo = _mockRepository.Create<IBudgetRepo>();
            _mockBudgetRepo.Setup(x => x.GetAll()).Returns(new List<Budget>
            {
                new Budget(){YearMonth = "202003", Amount = 3100},
                new Budget(){YearMonth = "202004", Amount = 30},
                new Budget(){YearMonth = "202005", Amount = 310}
            });

            var service = new BudgetService(_mockBudgetRepo.Object);
            DateTime start = new DateTime(2020, 03, 31);
            DateTime endTime = new DateTime(2020, 05, 1);

            // Act
            var result = service.Query(
                start,
                endTime);

            // Assert
            Assert.Equal(140, result);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void MissingMonthData()
        {
            // Arrange
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockBudgetRepo = _mockRepository.Create<IBudgetRepo>();
            _mockBudgetRepo.Setup(x => x.GetAll()).Returns(new List<Budget>
            {
                new Budget(){YearMonth = "202003", Amount = 3100},
                new Budget(){YearMonth = "202005", Amount = 310}
            });

            var service = new BudgetService(_mockBudgetRepo.Object);
            DateTime start = new DateTime(2020, 03, 31);
            DateTime endTime = new DateTime(2020, 05, 1);

            // Act
            var result = service.Query(
                start,
                endTime);

            // Assert
            Assert.Equal(110, result);
            _mockRepository.VerifyAll();
        }

    }
}
