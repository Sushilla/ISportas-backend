using ISporta.Controllers.StatistikaControler.Dto;
using Microsoft.AspNetCore.Mvc;
using Models.dto;
using Models.Models.Statistic;
using Persistance.Repositories.Statistika;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISporta.Controllers.StatistikaControler
{
    [ApiController]
    [Route("api/v1/models")]
    public class StatistikaControler
    {
        private readonly IStatistikaRepo _statistikaRepo;

        public StatistikaControler(IStatistikaRepo statistikaRepo)
        {
            _statistikaRepo = statistikaRepo;
        }

        [HttpPut]
        [Route("statistika")]
        public async Task<ActionResult> CreateQuestionnaire([FromBody] InsertStatistikaRequest model)
        {
            await _statistikaRepo.Insert(model.VartotojoId);
            return new AcceptedResult();
        }

        [HttpDelete]
        [Route("statistika/{id}")]
        public async Task<ActionResult> DeleteQuestionnaire([FromRoute] Guid id)
        {
            await _statistikaRepo.Delete(id);
            return new AcceptedResult();
        }

        [HttpGet]
        [Route("statistika")]
        public async Task<ActionResult<IEnumerable<StatistikaDo>>> GetAllQuestionnaire()
        {
            var result = await _statistikaRepo.GetAll();

            return new OkObjectResult(result);
        }
        [HttpGet]
        [Route("statistika/{userId}")]
        public async Task<ActionResult<StatisticGeneralDo>> GetAllQuestionnaire([FromRoute] string userId)
        {
            var result = await _statistikaRepo.GetUserGeneralStatistic(userId);

            return new OkObjectResult(result);
        }

        [HttpGet]
        [Route("statistikafortrainer/{userId}/{workoutId}")]
        public async Task<ActionResult<StatisticGeneralDo2>> GetAllQuestionnaireTrainer([FromRoute] string userId, [FromRoute] string workoutId)
        {
            var result = await _statistikaRepo.GetUserlStatisticForTrainer(userId, workoutId);

            return new OkObjectResult(result);
        }

        [HttpPost]
        [Route("statistika/{statistikosId}")]
        public async Task<ActionResult> UpdateQuestionnaire([FromRoute] Guid statistikosId, [FromBody] InsertStatistikaRequest model)
        {
            await _statistikaRepo.Update(statistikosId, model.TreniruotesPabaiga);

            return new AcceptedResult();
        }
    }
}
