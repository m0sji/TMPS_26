using FragranceComposer.Interfaces;
namespace FragranceComposer.Decorators;

public class GiftWrapDecorator : ProductDecorator
{
    private const decimal Price = 3.5m;

    public GiftWrapDecorator(IProduct product) : base(product)
    {
    }

    public override string Description => base.Description + " +gift wrap";
    public override decimal Cost => base.Cost + Price;
}