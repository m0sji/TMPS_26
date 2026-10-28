using FragranceComposer.Interfaces;
namespace FragranceComposer.Concentrations;

public class Parfum: IConcentration
{
 public string Name => "Parfum";
 public double OilPercentage => 25;   
}