using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BudgetService
{
    public class BudgetService
    {
        private IBudgetRepo _budgetRepo;
        public BudgetService(IBudgetRepo budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        public decimal Query(DateTime startTime, DateTime endTime)
        {
            if (startTime > endTime) return 0;

            // 1. getData
            // 2. 攤比例


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

}
