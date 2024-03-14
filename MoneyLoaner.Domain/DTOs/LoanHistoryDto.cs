namespace MoneyLoaner.Domain.DTOs;

public class LoanHistoryDto
{
    public int ProposalId { get; set; }
    public DateTime DateOfProposal { get; set; }
    public string? ProposalStatus { get; set; }
    public string? LoanNumber { get; set; }
    public decimal CurrentBalance { get; set; }
}