﻿using ISporta.Controllers.PrasymaiPakeistRoleControler.Dto;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Persistance.Repositories.PrasymaiPakeistRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISporta.Controllers.PrasymaiPakeistRoleControler
{
    [ApiController]
    [Route("api/v1/models")]
    public class PrasymaiPakeistRoleControler
    {
        private readonly IPrasymaiPakeistRoleRepo _prasymaiPakeistRoleRepo;

        public PrasymaiPakeistRoleControler(IPrasymaiPakeistRoleRepo pakviestiTreneriaiRepo)
        {
            _prasymaiPakeistRoleRepo = pakviestiTreneriaiRepo;
        }

        [HttpPut]
        [Route("PrasymaiPakeistRole")]
        public async Task<ActionResult> CreateQuestionnaire([FromBody] InsertPrasymaiPakeistRoleRequest model)
        {
            await _prasymaiPakeistRoleRepo.Insert(model.Id);
            return new AcceptedResult();
        }

        [HttpDelete]
        [Route("PrasymaiPakeistRole/{id}")]
        public async Task<ActionResult> DeleteQuestionnaire([FromRoute] Guid id)
        {
            await _prasymaiPakeistRoleRepo.Delete(id);
            return new AcceptedResult();
        }

        [HttpGet]
        [Route("PrasymaiPakeistRole")]
        public async Task<ActionResult<IEnumerable<PrasymaiPakeistRoleDo>>> GetAllQuestionnaire()
        {
            var result = await _prasymaiPakeistRoleRepo.GetAll();

            return new OkObjectResult(result);
        }
    }
}
