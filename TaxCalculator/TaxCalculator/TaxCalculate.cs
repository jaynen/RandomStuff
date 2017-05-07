using System;
using System.Collections.Generic;
using System.Linq;

namespace TaxCalculator
{
    public class TaxCalculate
    {
        private class TaxBracket
        {
            public decimal Low { get; set; }
            public decimal High { get; set; }
            public decimal Rate { get; set; }
        }

        private class TaxReliefBracket
        {
            public decimal Min { get; set; }
            public decimal Max { get; set; }
            public decimal Discount { get; set; }
        }

        private readonly List<TaxBracket> _taxBrackets;
        private readonly List<TaxReliefBracket> _taxReliefBrackets;

        public TaxCalculate()
        {
            // initialize the tax brackets
            // remember these brackets are based on MONTHLY amounts
            // keep the ascending order of amounts or the calculation won't work
            _taxBrackets = new List<TaxBracket>
            {
                new TaxBracket {Low = 0m, High = 4999m, Rate = 0m},
                new TaxBracket {Low = 5000m, High = 10000m, Rate = 0.05m},
                new TaxBracket {Low = 10001m, High = 20000m, Rate = 0.075m},
                new TaxBracket {Low = 20001m, High = 35000m, Rate = 0.09m},
                new TaxBracket {Low = 35001m, High = 50000m, Rate = 0.15m},
                new TaxBracket {Low = 50001m, High = 70000m, Rate = 0.25m},
                new TaxBracket {Low = 70001m, High = decimal.MaxValue, Rate = 0.3m}
            };

            // initialize the tax relief brackets
            // remember these brackets are based on MONTHLY amounts
            _taxReliefBrackets = new List<TaxReliefBracket>
            {
                new TaxReliefBracket {Min = 18, Max = 50, Discount = 2000m*1m},
                new TaxReliefBracket {Min = 51, Max = int.MaxValue, Discount = 5000m*0.85m},
            };
        }

        /// <summary>
        /// calculates the tax owed based on age and salary
        /// </summary>
        public void CalculateTax(TaxViewModel viewModel)
        {
            var taxDiscount = _taxReliefBrackets.SingleOrDefault(x => viewModel.Age >= x.Min && viewModel.Age <= x.Max);

            viewModel.MonthlyTaxRelief = taxDiscount == null ? 0m : taxDiscount.Discount;

            // TODO: do we apply the tax relief before or after computing the tax owed?
            var monthlySalary = viewModel.MonthlySalary - viewModel.MonthlyTaxRelief;
            viewModel.MonthlyTaxAmount = 0m;

            // we could LINQ this but we'd lose some readability...
            foreach (var bracket in _taxBrackets)
            {
                if (monthlySalary <= bracket.Low) break;
                var taxableAtThisRate = Math.Min(bracket.High - bracket.Low, monthlySalary - bracket.Low);
                var taxThisBand = taxableAtThisRate * bracket.Rate;
                viewModel.MonthlyTaxAmount += taxThisBand;
            }
        }
    }
}
