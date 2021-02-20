using ISporta.Controllers.TreniruoteControl.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistance.Repositories.Treniruote;
using ISporta.Controllers.KvietimasControler.Dto;
using Models.Models;
using Models.Models.Treniruotes;
using Persistance.Repositories.Vartotojai;
using Persistance.Repositories.PratymuSkaicius;

namespace ISporta.Controllers.TreniruoteControl
{
    [ApiController]
    [Route("api/v1/models")]
    public class TreniruoteControler
    {
        private readonly ITreniruoteRepo _treniruoteRepo;
        private readonly IVartotojaiRepo _ivertotojai;
        private readonly IPratymuSkaiciusRepo _ipratymuSkaicius;
        public TreniruoteControler(ITreniruoteRepo treniruoteRepo, IVartotojaiRepo ivertotojai, IPratymuSkaiciusRepo ipratymuSkaicius)
        {
            _treniruoteRepo = treniruoteRepo;
            _ivertotojai = ivertotojai;
            _ipratymuSkaicius = ipratymuSkaicius;
        }

        [HttpPut]
        [Route("treniruote")]
        public async Task<ActionResult> CreateQuestionnaire([FromBody] InsertTreniruoteRequest model)
        {
            await _treniruoteRepo.Insert(model.TrenerioId, model.VartotojoId, model.Pavadinimas, model.Aprasymas, model.vartID, model.prat);
            return new AcceptedResult();
        }

        [HttpDelete]
        [Route("treniruote/{id}")]
        public async Task<ActionResult> DeleteQuestionnaire([FromRoute] Guid id)
        {
            await _treniruoteRepo.Delete(id);
            await _ivertotojai.DeleteAll(id);
            await _ipratymuSkaicius.DeleteAll(id);
            //need delete statistika????????????
            return new AcceptedResult();
        }

        [HttpGet]
        [Route("treniruote/{trenerioid}")]
        public async Task<ActionResult<IEnumerable<KvietimaiDo>>> GetAllQuestionnaire([FromRoute] Guid trenerioid)
        {
            var result = await _treniruoteRepo.GetAll(trenerioid);

            return new OkObjectResult(result);
        }

        [HttpGet]
        [Route("treniruoteEditData/{trenerioid}")]
        public async Task<ActionResult<IEnumerable<TreniruotesWithDataDo>>> GetAllTreniruoteData([FromRoute] Guid trenerioid)
        {
            var result = await _treniruoteRepo.GetEditData(trenerioid);

            return new OkObjectResult(result);
        }

        /*[HttpPost]
        [Route("treniruote/{treniruotesid}")]
        public async Task<ActionResult> UpdateQuestionnaire([FromRoute] Guid treniruotesid, [FromBody] InsertTreniruoteRequest model)
        {
            await _treniruoteRepo.Update(treniruotesid, new Guid(model.TrenerioId), new Guid(model.VartotojoId), model.Pavadinimas, model.Aprasymas);

            return new AcceptedResult();
        }*/

        [HttpPost]
        [Route("treniruote/")]
        public async Task<ActionResult> UpdateQuestionnaire([FromBody] UpdateTreniruotes model)
        {
            await _treniruoteRepo.Update(new Guid(model.TreniruotesId), model.Pavadinimas, model.Aprasymas);
            await _ivertotojai.DeleteAll(new Guid(model.TreniruotesId));
            foreach (var user in model.UsersIds)
            {
            await _ivertotojai.Insert(model.TreniruotesId, user);
            }
            await _ipratymuSkaicius.DeleteAll(new Guid(model.TreniruotesId));
            foreach(var exercise in model.TreniruotesPratymai)
            {
                await _ipratymuSkaicius.Insert(model.TreniruotesId, exercise.pratymoId, exercise.priejimai, exercise.skaicius);
            }
            return new AcceptedResult();
        }
    }
}
