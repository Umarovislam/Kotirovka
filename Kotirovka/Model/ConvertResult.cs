using System;
using System.Collections.Generic;
using System.Text;

namespace Kotirovka.Model
{
    public class ConvertResult
    {
        public string Code { get; set; }
        public decimal Nominal { get; set; }
        public decimal Usd { get; set; }
        public decimal Rubl { get; set; }
    }
}
