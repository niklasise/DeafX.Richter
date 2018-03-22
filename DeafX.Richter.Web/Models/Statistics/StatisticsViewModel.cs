using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeafX.Richter.Web.Models.Statistics
{
    public class StatisticsViewModel : Dictionary<long, double>
    {
        public StatisticsViewModel(Dictionary<long, double> values)
            : base(values)
        {

        }
    }
}
