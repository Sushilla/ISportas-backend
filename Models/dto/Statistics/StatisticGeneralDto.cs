using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models.Statistic
{
    public class StatisticGeneralDto
    {
        public string meanTime { get; set; }
        public string meanCount { get; set; }
        public IEnumerable<string> chartLabels { get; set; }
        public IEnumerable<string> dataForTable { get; set; }

    }

    public class tableData
    {
        public IEnumerable<string> data { get; set; }
        public string label { get; set; }
    }
}
