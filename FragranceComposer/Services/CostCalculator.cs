using FragranceComposer.Interfaces;
namespace FragranceComposer.Services;

public class CostCalculator
{
    public decimal Calculate(IScent scent, double bottleML) =>
    (decimal)bottleML * scent.CostPerMl;
}