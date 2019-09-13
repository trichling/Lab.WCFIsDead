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
    public class CalculationsController : Controller
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
        public IActionResult Get()
        {
            return Ok(CalculationResults);
        }

        [HttpGet("{id}", Name = "GetCalculationById")]
        public IActionResult GetCalculationResult(Guid id)
        {
            if (!CalculationResults.ContainsKey(id))
                return NotFound();

            return Ok(CalculationResults[id]);
        }

        [HttpPost]
        public IActionResult Create(Calculation calculation)
        {
            var resultId = Guid.NewGuid();
            var calulationResult = Operations[calculation.Operator](calculation);
            CalculationResults.Add(resultId, calulationResult);

            return CreatedAtRoute("GetCalculationById", new { id = resultId }, calulationResult);
        }

    }
}
