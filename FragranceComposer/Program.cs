using FragranceComposer.Builders;
using FragranceComposer.Models;
using FragranceComposer.Services;
using FragranceComposer.Repositories;
using FragranceComposer.Interfaces;
using FragranceComposer.Concentrations;
using FragranceComposer.Factories;
using System.Net.Http.Headers;

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


//2 Director with predifined receipts
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


    }
}