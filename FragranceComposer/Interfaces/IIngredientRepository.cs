using FragranceComposer.Models;
namespace FragranceComposer.Interfaces;

public interface IIngredientRepository
{
    Ingredient GetByName(string name);
}