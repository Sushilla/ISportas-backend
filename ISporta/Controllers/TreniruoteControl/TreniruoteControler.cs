using ISporta.Controllers.TreniruoteControl.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistance.Repositories.Treniruote;
using ISporta.Controllers.KvietimasControler.Dto;
using Models.Models;

namespace ISporta.Controllers.TreniruoteControl
{
    [ApiController]
    [Route("api/v1/models")]
    public class TreniruoteControler
    {
        private readonly ITreniruoteRepo _treniruoteRepo;

        public TreniruoteControler(ITreniruoteRepo treniruoteRepo)
        {
            _treniruoteRepo = treniruoteRepo;
        }

        [HttpPut]
        [Route("treniruote")]
        public async Task<ActionResult> CreateQuestionnaire([FromBody] InsertTreniruoteRequest model)
        {
            await _treniruoteRepo.Insert(model.TrenerioId, model.VartotojoId, model.Pavadinimas, model.Aprasymas);
            return new AcceptedResult();
        }

        [HttpDelete]
        [Route("treniruote/{id}")]
        public async Task<ActionResult> DeleteQuestionnaire([FromRoute] Guid id)
        {
            await _treniruoteRepo.Delete(id);
            return new AcceptedResult();
        }

        [HttpGet]
        [Route("treniruote")]
        public async Task<ActionResult<IEnumerable<KvietimaiDo>>> GetAllQuestionnaire()
        {
            var result = await _treniruoteRepo.GetAll();

            return new OkObjectResult(result);
        }

        [HttpPost]
        [Route("treniruote/{treniruotesid}")]
        public async Task<ActionResult> UpdateQuestionnaire([FromRoute] Guid treniruotesid, [FromBody] InsertTreniruoteRequest model)
        {
            await _treniruoteRepo.Update(treniruotesid, new Guid(model.TrenerioId), new Guid(model.VartotojoId), model.Pavadinimas, model.Aprasymas);

            return new AcceptedResult();
        }
    }
}
