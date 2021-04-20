using Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISporta.Controllers.StatistikaControler.Dto
{
    public class ExerciseData
    {
        public IEnumerable<AtliktiPrat> StatistikaData { get; set; }
    }
}