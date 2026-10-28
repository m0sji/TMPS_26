using FragranceComposer.Interfaces;
namespace FragranceComposer.Components;

public class GiftBox : IPackaging
{
    public string Name => "Gift box";
    public decimal Price => 9.0m;
}