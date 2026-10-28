using FragranceComposer.Interfaces;
using FragranceComposer.Concentrations;
using FragranceComposer.Components;
namespace FragranceComposer.Factories;

public class ClassicLineFactory : IProductLineFactory
{
    public string LineName => "Classic";
    public IConcentration CreateConcentration()=> new EauDeParfum();
    public IBottle CreateBottle()=> new GlassBottle();
    public IPackaging CreatePackaging()=> new CardboardBox();
}