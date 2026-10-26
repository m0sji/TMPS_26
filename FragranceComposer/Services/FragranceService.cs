using FragranceComposer.Interfaces;
using FragranceComposer.Models;

namespace FragranceComposer.Services;

public class FragranceService
{
    private readonly IIngredientRepository _repository;
    public FragranceService(IIngredientRepository repository)
    {
        _repository = repository;
    }

    public Fragrance Create(string name, IConcentration concentration, Dictionary<string, double> formula)
    {
        var items=formula.ToDictionary(f => _repository.GetByName(f.Key), f => f.Value);
        return new Fragrance(name, concentration, items);
    }
}
