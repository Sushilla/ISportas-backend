using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models.Statistic
{
    public class StatisticGeneralDo
    {
        public double meanTime { get; set; }
        public double meanCount { get; set; }
        public IEnumerable<string> chartLabels { get; set; }
        public IEnumerable<tableDataa> dataForTable { get; set; }

    }

    public class tableDataa
    {
        public IEnumerable<int> data { get; set; }
        public string label { get; set; }
    }

    public class StatisticGeneralDo2
    {
        //public double meanTime { get; set; }
        //public double meanCount { get; set; }
        public IEnumerable<string> chartLabels { get; set; }
        public IEnumerable<tableDataa> dataForTable { get; set; }
        public IEnumerable<tableDataa2> dataForTable2 { get; set; }
    }

    public class tableDataa2
    {
        public IEnumerable<double> data { get; set; }
        public string label { get; set; }
    }
}
