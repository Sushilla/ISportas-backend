using ISporta.Controllers.RoleControler.Dto;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Persistance.Repositories.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISporta.Controllers.RoleControler
{
    [ApiController]
    [Route("api/v1/models")]
    public class RoleControler
    {
        private readonly IRoleRepo _roleRepo;

        public RoleControler(IRoleRepo roleRepo)
        {
            _roleRepo = roleRepo;
        }

        [HttpPut]
        [Route("role")]
        public async Task<ActionResult> CreateQuestionnaire([FromBody] InsertRoleRequest model)
        {
            await _roleRepo.Insert(model.Pavadinimas);
            return new AcceptedResult();
        }

        [HttpDelete]
        [Route("role/{id}")]
        public async Task<ActionResult> DeleteQuestionnaire([FromRoute] Guid id)
        {
            await _roleRepo.Delete(id);
            return new AcceptedResult();
        }

        [HttpGet]
        [Route("role")]
        public async Task<ActionResult<IEnumerable<RoleDo>>> GetAllQuestionnaire()
        {
            var result = await _roleRepo.GetAll();

            return new OkObjectResult(result);
        }

        [HttpPost]
        [Route("role/{roleid}")]
        public async Task<ActionResult> UpdateQuestionnaire([FromRoute] Guid roleid, [FromBody] InsertRoleRequest model)
        {
            await _roleRepo.Update(roleid, model.Pavadinimas);

            return new AcceptedResult();
        }
    }
}
