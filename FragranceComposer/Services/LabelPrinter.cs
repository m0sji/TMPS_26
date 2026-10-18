using FragranceComposer.Models;
namespace FragranceComposer.Services;

public class LabelPrinter
{
    public void Print(Fragrance fragrance)
    {
        Console.WriteLine($"=== {fragrance.Name} ({fragrance.Concentration.Name}) ===");
        foreach (var f in fragrance.Formula)
            Console.WriteLine($"{f.Key.Layer} note: {f.Key.Name} {f.Value}%");
    }
}