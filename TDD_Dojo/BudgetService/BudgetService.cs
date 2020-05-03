using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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

            var monthDiff = endTime.Month - startTime.Month;

            switch (monthDiff)
            {
                case 0:
                    {
                        var budget = GetMonthBudget(startTime);
                        var days = endTime.Day - startTime.Day + 1;
                        return budget.CountAmountByTotalDays(days);
                    }
                case 1:
                    {
                        var startBudget = GetMonthBudget(startTime);
                        var startAmount = startBudget.CountAmountByDaysFromMonthEnd(startTime.Day);

                        var endBudget = GetMonthBudget(endTime);
                        var endAmount = endBudget.CountAmountByDaysFromMonthStart(endTime.Day);

                        return startAmount + endAmount;
                    }
                default:
                    {

                        var startBudget = GetMonthBudget(startTime);
                        var startAmount = startBudget.CountAmountByDaysFromMonthEnd(startTime.Day);

                        // full month
                        decimal sum = 0;
                        var month = startTime.Date.AddMonths(1);
                        while (month.Month < endTime.Month)
                        {
                            var budget = GetMonthBudget(month);
                            sum += budget?.Amount ?? 0;
                            month = month.AddMonths(1);
                        }

                        var endBudget = GetMonthBudget(endTime);
                        var endAmount = endBudget.CountAmountByDaysFromMonthStart(endTime.Day);

                        return startAmount + sum + endAmount;
                    }
            }

        }

        public Budget GetMonthBudget(DateTime dateTime)
            => _budgetRepo.GetAll().FirstOrDefault(b => b.YearMonth == dateTime.ToString("yyyyMM"));
    }

    public class Budget
    {
        public string YearMonth { get; set; }
        public decimal Amount { get; set; }

        public DateTime CurrentMonthAsDateTime
        {
            get
            {
                return DateTime.ParseExact(YearMonth, "yyyyMM", CultureInfo.CurrentCulture);
            }
        }

        public int CurrentDaysInMonth
        {
            get
            {
                return DateTime.DaysInMonth(CurrentMonthAsDateTime.Year, CurrentMonthAsDateTime.Month);
            }
        }

        public decimal CountAmountByTotalDays(int days)
            => Amount * days / CurrentDaysInMonth;

        public decimal CountAmountByDaysFromMonthStart(int days)
            => Amount * days / CurrentDaysInMonth;

        public decimal CountAmountByDaysFromMonthEnd(int days)
            => Amount * (CurrentDaysInMonth - days + 1) / CurrentDaysInMonth;
    }

    public interface IBudgetRepo
    {
        IEnumerable<Budget> GetAll();
    }
}
