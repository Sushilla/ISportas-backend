using ISporta.Controllers.VartotojaiControler.Dto;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Persistance.Repositories.Vartotojai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISporta.Controllers.VartotojaiControler
{
    [ApiController]
    [Route("api/v1/models")]
    public class VartotojaiControler
    {
        private readonly IVartotojaiRepo _vartotojaiRepo;

        public VartotojaiControler(IVartotojaiRepo vartotojaiRepo)
        {
            _vartotojaiRepo = vartotojaiRepo;
        }

        [HttpPut]
        [Route("vartotojai")]
        public async Task<ActionResult> CreateQuestionnaire([FromBody] InsertVartotojaiRequest model)
        {
            await _vartotojaiRepo.Insert(model.VartotojoId);
            return new AcceptedResult();
        }

        [HttpDelete]
        [Route("vartotojai/{id}/{vartId}")]
        public async Task<ActionResult> DeleteQuestionnaire([FromRoute] Guid id, [FromRoute] Guid vartId)
        {
            await _vartotojaiRepo.Delete(id, vartId);
            return new AcceptedResult();
        }

        [HttpGet]
        [Route("vartotojai")]
        public async Task<ActionResult<IEnumerable<VartotojaiDo>>> GetAllQuestionnaire()
        {
            var result = await _vartotojaiRepo.GetAll();

            return new OkObjectResult(result);
        }

    }
}
