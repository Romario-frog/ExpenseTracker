using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<Transaction> _transactions;

    public MainWindow()
    {
        InitializeComponent();

        _transactions = new ObservableCollection<Transaction>(StorageService.Load());
        TransactionsGrid.ItemsSource = _transactions;
        UpdateBalance();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        if (!decimal.TryParse(AmountBox.Text.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal amount) || amount <= 0)
        {
            MessageBox.Show("Enter a valid amount greater than 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var transaction = new Transaction
        {
            Date = DateTime.Now,
            Type = GetComboBoxText(TypeBox),
            Category = GetComboBoxText(CategoryBox),
            Amount = amount,
            Comment = CommentBox.Text.Trim()
        };

        _transactions.Insert(0, transaction);
        SaveAndRefresh();

        AmountBox.Clear();
        CommentBox.Clear();
        AmountBox.Focus();
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (TransactionsGrid.SelectedItem is not Transaction selected)
        {
            MessageBox.Show("First select a transaction from the list.", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        _transactions.Remove(selected);
        SaveAndRefresh();
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        if (_transactions.Count == 0)
            return;

        var result = MessageBox.Show("Are you sure you want to clear the entire history?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            _transactions.Clear();
            SaveAndRefresh();
        }
    }

    private void SaveAndRefresh()
    {
        StorageService.Save(_transactions);
        TransactionsGrid.Items.Refresh();
        UpdateBalance();
    }

    private void UpdateBalance()
    {
        decimal income = _transactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
        decimal expenses = _transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);
        decimal balance = income - expenses;

        BalanceText.Text = $"${balance:0.00}";
    }

    private static string GetComboBoxText(ComboBox comboBox)
    {
        return (comboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? string.Empty;
    }
}
