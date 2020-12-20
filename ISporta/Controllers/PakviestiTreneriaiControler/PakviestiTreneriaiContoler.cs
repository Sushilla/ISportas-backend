using ISporta.Controllers.PakviestiTreneriaiControler.Dto;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
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

        [HttpDelete]
        [Route("pakviestiTreneriai/{id}")]
        public async Task<ActionResult> DeleteQuestionnaire([FromRoute] Guid id)
        {
            await _pakviestiTreneriaiRepo.Delete(id);
            return new AcceptedResult();
        }

        [HttpGet]
        [Route("pakviestiTreneriai")]
        public async Task<ActionResult<IEnumerable<PakviestiTreneriaiDo>>> GetAllQuestionnaire()
        {
            var result = await _pakviestiTreneriaiRepo.GetAll();

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
