using FragranceComposer.Interfaces;
namespace FragranceComposer.Factories;

public interface IProductLineFactory
{
    string LineName {get;}
    IConcentration CreateConcentration();
    IBottle CreateBottle();
    IPackaging CreatePackaging();
}