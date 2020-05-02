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
        private IBudgetRepo _budgetRepo;
        public BudgetService(IBudgetRepo budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        public decimal Query(DateTime startTime, DateTime endTime)
        {
            if (startTime > endTime) return 0;

            var monthDiff = endTime.Month - startTime.Month;

            if (monthDiff == 0)
            {
                var amount = GetMonthBudgetAmount(startTime);
                var percentage = GetPercentageFromTwoDates(startTime, endTime);
                return (decimal)(amount * (decimal)percentage);
            }
            else if (monthDiff == 1)
            {
                var startAmount = GetMonthBudgetAmount(startTime);
                var startPercentage = GetPercentageFromMonthEnd(startTime);
                var startResult = (decimal)(startAmount * (decimal)startPercentage);

                var endAmount = GetMonthBudgetAmount(endTime);
                var endPercentage = GetPercentageFromMonthEnd(endTime);
                var endResult = (decimal)(endAmount * (decimal)endPercentage);

                return startResult + endResult;
            }
            else if (monthDiff > 1)
            {
                var startAmount = GetMonthBudgetAmount(startTime);
                var startPercentage = GetPercentageFromMonthEnd(startTime);
                var startResult = (decimal)(startAmount * (decimal)startPercentage);

                // full month
                decimal sum = 0;
                var month = startTime.Date.AddMonths(1);
                while (month.Month < endTime.Month)
                {
                    sum += GetMonthBudgetAmount(month);
                    month = month.AddMonths(1);
                }

                var endAmount = GetMonthBudgetAmount(endTime);
                var endPercentage = GetPercentageFromMonthEnd(endTime);
                var endResult = (decimal)(endAmount * (decimal)endPercentage);

                return startResult + sum + endResult;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }


        }

        public decimal GetMonthBudgetAmount(DateTime dateTime)
        {
            var budget = _budgetRepo.GetAll().FirstOrDefault(b => b.YearMonth == dateTime.ToString("yyyyMM"));
            return budget?.Amount ?? 0;
        }

        public double GetPercentageFromMonthStart(DateTime dateTime)
        {
            var daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            return dateTime.Day / daysInMonth;
        }

        public double GetPercentageFromMonthEnd(DateTime dateTime)
        {
            var daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            return (daysInMonth - dateTime.Day + 1) / daysInMonth;
        }

        public double GetPercentageFromTwoDates(DateTime startTime, DateTime endTime)
        {
            var daysInMonth = DateTime.DaysInMonth(startTime.Year, startTime.Month);
            return (endTime.Day - startTime.Day + 1) / daysInMonth;
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
