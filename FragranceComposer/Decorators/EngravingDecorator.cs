using FragranceComposer.Interfaces;
namespace FragranceComposer.Decorators;

public class EngravingDecorator : ProductDecorator
{
    private const decimal Price = 6.0m;
    private readonly string _text;

    public EngravingDecorator(IProduct product, string text) : base(product)
    {
        _text = text;
    }

    public override string Description => base.Description + $" + engraving \"{_text}\"";
    public override decimal Cost => base.Cost + Price;
}