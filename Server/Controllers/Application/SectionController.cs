using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [Route("GetSection/{pSectionId}")]
        public async Task<IActionResult> Get(int pSectionId)
        {
            Section itmSection = await _context.Sections.Where(x => x.SectionId == pSectionId).FirstOrDefaultAsync();
            return Ok(itmSection);
        }

        [HttpGet]
        [Route("GetSections/{pCourseNo}")]
        public async Task<IActionResult> GetSectionsByCourse(int pCourseNo)
        {
            List<Section> lstSections = await _context.Sections.Where(x => x.CourseNo == pCourseNo).ToListAsync();
            return Ok(lstSections);
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
