using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace TaxCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new TaxViewModel();
        }

        private void CalculateOnClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TaxViewModel;

            if (viewModel != null)
            {
                try
                {
                    // perform the tax calculation
                    new TaxCalculate().CalculateTax(viewModel);
                    // format the result for viewing
                    viewModel.FormatResult();
                }
                catch (Exception ex)
                {
                    viewModel.Result = ex.Message;
                }
            }
        }

        /// <summary>
        /// makes sure that input typed into the textbox is numeric only
        /// TODO: this won't stop pasting non-numerics into the textbox
        /// </summary>
        private void NumericOnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
