using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BudgetService
{
    public class BudgetService
    {
        public decimal Query(DateTime start, DateTime endTime)
        {
            return 0;
        }
    }

    public class Budget
    {
        public string YearMonth { get; set; }
        public decimal Amount { get; set; }
    }


    public interface IBudgetRepo
    {
        IEnumerable<Budget> GetAll();
    }

    public class BudgetRepo : IBudgetRepo
    {
        public IEnumerable<Budget> GetAll()
        {
            return new List<Budget>
            {
                new Budget(){YearMonth = "202005", Amount = 31000}
            };
        }
    }
}
