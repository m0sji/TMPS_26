using FragranceComposer.Interfaces;
using FragranceComposer.Models;

namespace FragranceComposer.Builders;

public class FragranceBuilder
{
    private readonly IIngredientRepository _repository;
    private string? _name;
    private ScentFamily? _family;
    private IConcentration? _concentration;
    private Dictionary<Ingredient, double> _formula = new();

    public FragranceBuilder(IIngredientRepository repository)
    {
        _repository = repository;
    }

    public FragranceBuilder WithName(string name) {_name = name;return this;}
    public FragranceBuilder WithFamily(ScentFamily family) {_family = family; return this;}
    public FragranceBuilder WithConcentration(IConcentration concentration){_concentration = concentration; return this;}

    public FragranceBuilder AddNote(string ingredientName, double percent)
    {
        if (percent<=0)
            throw new ArgumentOutOfRangeException(nameof(percent), "Percent must be positive");
            _formula[_repository.GetByName(ingredientName)] = percent;
            return this;
    }

    public Fragrance Build()
    {
        try
        {
            Validate();
            return new Fragrance(_name!, _family!.Value, _concentration!, _formula);
        }
        finally
        {
            Reset();
        }

    }

    public void Reset()
    {
        _name = null;
        _family = null;
        _concentration = null;
        _formula = new Dictionary<Ingredient, double>();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(_name))
        throw new InvalidOperationException("Fragrance name is required");
        if (_family is null)
        throw new InvalidOperationException("Scent family is required");
        if (_concentration is null)
        throw new InvalidOperationException("Concentration is required");

        double total = _formula.Values.Sum();
        if (Math.Abs(total-100)>0.01)
        throw new InvalidOperationException($"Formula must total 100%, got{total}%");

        foreach (var layer in Enum.GetValues<NoteLayer>())
        if (!_formula.Keys.Any(i=> i.Layer == layer))
        throw new InvalidOperationException ($"Formula is missing a {layer} note");
    }













}