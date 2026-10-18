using FragranceComposer.Services;
using FragranceComposer.Repositories;
using FragranceComposer.Interfaces;
using FragranceComposer.Concentrations;

namespace FragranceComposer;

class Program
{
    static void Main(string[] args)
    {
         IIngredientRepository repository = new InMemoryIngredientRepository();
        var service = new FragranceService(repository);

        var fragrance = service.Create("Moldavian Summer", new EauDeParfum(), new()
        {
            ["Bergamot"] = 25,
            ["Rose"] = 40,
            ["Vanilla"] = 35,
        });

        new LabelPrinter().Print(fragrance);
        decimal cost = new CostCalculator().Calculate(fragrance, 50);
        Console.WriteLine($"Oil cost for 50 ml: {cost:F2} EUR");
    }
}