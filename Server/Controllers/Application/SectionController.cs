using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWARM.Server.Controllers.Application
{
    public class SectionController : BaseController<Section>, iBaseController<Section>
    {
        public SectionController(SWARMOracleContext context,
                IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        public Task<IActionResult> Delete(int itemID)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public Task<IActionResult> Get(int itemID)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task<IActionResult> Post([FromBody] Section _Item)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public Task<IActionResult> Put([FromBody] Section _Item)
        {
            throw new NotImplementedException();
        }
    }
}
