namespace FragranceComposer.Models;

  public enum NoteLayer 
  {
    Top,
    Heart,
    Base
  }
  
  public class Ingredient
  {
    public string Name { get;}
    public NoteLayer Layer { get;}
    public decimal PricePerMl { get; }

    public Ingredient(string name, NoteLayer layer, decimal pricePerMl)
    {
      Name = name;
      Layer = layer;
      PricePerMl = pricePerMl;
    }
  }
