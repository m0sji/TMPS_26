using FragranceComposer.Interfaces;
using FragranceComposer.Concentrations;
using FragranceComposer.Components;
namespace FragranceComposer.Factories;

public class TravelLineFactory : IProductLineFactory
{
    public string LineName => "Travel";
    public IConcentration CreateConcentration()=> new EauDeToilette();
    public IBottle CreateBottle()=> new TravelSprayBottle();
    public IPackaging CreatePackaging()=> new Pouch();
}