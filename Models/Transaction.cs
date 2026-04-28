namespace ExpenseTracker.Models;

public class Transaction
{
    public DateTime Date { get; set; } = DateTime.Now;
    public string Type { get; set; } = "Витрата";
    public string Category { get; set; } = "Інше";
    public decimal Amount { get; set; }
    public string Comment { get; set; } = string.Empty;

    public string AmountText => Type == "Дохід" ? $"+{Amount:0.00} грн" : $"-{Amount:0.00} грн";
}
