using FragranceComposer.Interfaces;
using FragranceComposer.Models;
namespace FragranceComposer.Composites;

public class LayeredScent : IScent
{
    private readonly List<(IScent Scent, int Sprays)> _layers = new();
    public string Name {get;}
    public LayeredScent(string name)
    {
        Name = name;
    }

    public LayeredScent Add(IScent scent, int sprays)
    {
        if (sprays <= 0)
        throw new ArgumentOutOfRangeException(nameof(sprays), "Sprays must be positive");
        _layers.Add((scent,sprays));
        return this;
    }

    public IReadOnlyDictionary<Ingredient, double> Composition
    {
        get
        {
            int total = TotalSprays();
            var combined = new Dictionary<Ingredient, double>();
            foreach (var (scent,sprays) in _layers)
            foreach(var note in scent.Composition)
            combined[note.Key] = combined.GetValueOrDefault(note.Key)+note.Value*sprays/total;
            return combined;
        }
    }

    public decimal CostPerMl => _layers.Sum(l => l.Scent.CostPerMl*l.Sprays)/TotalSprays();
    private int TotalSprays()=> 
    _layers.Count >0 
    ? _layers.Sum(l => l.Sprays) 
    : throw new InvalidOperationException($"Layered scent '{Name}' has no layers");
}