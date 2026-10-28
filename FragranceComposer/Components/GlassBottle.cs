using FragranceComposer.Interfaces;
namespace FragranceComposer.Components;

public class GlassBottle : IBottle
{
    public string Name => "Glass bottle";
    public double VolumeMl => 50;
    public decimal Price =>4.0m;
}