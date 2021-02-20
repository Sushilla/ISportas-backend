using ISporta.Controllers.PratymuSkaiciusControler.Dto;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Persistance.Repositories.PratymuSkaicius;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISporta.Controllers.PratymuSkaiciusControler
{
    [ApiController]
    [Route("api/v1/models")]
    public class PratymuSkaiciusControler
    {
        private readonly IPratymuSkaiciusRepo _pratymuSkaiciusRepo;

        public PratymuSkaiciusControler(IPratymuSkaiciusRepo pratymuSkaiciusRepp)
        {
            _pratymuSkaiciusRepo = pratymuSkaiciusRepp;
        }

        [HttpPut]
        [Route("pratymuSkaicius")]
        public async Task<ActionResult> CreateQuestionnaire([FromBody] InsertPratymuSkaiciusRequest model)
        {
            await _pratymuSkaiciusRepo.Insert(model.id, model.PratimoId, model.Priejimas, model.Skaicius);
            return new AcceptedResult();
        }

        [HttpDelete]
        [Route("pratymuSkaicius/{id}")]
        public async Task<ActionResult> DeleteQuestionnaire([FromRoute] Guid id)
        {
            await _pratymuSkaiciusRepo.Delete(id);
            return new AcceptedResult();
        }

        [HttpGet]
        [Route("pratymuSkaicius/{trenerioid}")]
        public async Task<ActionResult<IEnumerable<PratymuSksaiciusDo>>> GetAllQuestionnaire([FromRoute] Guid trenerioid)
        {
            var result = await _pratymuSkaiciusRepo.GetAll(trenerioid);

            return new OkObjectResult(result);
        }


        [HttpPost]
        [Route("pratymuSkaicius/{pratymuSkaiciusid}")]
        public async Task<ActionResult> UpdateQuestionnaire([FromRoute] Guid pratymuSkaiciusid, [FromBody] InsertPratymuSkaiciusRequest model)
        {
            await _pratymuSkaiciusRepo.Update(pratymuSkaiciusid, new Guid(model.PratimoId), model.Priejimas, model.Skaicius);

            return new AcceptedResult();
        }
    }
}
