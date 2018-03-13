using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NashConfigurator.Model
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Closed { get; set; }
        public DateTime CloseDate { get; set; }
        public bool IgnoreLastPeriod { get; set; }
        public DateTime FiscalDate { get; set; }
    }
}
