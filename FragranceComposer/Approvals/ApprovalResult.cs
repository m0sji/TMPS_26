namespace FragranceComposer.Approvals;

public record ApprovalResult(bool IsApproved, string Message)
{
    public static ApprovalResult Approved(string message) => new(true, message);
    public static ApprovalResult Rejected(string message) => new(false, message);
}