using FragranceComposer.Models;
namespace FragranceComposer.Prototypes;

public class FragranceRegistry
{
    private readonly Dictionary<string, Fragrance> _prototype =new();
    public void Register(string key, Fragrance prototype) => _prototype[key]=prototype;

    public Fragrance Get(string key)=>
    _prototype.TryGetValue(key, out var prototype)
    ?prototype.Clone()
    :throw new ArgumentException($"Unknown prototype: {key}");
}