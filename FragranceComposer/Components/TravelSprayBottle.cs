using FragranceComposer.Interfaces;
namespace FragranceComposer.Components;

public class TravelSprayBottle : IBottle
{
    public string Name => "Travel spray bottle";
    public double VolumeMl => 10;
    public decimal Price => 1.5m;
}