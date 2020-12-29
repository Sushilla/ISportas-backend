using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.Models.Trainers;
using Models.Models.Users;
using Persistance.Repositories.Vartotojas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISporta.Controllers.VartotojasControler.Dto
{
    [ApiController]
    [Route("api/v1/models")]
    public class VartotojasControler
    {
            private readonly IVartotojasRepo _vartotojasRepo;

            public VartotojasControler(IVartotojasRepo vartotojasRepo)
            {
                _vartotojasRepo = vartotojasRepo;
            }

            [HttpPut]
            [Route("vartotojas")]
            public async Task<ActionResult> CreateQuestionnaire([FromBody] InsertVartotojasRequest model)
            {
                await _vartotojasRepo.UserRegister(model.Vardas, model.Pavarde, model.Email, model.Password);
                return new AcceptedResult();
            }

            [HttpDelete]
            [Route("vartotojas/{id}")]
            public async Task<ActionResult> DeleteQuestionnaire([FromRoute] Guid id)
            {
                await _vartotojasRepo.Delete(id);
                return new AcceptedResult();
            }

            [HttpGet]
            [Route("vartotojas")]
            public async Task<ActionResult<IEnumerable<VartotojasDo>>> GetAllQuestionnaire()
            {
                var result = await _vartotojasRepo.GetAll();

                return new OkObjectResult(result);
            }

            [HttpPost]
            [Route("vartotojas/{id}")]
            public async Task<ActionResult> UpdateQuestionnaire([FromRoute] Guid id, [FromBody] InsertVartotojasRequest model)
            {
                await _vartotojasRepo.Update(id, new Guid(model.RolesId), model.Vardas, model.Pavarde, model.Email, model.Password);

                return new AcceptedResult();
            }


            [HttpGet]
            [Route("vartotojasTreneris")]
            public async Task<ActionResult<IEnumerable<TrainerListDo>>> GetTrainers()
            {
                var result = await _vartotojasRepo.GetTrainers();

                return new OkObjectResult(result);
            }

            [HttpGet]
            [Route("vartotojasLogin/{email}/{pass}")]
            public async Task<ActionResult<IEnumerable<LoginResponseDo>>> GetLoginUserInfo([FromRoute] string email, [FromRoute] string pass)
            {
                var result = await _vartotojasRepo.GetLoginUserInfo(email, pass);

                return new OkObjectResult(result);
            }
    }
    }
