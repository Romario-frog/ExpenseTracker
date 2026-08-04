namespace ExpenseTracker.Models;

public class Transaction
{
    public DateTime Date { get; set; } = DateTime.Now;
    public string Type { get; set; } = "Expense";
    public string Category { get; set; } = "Other";
    public decimal Amount { get; set; }
    public string Comment { get; set; } = string.Empty;

    public string AmountText => Type == "Income" ? $"+${Amount:0.00}" : $"-${Amount:0.00}";
}
