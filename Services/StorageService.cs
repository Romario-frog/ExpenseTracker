using System.IO;
using System.Text.Json;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services;

public static class StorageService
{
    private static readonly string FolderPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ExpenseTracker");

    private static readonly string FilePath = Path.Combine(FolderPath, "transactions.json");

    public static List<Transaction> Load()
    {
        try
        {
            if (!File.Exists(FilePath))
                return new List<Transaction>();

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
        }
        catch
        {
            return new List<Transaction>();
        }
    }

    public static void Save(IEnumerable<Transaction> transactions)
    {
        Directory.CreateDirectory(FolderPath);

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(transactions, options);
        File.WriteAllText(FilePath, json);
    }
}
