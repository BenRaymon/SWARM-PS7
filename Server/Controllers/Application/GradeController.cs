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
    public class GradeController : BaseController<Grade>, iBaseController<Grade>
    {
        public GradeController(SWARMOracleContext context,
                IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }


        [HttpDelete]
        [Route("DeleteGrade/{pStudentId}/{pSectionId}/{pGTC}/{pGCO}")]
        public async Task<IActionResult> Delete(int pStudentId, int pSectionId, string pGTC, int pGCO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Grade itmGrade = await _context.Grades
                                .Where(x => x.StudentId == pStudentId && x.SectionId == pSectionId
                                    && x.GradeTypeCode == pGTC && x.GradeCodeOccurrence == pGCO)
                                .FirstOrDefaultAsync();

                _context.Remove(itmGrade);
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
        [Route("GetGrade/{pStudentId}/{pSectionId}/{pGTC}/{pGCO}")]
        public async Task<IActionResult> Get(int pStudentId, int pSectionId, string pGTC, int pGCO)
        {
            Grade itmGrade = await _context.Grades
                                .Where(x => x.StudentId == pStudentId && x.SectionId == pSectionId
                                    && x.GradeTypeCode == pGTC && x.GradeCodeOccurrence == pGCO)
                                .FirstOrDefaultAsync();

            return Ok(itmGrade);
        }

        [HttpGet]
        [Route("GetGradesByStudent/{pStudentId}")]
        public async Task<IActionResult> GetByStudentId(int pStudentId)
        {
            List<Grade> lstGrades = await _context.Grades.Where(x => x.StudentId == pStudentId).ToListAsync();
            return Ok(lstGrades);
        }

        [HttpGet]
        [Route("GetGradesBySection/{pSectionId}")]
        public async Task<IActionResult> GetBySectionId(int pSectionId)
        {
            List<Grade> lstGrades = await _context.Grades.Where(x => x.SectionId == pSectionId).ToListAsync();
            return Ok(lstGrades);
        }

        [HttpGet]
        [Route("GetGradesByStudentSection/{pStudentId}/{pSectionId}")]
        public async Task<IActionResult> GetByStudentSection(int pStudentId, int pSectionId)
        {
            List<Grade> lstGrades = await _context.Grades
                                        .Where(x => x.StudentId == pStudentId && x.SectionId == pSectionId)
                                        .ToListAsync();
            return Ok(lstGrades);
        }

        [HttpGet]
        [Route("GetGrades")]
        public async Task<IActionResult> Get()
        {
            List<Grade> lstGrades = await _context.Grades.ToListAsync();
            return Ok(lstGrades);
        }

        public Task<IActionResult> Post([FromBody] Grade _Item)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Put([FromBody] Grade _Item)
        {
            throw new NotImplementedException();
        }

        //NOT IMPLEMENTED BECAUSE COMPOSITE PK
        public Task<IActionResult> Delete(int itemID)
        {
            throw new NotImplementedException();
        }

        //NOT IMPLEMENTED BECAUSE COMPOSITE PK
        public Task<IActionResult> Get(int itemID)
        {
            throw new NotImplementedException();
        }
    }
}
