using CzechUp.EF;
using CzechUp.EF.Models;
using CzechUp.Services.DTOs;
using CzechUp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CzechUp.Services.Services
{
    public class RuleService : BaseService<Rule>, IRuleService
    {
        private readonly DatabaseContext _databaseContext;

        public RuleService(DatabaseContext databaseContext) : base(databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public async Task<List<Rule>> GetRules(CancellationToken cancellationToken)
        {
            return await GetQuery().ToListAsync(cancellationToken);
        }

        public async Task<UserRuleNote> GetRuleNotes(Guid ruleGuid, Guid userGuid, CancellationToken cancellationToken)
        {
            return _databaseContext.UserRuleNotes.Where(r => r.UserGuid == userGuid && r.RuleGuid == ruleGuid).Include(r=>r.Rule).FirstOrDefault();
        }

        public async Task<UserRuleNote> UpdateRuleNote(UserRuleNote ruleNote, CancellationToken cancellationToken)
        {
            var dbRuleNote = _databaseContext.UserRuleNotes.Where(r => r.Guid == ruleNote.Guid).FirstOrDefault();
            if (dbRuleNote == null)
            {
                throw new Exception("Can not update rule note");
            }

            dbRuleNote.Note = ruleNote.Note;
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return dbRuleNote;
        }        
    }
}
