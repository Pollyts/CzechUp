using CzechUp.EF.Models;

namespace CzechUp.Services.Interfaces
{
    public interface IRuleService
    {
        Task<List<Rule>> GetRules(CancellationToken cancellationToken);
        Task<UserRuleNote> GetRuleNotes(Guid ruleGuid, Guid userGuid, CancellationToken cancellationToken);
        Task<UserRuleNote> UpdateRuleNote(UserRuleNote ruleNote, CancellationToken cancellationToken);
    }
}
