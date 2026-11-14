using FragranceComposer.Interfaces;
namespace FragranceComposer.Services;

public class ScentPrinter
{
    public void Print(IScent scent)
    {
        Console.WriteLine($"=== {scent.Name} ===");
        foreach (var note in scent.Composition.OrderBy(n=> n.Key.Layer).ThenByDescending(n=> n.Value))
        Console.WriteLine($"{note.Key.Layer} note: {note.Key.Name} {note.Value:F1}%");
        Console.WriteLine($"Composition total: {scent.Composition.Values.Sum():F1}%");
        Console.WriteLine($"Cost per ml: {scent.CostPerMl:F4} EUR\n");
    }
}