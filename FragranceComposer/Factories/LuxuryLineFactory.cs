using FragranceComposer.Interfaces;
using FragranceComposer.Concentrations;
using FragranceComposer.Components;
namespace FragranceComposer.Factories;

public class LuxuryLineFactory : IProductLineFactory
{
    public string LineName => "Luxury";
    public IConcentration CreateConcentration() => new Parfum();
    public IBottle CreateBottle()=> new CrystalBottle();
    public IPackaging CreatePackaging()=> new GiftBox();
}