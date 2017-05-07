using System;
using System.ComponentModel;

namespace TaxCalculator
{
    public class TaxViewModel : INotifyPropertyChanged
    {
        #region Variables
        private int _age;
        private int _annualSalary;
        private string _result;
        private decimal _monthlyTaxAmount;
        private decimal _monthlyTaxRelief;
        #endregion

        #region Properties
        public decimal MonthlyTaxRelief
        {
            get { return _monthlyTaxRelief; }
            set { _monthlyTaxRelief = Math.Round(value, 2); }
        }

        public decimal MonthlyTaxAmount
        {
            get { return _monthlyTaxAmount; }
            set { _monthlyTaxAmount = Math.Round(value, 2); }
        }

        public decimal AnnualTaxAmount
        {
            get { return Math.Round(MonthlyTaxAmount * 12m, 2); }
        }

        public decimal MonthlySalary
        {
            get { return Math.Round(Convert.ToDecimal(AnnualSalary) / 12m, 2); }
        }

        public int Age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChanged("Age");
            }
        }

        public int AnnualSalary
        {
            get { return _annualSalary; }
            set
            {
                _annualSalary = value;
                OnPropertyChanged("AnnualSalary");
            }
        }

        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged("Result");
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// formats the result (output) based on the current data
        /// </summary>
        public void FormatResult()
        {
            Result = string.Format(
                "Age: {0}\n" +
                "Annual Salary: {1}\n" +
                "Monthly Salary: {2}\n" +
                "Monthly Tax Due: {3}\n" +
                "Monthly Tax Relief: {4}\n" +
                "Annual Tax Due: {5}",
                Age,
                AnnualSalary,
                MonthlySalary,
                MonthlyTaxAmount,
                MonthlyTaxRelief,
                AnnualTaxAmount);
        }
        #endregion

        #region Databinding - Property Changes
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}