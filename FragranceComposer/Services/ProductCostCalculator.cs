using FragranceComposer.Models;
namespace FragranceComposer.Services;

public class ProductCostCalculator
{
    private readonly CostCalculator _oilCalculator;

    public ProductCostCalculator(CostCalculator oilCalculator)
    {
        _oilCalculator = oilCalculator;
    }

    public decimal Calculate(Product product)=>
    _oilCalculator.Calculate(product.Fragrance, product.Bottle.VolumeMl)
    + product.Bottle.Price
    +product.Packaging.Price;
}