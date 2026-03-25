using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Calculator
{
	public sealed partial class MortgageCalculator : Page
	{
		public MortgageCalculator()
		{
			this.InitializeComponent();
		}

		private void Calculate_Click(object sender, RoutedEventArgs e)
		{
			// validate principal
			if (!double.TryParse(LoanAmountBox.Text, out double principal) || principal <= 0)
			{
				RepaymentBox.Text = "Invalid loan amount";
				return;
			}

			// validate annual interest rate
			if (!double.TryParse(AnnualRateBox.Text, out double annualRate) || annualRate <= 0)
			{
				RepaymentBox.Text = "Invalid annual interest rate";
				return;
			}

			// parse years and months (at least one must be > 0)
			int years = 0;
			int months = 0;

			int.TryParse(YearsBox.Text, out years);
			int.TryParse(MonthsBox.Text, out months);

			if (years == 0 && months == 0)
			{
				RepaymentBox.Text = "Enter years or months";
				return;
			}

			// convert annual rate to monthly decimal rate
			double i = (annualRate / 100) / 12;

			// display monthly interest rate
			MonthlyRateBox.Text = i.ToString("F4");

			// total number of months
			int n = (years * 12) + months;

			// mortgage formula: M = P [ i(1 + i)^n ] / [ (1 + i)^n – 1 ]
			double numerator = i * Math.Pow(1 + i, n);
			double denominator = Math.Pow(1 + i, n) - 1;

			if (denominator == 0)
			{
				RepaymentBox.Text = "Calculation error";
				return;
			}

			double monthlyRepayment = principal * (numerator / denominator);

			// display result
			RepaymentBox.Text = monthlyRepayment.ToString("F2");
		}

		private void Back_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(MainPage));
		}
	}
}