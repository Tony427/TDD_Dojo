using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;

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

            var data = _budgetRepo.GetAll().ToList();
            var days = (endTime.AddDays(1) - startTime).Days;

            double sum = 0;
            for (var date = startTime.Date; date <= endTime.Date; date = date.AddDays(1))
            {
                sum += GetBudget(date, data);
            }

            return (decimal)sum;
        }

        private double GetBudget(DateTime startTime, List<Budget> budgets)
        {
            var start = startTime.ToString("yyyyMM");
            var budget = budgets.FirstOrDefault(b => b.YearMonth == start);
            if (budget == null) return 0;

            var days = DateTime.DaysInMonth(startTime.Year, startTime.Month);

            return (double)budget.Amount / days;
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
