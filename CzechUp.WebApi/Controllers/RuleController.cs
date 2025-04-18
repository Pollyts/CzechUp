using CzechUp.EF.Models;
using CzechUp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CzechUp.WebApi.Controllers
{
    public class RuleController : BaseController
    {
        private readonly IRuleService ruleService;
        public RuleController(IRuleService ruleService)
        {
            this.ruleService = ruleService;
        }
        [HttpGet]
        public async Task<IActionResult> GetRules(CancellationToken cancellationToken)
        {
            var rules = await this.ruleService.GetRules(cancellationToken);
            return Ok(rules);
        }

        [HttpGet("ruleNotes")]
        public async Task<IActionResult> GetRuleNotes(Guid ruleGuid, CancellationToken cancellationToken)
        {
            var ruleNotes = await this.ruleService.GetRuleNotes(ruleGuid, UserGuid(), cancellationToken);
            return Ok(ruleNotes);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRuleNotes([FromBody] UserRuleNote userRuleNote, CancellationToken cancellationToken)
        {
            var ruleNote = await this.ruleService.UpdateRuleNote(userRuleNote, cancellationToken);
            return Ok(ruleNote);
        }
    }
}
