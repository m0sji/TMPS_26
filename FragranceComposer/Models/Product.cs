using FragranceComposer.Interfaces;
namespace FragranceComposer.Models;

public class Product
{
    public string LineName {get;}
    public Fragrance Fragrance {get;}
    public IBottle Bottle {get;}
    public IPackaging Packaging {get;}

    public Product(string lineName, Fragrance fragrance, IBottle bottle, IPackaging packaging)
    {
        LineName = lineName;
        Fragrance = fragrance;
        Bottle = bottle;
        Packaging = packaging;
    }
}