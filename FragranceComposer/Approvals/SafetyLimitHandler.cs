using FragranceComposer.Models;
namespace FragranceComposer.Approvals;

public class SafetyLimitHandler : ApprovalHandler
{
    private readonly IReadOnlyDictionary<string, double> _maxPercent;

    public SafetyLimitHandler(IReadOnlyDictionary<string, double> maxPercent)
    {
        _maxPercent = maxPercent;
    }

    public override ApprovalResult Handle(ProductionOrder order)
    {
        foreach( var note in order.Product.Scent.Composition)
        if (_maxPercent.TryGetValue(note.Key.Name, out double max) && note.Value>max)
        return ApprovalResult.Rejected($"safety check: {note.Key.Name} is {note.Value:F1}%, the limit is {max:F1}%");
        return base.Handle(order);
    }
}