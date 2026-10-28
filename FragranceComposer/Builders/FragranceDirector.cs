using FragranceComposer.Models;
using FragranceComposer.Interfaces;

namespace FragranceComposer.Builders;

public class FragranceDirector
{
    private readonly FragranceBuilder _builder;

    public FragranceDirector(FragranceBuilder builder)
    {
        _builder = builder;
    }

    public Fragrance BuildAquatic(string name, IConcentration concentration)=>
    _builder.WithName(name)
            .WithFamily(ScentFamily.Aquatic)
            .WithConcentration(concentration)
            .AddNote("Sea Notes", 20)
            .AddNote("Bergamot", 15)
            .AddNote("Lavender", 25)
            .AddNote("Musk", 25)
            .AddNote("Ambroxan", 15)
            .Build();

    public Fragrance BuildGourmand(string name, IConcentration concentration)=>
    _builder.WithName(name)
            .WithFamily(ScentFamily.Gourmand)
            .WithConcentration(concentration)
            .AddNote("Bergamot", 15)
            .AddNote("Jasmine", 25)
            .AddNote("Vanilla", 40)
            .AddNote("Sandalwood",20)
            .Build();




}