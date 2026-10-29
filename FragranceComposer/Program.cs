using FragranceComposer.Builders;
using FragranceComposer.Models;
using FragranceComposer.Services;
using FragranceComposer.Repositories;
using FragranceComposer.Interfaces;
using FragranceComposer.Concentrations;
using FragranceComposer.Factories;
using FragranceComposer.Prototypes;


namespace FragranceComposer;

class Program
{
    static void Main(string[] args)
    {
       IIngredientRepository repository = new InMemoryIngredientRepository();
       var builder = new FragranceBuilder(repository);
       var director = new FragranceDirector(builder);
       var printer = new LabelPrinter();
       var costCalculator = new ProductCostCalculator(new CostCalculator());


       //1. Builder used directly
       var summer = builder
       .WithName("Moldavian Summer")
       .WithFamily(ScentFamily.Floral)
       .WithConcentration(new EauDeParfum())
       .AddNote("Bergamot", 25)
       .AddNote("Rose", 40)
       .AddNote("Vanilla", 35)
       .Build();
       printer.Print(summer);
       Console.WriteLine();


//2 Abstract Factory: the same receipt in 3 product lines
IProductLineFactory[] lines=
        {
            new LuxuryLineFactory(),
            new ClassicLineFactory(),
            new TravelLineFactory()
        };

        foreach (var line in lines)
        {
            var fragrance = director.BuildAquatic ("Black Sea Breeze", line.CreateConcentration());
            var product = new Product(line.LineName, fragrance, line.CreateBottle(),line.CreatePackaging());
            printer.Print(product);
            Console.WriteLine($"Total cost: {costCalculator.Calculate(product):F2} EUR\n");
        }


//3 Validation: this formula adds only to 90%
try
        {
            builder.WithName("Broken")
            .WithFamily(ScentFamily.Citrus)
            .WithConcentration(new EauDeToilette())
            .AddNote("Lemon", 50)
            .AddNote("Musk", 40)
            .Build();
        }

    catch (InvalidOperationException ex)
        {
            Console.WriteLine($"BUILD REJECTED: {ex.Message}");
        }


//4.Prototype: register originals, create variants by cloninig
Console.WriteLine("\n--- Prototype ---");
var registry = new FragranceRegistry();
registry.Register("summer", summer);
registry.Register("vanilla-night", director.BuildGourmand("Vanilla Night", new EauDeParfum()));

var summerCopy = registry.Get("summer");
Console.WriteLine($"Same object as original? {ReferenceEquals(summer,summerCopy)}");
Console.WriteLine($"Same formula instance? {ReferenceEquals(summer.Formula, summerCopy.Formula)}");

var summerIntense = registry.Get("summer").CloneAs("Moldavian Summer Intense", new Parfum());
printer.Print(summerIntense);
Console.WriteLine($"Original unchanged: {summer.Name} ({summer.Concentration.Name})\n");

//All 3 patterns together: cloned variant packaged by a product line
var luxury = new LuxuryLineFactory();
var extrait = registry.Get("vanilla-night").CloneAs("Vanilla Night Extrait", luxury.CreateConcentration());
var extraitProduct = new Product(luxury.LineName, extrait, luxury.CreateBottle(), luxury.CreatePackaging());
printer.Print(extraitProduct);
Console.WriteLine($"Total cost: {costCalculator.Calculate(extraitProduct):F2} EUR");

    }
}