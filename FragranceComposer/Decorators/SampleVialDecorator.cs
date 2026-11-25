using FragranceComposer.Interfaces;
namespace FragranceComposer.Decorators;

public class SampleVialDecorator : ProductDecorator
{
    private const double VolumeMl = 2;
    private const decimal VialPrice = 0.6m;
    private readonly IScent _sample;

    public SampleVialDecorator(IProduct product, IScent sample) : base(product)
    {
        _sample = sample;
    }

    public override string Description => base.Description + $" + {VolumeMl} ml sample of {_sample.Name}";
    public override decimal Cost => base.Cost + (decimal)VolumeMl * _sample.CostPerMl + VialPrice;
}