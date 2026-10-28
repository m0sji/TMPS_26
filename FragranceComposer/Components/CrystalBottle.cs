using FragranceComposer.Interfaces;
namespace FragranceComposer.Components;

public class CrystalBottle : IBottle
{
    public string Name => "Crystal bottle";
    public double VolumeMl => 100;
    public decimal Price => 18.0m;
}