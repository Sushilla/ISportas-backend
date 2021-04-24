using ISporta.Controllers.PratymaiControler.Dto;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Persistance.Repositories.Pratymai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISporta.Controllers.PratymaiControler
{
    [ApiController]
    [Route("api/v1/models")]
    public class PratymaiControler
    {
        private readonly IPratymaiRepo _pratymaiRepo;

        public PratymaiControler(IPratymaiRepo roleRepo)
        {
            _pratymaiRepo = roleRepo;
        }

        [HttpPut]
        [Route("pratymai")]
        public async Task<ActionResult> CreateQuestionnaire([FromBody] InsertPratymaiRequest model)
        {
            await _pratymaiRepo.Insert(model.Pavadinimas);
            return new AcceptedResult();
        }

        [HttpDelete]
        [Route("pratymai/{id}")]
        public async Task<ActionResult> DeleteQuestionnaire([FromRoute] Guid id)
        {
            await _pratymaiRepo.Delete(id);
            return new AcceptedResult();
        }

        [HttpGet]
        [Route("pratymai")]
        public async Task<ActionResult<IEnumerable<PratymaiDo>>> GetAllQuestionnaire()
        {
            var result = await _pratymaiRepo.GetAll();

            return new OkObjectResult(result);
        }

        [HttpPost]
        [Route("pratymai/{pratymoid}")]
        public async Task<ActionResult> UpdateQuestionnaire([FromRoute] Guid pratymoid, [FromBody] UpdatePratymaiRequest model)
        {
            await _pratymaiRepo.Update(pratymoid, model.Pavadinimas);

            return new AcceptedResult();
        }
    }
}
