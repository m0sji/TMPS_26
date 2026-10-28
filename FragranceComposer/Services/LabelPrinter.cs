using System.Net.Http.Headers;
using FragranceComposer.Models;
namespace FragranceComposer.Services;

public class LabelPrinter
{
    public void Print(Fragrance fragrance)
    {
        Console.WriteLine($"==={fragrance.Name}({fragrance.Concentration.Name},{fragrance.Family})===");
        foreach (var f in fragrance.Formula)
        Console.WriteLine($"{f.Key.Layer} note: {f.Key.Name} {f.Value}%");
    }

    public void Print(Product product)
    {
        Console.WriteLine($"[{product.LineName} line]");
        Print(product.Fragrance);
        Console.WriteLine($"Bottle: {product.Bottle.Name} ({product.Bottle.VolumeMl} ml)");
        Console.WriteLine($"Packaging: {product.Packaging.Name}");
    }
}