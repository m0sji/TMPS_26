using FragranceComposer.Models;
using FragranceComposer.Interfaces;
namespace FragranceComposer.Services;

public class LabelPrinter
{
    public void Print(Fragrance fragrance)
    {
        Console.WriteLine($"==={fragrance.Name}({fragrance.Concentration.Name},{fragrance.Family})===");
        foreach (var f in fragrance.Formula)
        Console.WriteLine($"{f.Key.Layer} note: {f.Key.Name} {f.Value}%");
    }

    public void Print(IProduct product)
    {
        Console.WriteLine($"Product: {product.Description}");
        Console.WriteLine($"Total cost: {product.Cost:F2} EUR");
    }
}