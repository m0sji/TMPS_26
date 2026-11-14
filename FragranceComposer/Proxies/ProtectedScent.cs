using System.Runtime.CompilerServices;
using System.Text;
using FragranceComposer.Interfaces;
using FragranceComposer.Models;
namespace FragranceComposer.Proxies;

public class ProtectedScent : IScent
{
    private readonly IScent _scent;
    private readonly IUserSession _session;

    public ProtectedScent(IScent scent, IUserSession session)
    {
        _scent = scent;
        _session = session;
    }

    public string Name => _scent.Name;
    public decimal CostPerMl => _scent.CostPerMl;

    public IReadOnlyDictionary<Ingredient, double> Composition
    {
        get
        {
            if (_session.CurrentRole != UserRole.Perfumer)
            throw new UnauthorizedAccessException($"The formula of '{_scent.Name}' is secret; {_session.CurrentRole} cannot read it");
            return _scent.Composition;
        }
    }
}