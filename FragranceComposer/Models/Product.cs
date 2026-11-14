using FragranceComposer.Interfaces;
namespace FragranceComposer.Models;

public class Product: IProduct
{
    public string LineName {get;}
    public IScent Scent {get;}
    public IBottle Bottle {get;}
    public IPackaging Packaging {get;}

    public Product(string lineName, IScent scent, IBottle bottle, IPackaging packaging)
    {
        LineName = lineName;
        Scent = scent;
        Bottle = bottle;
        Packaging = packaging;
    }

    public string Description => $"{Scent.Name} [{LineName} line]: {Bottle.Name} ({Bottle.VolumeMl} ml), {Packaging.Name}";
    public decimal Cost => (decimal)Bottle.VolumeMl * Scent.CostPerMl + Bottle.Price + Packaging.Price;
}