// Jamie Chadwick 000075123 Currency Converter
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Calculator
{
	public sealed partial class CurrencyConverter : Page
	{
		// Constants = conversion rates
		// USD conversion
		const double USD_TO_EUR = 0.85189982;
		const double USD_TO_GBP = 0.72872436;
		const double USD_TO_INR = 74.257327;

		// EUR conversion
		const double EUR_TO_USD = 1.1739732;
		const double EUR_TO_GBP = 0.8556672;
		const double EUR_TO_INR = 87.00755;

		// GBP conversion
		const double GBP_TO_USD = 1.371907;
		const double GBP_TO_EUR = 1.1686692;
		const double GBP_TO_INR = 101.68635;

		// INR conversion
		const double INR_TO_USD = 0.011492628;
		const double INR_TO_EUR = 0.013492774;
		const double INR_TO_GBP = 0.0098339397;

		public CurrencyConverter()
		{
			InitializeComponent();
		}

		//  set window size and title bar 
		private void pageLoaded(object sender, RoutedEventArgs e)
		{
			ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(320, 480));

			CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

			ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
			titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
			titleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Transparent;
			titleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.White;
		}

		// Get currency code tag from ComboBox
		private string getSelectedCurrency(ComboBox combo)
		{
			ComboBoxItem selectedItem = (ComboBoxItem)combo.SelectedItem;

			if (selectedItem == null)
				return "";

			return selectedItem.Tag.ToString();
		}

		// Error dialog
		private void showError(string message)
		{
			ContentDialog errorDialog = new ContentDialog
			{
				Title = "Input Error",
				Content = message,
				CloseButtonText = "OK"
			};

			_ = errorDialog.ShowAsync();
		}

		// Get the conversion rate based on from and to currency
		private double getConversionRate(string fromCurrency, string toCurrency)
		{
			// USD
			if (fromCurrency == "USD" && toCurrency == "EUR")
				return USD_TO_EUR;
			else if (fromCurrency == "USD" && toCurrency == "GBP")
				return USD_TO_GBP;
			else if (fromCurrency == "USD" && toCurrency == "INR")
				return USD_TO_INR;

			// EUR
			else if (fromCurrency == "EUR" && toCurrency == "USD")
				return EUR_TO_USD;
			else if (fromCurrency == "EUR" && toCurrency == "GBP")
				return EUR_TO_GBP;
			else if (fromCurrency == "EUR" && toCurrency == "INR")
				return EUR_TO_INR;

			// GBP
			else if (fromCurrency == "GBP" && toCurrency == "USD")
				return GBP_TO_USD;
			else if (fromCurrency == "GBP" && toCurrency == "EUR")
				return GBP_TO_EUR;
			else if (fromCurrency == "GBP" && toCurrency == "INR")
				return GBP_TO_INR;

			// INR
			else if (fromCurrency == "INR" && toCurrency == "USD")
				return INR_TO_USD;
			else if (fromCurrency == "INR" && toCurrency == "EUR")
				return INR_TO_EUR;
			else if (fromCurrency == "INR" && toCurrency == "GBP")
				return INR_TO_GBP;

			// Same currency selected
			else
				return 1.0;
		}

		// Set currency symbols 
		private string getCurrencySymbol(string currencyCode)
		{
			if (currencyCode == "USD")
				return "$";
			else if (currencyCode == "EUR")
				return "€";
			else if (currencyCode == "GBP")
				return "£";
			else if (currencyCode == "INR")
				return "₹";
			else
				return "";
		}

		// validate input and calculate result
		private void convertButton_Click(object sender, RoutedEventArgs e)
		{
			double amount;
			double conversionRate;
			double convertedAmount;
			string fromCurrency;
			string toCurrency;
			string fromSymbol;
			string toSymbol;

			// Validate amount is not empty
			if (string.IsNullOrWhiteSpace(amountInput.Text))
			{
				showError("Please enter an amount to convert.");
				amountInput.Focus(FocusState.Programmatic);
				return;
			}

			// Try to parse the amount
			try
			{
				amount = double.Parse(amountInput.Text);
			}
			catch
			{
				showError("Please enter a valid number for the amount.");
				amountInput.Focus(FocusState.Programmatic);
				amountInput.SelectAll();
				return;
			}

			// Validate amount is greater than 0
			if (amount <= 0)
			{
				showError("Amount must be greater than 0.");
				amountInput.Focus(FocusState.Programmatic);
				amountInput.SelectAll();
				return;
			}

			// Get selected currencies
			fromCurrency = getSelectedCurrency(fromCurrencyCombo);
			toCurrency = getSelectedCurrency(toCurrencyCombo);

			// Check currencies were selected
			if (fromCurrency == "" || toCurrency == "")
			{
				showError("Please select both a From and To currency.");
				return;
			}

			// Check currency isnt the same
			if (fromCurrency == toCurrency)
			{
				showError("From and To currencies cannot be the same.");
				return;
			}

			// Get conversion rate and calculate result
			conversionRate = getConversionRate(fromCurrency, toCurrency);
			convertedAmount = amount * conversionRate;

			// Get currency symbols for display
			fromSymbol = getCurrencySymbol(fromCurrency);
			toSymbol = getCurrencySymbol(toCurrency);

			// Display result
			resultDisplay.Text = fromSymbol + amount.ToString("F2")
								+ "  =  "
								+ toSymbol + convertedAmount.ToString("F4");
		}
		// Back button - navigate back to menu
		private void backButton_Click(object sender, RoutedEventArgs e)
		{
			if (Frame.CanGoBack)
				Frame.GoBack();
			else
				Frame.Navigate(typeof(MainPage));
		}
	}
}
