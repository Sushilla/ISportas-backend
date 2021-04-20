using Models.Classes;
using Models.dto;
using Models.Models.Statistic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Statistika
{
    public interface IStatistikaRepo
    {
        public Task<Guid> Insert(string VartotojoId);
        public Task Delete(Guid id);
        public Task<IEnumerable<StatistikaDo>> GetAll();
        public Task Update(Guid id, IEnumerable<AtliktiPrat> prat);
        public Task<StatisticGeneralDo> GetUserGeneralStatistic(string VartotojoId);
        public Task<StatisticGeneralDo2> GetUserlStatisticForTrainer(string VartotojoId, string WorkoutId);
    }
}
