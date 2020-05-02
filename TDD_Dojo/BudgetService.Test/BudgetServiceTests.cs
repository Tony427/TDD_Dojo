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
            return new BudgetService();
        }

        [Fact]
        public void Query_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            DateTime start = default(global::System.DateTime);
            DateTime endTime = default(global::System.DateTime);

            // Act
            var result = service.Query(
                start,
                endTime);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }
    }
}
