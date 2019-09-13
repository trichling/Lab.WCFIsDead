using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab.WCFIsDead.ASPNETCore.Web.Models
{
    public class Calculation
    {
        public decimal Operand1 { get; set; }
        public decimal Operand2 { get; set; }
        public string Operator { get; set; }
    }

    public class CalculationResult
    {
        public Calculation Calculation { get; set; }
        public decimal Result { get; set; }
    }
}
