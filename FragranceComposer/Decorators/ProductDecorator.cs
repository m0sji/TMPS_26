using FragranceComposer.Interfaces;
namespace FragranceComposer.Decorators;

public abstract class ProductDecorator : IProduct
{
    private readonly IProduct _product;

    protected ProductDecorator(IProduct product)
    {
        _product = product;
    }

    public virtual string Description => _product.Description;
    public virtual decimal Cost => _product.Cost;
}