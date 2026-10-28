using FragranceComposer.Interfaces;
using FragranceComposer.Models;

namespace FragranceComposer.Repositories;

public class InMemoryIngredientRepository : IIngredientRepository
{
    private readonly List<Ingredient> _ingredients=new()
    {
        new Ingredient("Bergamot",   NoteLayer.Top,   0.8m),
        new Ingredient("Lemon",      NoteLayer.Top,   0.6m),
        new Ingredient("Sea Notes",  NoteLayer.Top,   1.2m),
        new Ingredient("Rose",       NoteLayer.Heart, 4.0m),
        new Ingredient("Jasmine",    NoteLayer.Heart, 3.5m),
        new Ingredient("Lavender",   NoteLayer.Heart, 1.0m),
        new Ingredient("Vanilla",    NoteLayer.Base,  2.0m),
        new Ingredient("Sandalwood", NoteLayer.Base,  3.0m),
        new Ingredient("Musk",       NoteLayer.Base,  1.5m),
        new Ingredient("Ambroxan",   NoteLayer.Base,  2.5m),
        };

    public Ingredient GetByName(string name)=> 
    _ingredients.FirstOrDefault(i => i.Name == name) 
    ?? throw new ArgumentException($"Unknown ingredient: {name}");
}