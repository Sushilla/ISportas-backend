using ISporta.Controllers.KvietimasControler.Dto;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.Models.Trainers;
using Persistance.Repositories.Kvietimai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISporta.Controllers.KvietimasControler
{
    [ApiController]
    [Route("api/v1/models")]
    public class KvietimasControler
    {
        private readonly IKvietimaiRepo _kvietimaiRepo;

        public KvietimasControler(IKvietimaiRepo kvietimaiRepo)
        {
            _kvietimaiRepo = kvietimaiRepo;
        }

        [HttpPut]
        [Route("kvietimai")]
        public async Task<ActionResult> CreateQuestionnaire([FromBody] InsertKvietimasRequest model)
        {
            await _kvietimaiRepo.Insert(model.TrenerioID, model.VartotojoId);
            return new AcceptedResult();
        }

        [HttpDelete]
        [Route("kvietimai/{id}")]
        public async Task<ActionResult> DeleteQuestionnaire([FromRoute] Guid id)
        {
            await _kvietimaiRepo.Delete(id);
            return new AcceptedResult();
        }

        [HttpGet]
        [Route("kvietimai/{trenerioId}")]
        public async Task<ActionResult<IEnumerable<TrainerRequestsToFriendDo>>> GetAllQuestionnaire([FromRoute] Guid trenerioId)
        {
            var result = await _kvietimaiRepo.GetAll(trenerioId);

            return new OkObjectResult(result);
        }

        [HttpPost]
        [Route("kvietimai/{kvietimoid}")]
        public async Task<ActionResult> UpdateQuestionnaire([FromRoute] Guid kvietimoid, [FromBody] InsertKvietimasRequest model)
        {
            await _kvietimaiRepo.Update(kvietimoid, new Guid(model.TrenerioID), new Guid(model.VartotojoId));

            return new AcceptedResult();
        }
    }
}
