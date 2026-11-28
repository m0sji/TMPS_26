using FragranceComposer.Models;
namespace FragranceComposer.Observers;

public class AuditLogObserver : IOrderObserver
{
    private readonly List<string> _entries = new();
    public IReadOnlyList<string> Entries => _entries;

    public void OnStateChanged(ProductionOrder order, string previousState)
    {
        string entry = $"{order.Id}: {previousState} -> {order.StateName}";
        _entries.Add(entry);
        Console.WriteLine($"   [audit] {entry}");
    }
}