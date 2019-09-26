using Lab.WCFIsDead.ASPNETCore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab.WCFIsDead.ASPNETCore.Web.Controllers
{
    [ApiController]
    [Route("calculations")]
    public class CalculationsController : ControllerBase
    {

        private static Dictionary<Guid, CalculationResult> CalculationResults = new Dictionary<Guid, CalculationResult>();
        private Dictionary<string, Func<Calculation, CalculationResult>> Operations = new Dictionary<string, Func<Calculation, CalculationResult>>();

        public CalculationsController()
        {
            Operations.Add("+", c => new CalculationResult() { Calculation = c, Result = c.Operand1 + c.Operand2 });
            Operations.Add("-", c => new CalculationResult() { Calculation = c, Result = c.Operand1 - c.Operand2 });
            Operations.Add("*", c => new CalculationResult() { Calculation = c, Result = c.Operand1 * c.Operand2 });
            Operations.Add("/", c => new CalculationResult() { Calculation = c, Result = c.Operand1 / c.Operand2 });
        }

        [HttpGet]
        public IActionResult GetAllCalculations()
        {
            return Ok(CalculationResults.Select(kvp => new 
            { 
                Id = kvp.Key,
                Calculation = kvp.Value
            }).ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<CalculationResult> GetCalculationResult(Guid id)
        {
            if (!CalculationResults.ContainsKey(id))
                return NotFound();

            return Ok(CalculationResults[id]);
        }

        [HttpPost]
        public IActionResult CreateCalculationRequest(Calculation calculation)
        {
            var resultId = Guid.NewGuid();
            var calulationResult = Operations[calculation.Operator](calculation);
            CalculationResults.Add(resultId, calulationResult);

            return CreatedAtAction(nameof(GetCalculationResult), new { id = resultId }, calulationResult);
        }

    }
}
