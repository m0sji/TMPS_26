using System.Runtime.InteropServices.Marshalling;
using FragranceComposer.Interfaces;
namespace FragranceComposer.Components;

public class CardboardBox : IPackaging
{
    public string Name => "Cardboard box";
    public decimal Price =>1.2m;
}