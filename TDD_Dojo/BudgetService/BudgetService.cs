using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

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
                        var amount = GetMonthBudgetAmount(startTime);
                        var daysInMonth = DateTime.DaysInMonth(startTime.Year, startTime.Month);
                        var days = endTime.Day - startTime.Day + 1;
                        return amount * days / daysInMonth;
                    }
                case 1:
                    {
                        var startAmount = GetMonthBudgetAmount(startTime);
                        var startTimeDaysInMonth = DateTime.DaysInMonth(startTime.Year, startTime.Month);
                        var startResult = startAmount * (startTimeDaysInMonth - startTime.Day + 1) / startTimeDaysInMonth;

                        var endAmount = GetMonthBudgetAmount(endTime);
                        var endTimeDaysInMonth = DateTime.DaysInMonth(endTime.Year, endTime.Month);
                        var endResult = endAmount * endTime.Day / endTimeDaysInMonth;

                        return startResult + endResult;
                    }
                default:
                    {
                        var startAmount = GetMonthBudgetAmount(startTime);
                        var startTimeDaysInMonth = DateTime.DaysInMonth(startTime.Year, startTime.Month);
                        var startResult = startAmount * (startTimeDaysInMonth - startTime.Day + 1) / startTimeDaysInMonth;

                        // full month
                        decimal sum = 0;
                        var month = startTime.Date.AddMonths(1);
                        while (month.Month < endTime.Month)
                        {
                            sum += GetMonthBudgetAmount(month);
                            month = month.AddMonths(1);
                        }

                        var endAmount = GetMonthBudgetAmount(endTime);
                        var endTimeDaysInMonth = DateTime.DaysInMonth(endTime.Year, endTime.Month);
                        var endResult = endAmount * endTime.Day / endTimeDaysInMonth;

                        return startResult + sum + endResult;
                    }
            }

        }

        public decimal GetMonthBudgetAmount(DateTime dateTime)
        {
            var budget = _budgetRepo.GetAll().FirstOrDefault(b => b.YearMonth == dateTime.ToString("yyyyMM"));
            return budget?.Amount ?? 0;
        }

        public decimal GetPercentageFromMonthStart(DateTime dateTime)
        {
            var daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            return (decimal)dateTime.Day / daysInMonth;
        }

        public decimal GetPercentageFromMonthEnd(DateTime dateTime)
        {
            var daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            return (decimal)(daysInMonth - dateTime.Day + 1) / daysInMonth;
        }

        public decimal GetPercentageFromTwoDates(DateTime startTime, DateTime endTime)
        {
            var daysInMonth = DateTime.DaysInMonth(startTime.Year, startTime.Month);
            return (decimal)(endTime.Day - startTime.Day + 1) / daysInMonth;
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
