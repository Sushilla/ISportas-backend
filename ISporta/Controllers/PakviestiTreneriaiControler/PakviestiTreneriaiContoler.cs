using ISporta.Controllers.PakviestiTreneriaiControler.Dto;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.Models.Users;
using Persistance.Repositories.PakviestiTreneriai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISporta.Controllers.PakviestiTreneriaiControler
{
    [ApiController]
    [Route("api/v1/models")]
    public class PakviestiTreneriaiContoler
    {
        private readonly IPakviestiTreneriaiRepo _pakviestiTreneriaiRepo;

        public PakviestiTreneriaiContoler(IPakviestiTreneriaiRepo pakviestiTreneriaiRepo)
        {
            _pakviestiTreneriaiRepo = pakviestiTreneriaiRepo;
        }

        [HttpPut]
        [Route("pakviestiTreneriai")]
        public async Task<ActionResult> CreateQuestionnaire([FromBody] InsertPakviestiTreneriaiRequest model)
        {
            await _pakviestiTreneriaiRepo.Insert(model.Id, model.TrenerioID);
            return new AcceptedResult();
        }

        [HttpPut]
        [Route("pakviestiTreneriaiAcceptReuqest/{kvietimoId}")]
        public async Task<ActionResult> CopyAndDeleteFromKvietimai([FromRoute] Guid kvietimoId)
        {
            await _pakviestiTreneriaiRepo.InsertAcceptedRequest(kvietimoId);
            return new AcceptedResult();
        }

        [HttpDelete]
        [Route("pakviestiTreneriai/{id}")]
        public async Task<ActionResult> DeleteQuestionnaire([FromRoute] Guid id)
        {
            await _pakviestiTreneriaiRepo.Delete(id);
            return new AcceptedResult();
        }

        [HttpGet]
        [Route("pakviestiTreneriai/{id}")]
        public async Task<ActionResult<IEnumerable<UserGetAcceptedTrainerListDo>>> GetAllQuestionnaire([FromRoute] Guid id)
        {
            var result = await _pakviestiTreneriaiRepo.GetAll(id);

            return new OkObjectResult(result);
        }

        [HttpPost]
        [Route("pakviestiTreneriai/{pakvietimoid}")]
        public async Task<ActionResult> UpdateQuestionnaire([FromRoute] Guid pakvietimoid, [FromBody] InsertPakviestiTreneriaiRequest model)
        {
            await _pakviestiTreneriaiRepo.Update(pakvietimoid, new Guid(model.Id), model.Statusas, new Guid(model.TrenerioID));

            return new AcceptedResult();
        }
    }
}
