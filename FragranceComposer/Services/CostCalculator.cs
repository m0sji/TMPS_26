using FragranceComposer.Models;
namespace FragranceComposer.Services;

public class CostCalculator
{
    public decimal Calculate(Fragrance fragrance, double bottleMl)
    {
        double oilMl = bottleMl * fragrance.Concentration.OilPercentage / 100;
        return fragrance.Formula.Sum(f => (decimal)(oilMl * f.Value / 100) * f.Key.PricePerMl);
    }
}