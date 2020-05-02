using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BudgetService
{
    public class BudgetService
    {
        private readonly IBudgetRepo _budgetRepo;
        public BudgetService(IBudgetRepo budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        public decimal Query(DateTime startTime, DateTime endTime)
        {
            if (startTime > endTime) return 0;

            var budgets = _budgetRepo.GetAll().ToList();

            decimal sum = 0;
            var date = startTime.Date;
            while (date <= endTime.Date)
            {
                sum += GetBudget(date, budgets);
                date = date.AddDays(1);
            }

            return sum;
        }

        private decimal GetBudget(DateTime startTime, IEnumerable<Budget> budgets)
        {
            var yearMonthString = startTime.ToString("yyyyMM");
            var budget = budgets.FirstOrDefault(b => b.YearMonth == yearMonthString);
            if (budget == null) return 0;

            var daysInMonth = DateTime.DaysInMonth(startTime.Year, startTime.Month);

            return budget.Amount / daysInMonth;
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
