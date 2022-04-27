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
        [Route("DeleteSection/{pSectionId}")]
        public async Task<IActionResult> Delete(int pSectionId)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Section itmSection = await _context.Sections.Where(x => x.SectionId == pSectionId).FirstOrDefaultAsync();
                
                deleteEnrollments(itmSection);
                deleteGrades(itmSection);
                deleteGradeTypeWeights(itmSection);
                
                _context.Remove(itmSection);
                await _context.SaveChangesAsync();
                //trans.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
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
        [Route("GetSections")]
        public async Task<IActionResult> Get()
        {
            List<Section> lstSections = await _context.Sections.ToListAsync();
            return Ok(lstSections);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Section _Item)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Section _Item)
        {
            throw new NotImplementedException();
        }
    }
}
