using FragranceComposer.Interfaces;
namespace FragranceComposer.Components;

public class Pouch : IPackaging
{
    public string Name => "Pouch";
    public decimal Price =>0.5m;
}