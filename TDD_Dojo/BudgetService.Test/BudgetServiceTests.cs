using BudgetService;
using Moq;
using System;
using Xunit;

namespace BudgetService.Test
{
    public class BudgetServiceTests
    {
        private readonly MockRepository mockRepository;

        public BudgetServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

        }

        private BudgetService CreateService()
        {
            var mockBudgetRepo = new Mock<IBudgetRepo>();
            return new BudgetService(mockBudgetRepo.Object);
        }

        [Fact]
        public void StartTimeOverThanEndTime_ReturnZero()
        {
            // Arrange

            var service = this.CreateService();
            DateTime start = new DateTime(2020, 05, 02);
            DateTime endTime = new DateTime(2020, 03, 15);

            // Act
            var result = service.Query(
                start,
                endTime);

            // Assert
            Assert.Equal(0, result);
            this.mockRepository.VerifyAll();
        }
    }
}
