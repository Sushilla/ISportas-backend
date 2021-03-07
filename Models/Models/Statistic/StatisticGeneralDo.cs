using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models.Statistic
{
    public class StatisticGeneralDo
    {
        public double meanTime { get; set; }
        public int meanCount { get; set; }
        public IEnumerable<string> chartLabels { get; set; }
        public IEnumerable<tableDataa> dataForTable { get; set; }

    }

    public class tableDataa
    {
        public IEnumerable<int> data { get; set; }
        public string label { get; set; }
    }
}
