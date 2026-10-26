using FragranceComposer.Builders;
using FragranceComposer.Models;
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
       var builder = new FragranceBuilder(repository);
       var printer = new LabelPrinter();
       var calculator = new CostCalculator();

       //1. Builder used directly
       var summer = builder
       .WithName("Moldovian Summer")
       .WithFamily(ScentFamily.Floral)
       .WithConcentration(new EauDeParfum())
       .AddNote("Bergamot", 25)
       .AddNote("Rose", 40)
       .AddNote("Vanilla", 35)
       .Build();


//2 Director with predifined receipts
var director = new FragranceDirector(builder);
var marine= director.BuildAquatic("Black Sea Breeze", new EauDeToilette());
var gourmand = director.BuildGourmand("Vanilla Night", new EauDeParfum());

    foreach (var f in new[]{summer, marine, gourmand})
        {
            printer.Print(f);
            Console.WriteLine($"Oil cost for 50 ml: {calculator.Calculate(f,50):F2}EUR\n");
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