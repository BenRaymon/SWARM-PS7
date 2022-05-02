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

                if(itmGrade == null)
                {
                    trans.Rollback();
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                _context.Remove(itmGrade);
                await _context.SaveChangesAsync();
                trans.Commit();
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

            if(itmGrade == null)
                return StatusCode(StatusCodes.Status404NotFound);
            else
                return Ok(itmGrade);
        }

        [HttpGet]
        [Route("GetGradesByStudent/{pStudentId}")]
        public async Task<IActionResult> GetByStudentId(int pStudentId)
        {
            List<Grade> lstGrades = await _context.Grades.Where(x => x.StudentId == pStudentId).OrderBy(x => x.StudentId).ToListAsync();
            
            if(lstGrades.Count == 0)
                return StatusCode(StatusCodes.Status404NotFound);
            else
                return Ok(lstGrades);
        }

        [HttpGet]
        [Route("GetGradesBySection/{pSectionId}")]
        public async Task<IActionResult> GetBySectionId(int pSectionId)
        {
            List<Grade> lstGrades = await _context.Grades.Where(x => x.SectionId == pSectionId).OrderBy(x => x.SectionId).ToListAsync();
            
            if (lstGrades.Count == 0)
                return StatusCode(StatusCodes.Status404NotFound);
            else
                return Ok(lstGrades);
        }

        [HttpGet]
        [Route("GetGradesByStudentSection/{pStudentId}/{pSectionId}")]
        public async Task<IActionResult> GetByStudentSection(int pStudentId, int pSectionId)
        {
            List<Grade> lstGrades = await _context.Grades
                                        .Where(x => x.StudentId == pStudentId && x.SectionId == pSectionId)
                                        .ToListAsync();
            
            if (lstGrades.Count == 0)
                return StatusCode(StatusCodes.Status404NotFound);
            else
                return Ok(lstGrades);
        }

        [HttpGet]
        [Route("GetGrades")]
        public async Task<IActionResult> Get()
        {
            List<Grade> lstGrades = await _context.Grades.ToListAsync();
            return Ok(lstGrades);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Grade _Grade)
        {
            bool exists = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGrade = await _context.Grades
                                .Where(x => x.StudentId == _Grade.StudentId && x.SectionId == _Grade.SectionId
                                    && x.GradeTypeCode == _Grade.GradeTypeCode && x.GradeCodeOccurrence == _Grade.GradeCodeOccurrence)
                                .FirstOrDefaultAsync();

                if (existGrade == null)
                    existGrade = new Grade();
                else
                    exists = true;

                existGrade.StudentId = _Grade.StudentId;
                existGrade.SectionId = _Grade.SectionId;
                existGrade.SchoolId = _Grade.SchoolId;
                existGrade.GradeCodeOccurrence = _Grade.GradeCodeOccurrence;
                existGrade.GradeTypeCode = _Grade.GradeTypeCode;
                existGrade.NumericGrade = _Grade.NumericGrade;
                existGrade.Comments = _Grade.Comments;

                if (exists)
                    _context.Update(existGrade);
                else
                    _context.Add(existGrade);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(existGrade);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Grade _Grade)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var newGrade = await _context.Grades
                                .Where(x => x.StudentId == _Grade.StudentId && x.SectionId == _Grade.SectionId
                                    && x.GradeTypeCode == _Grade.GradeTypeCode && x.GradeCodeOccurrence == _Grade.GradeCodeOccurrence)
                                .FirstOrDefaultAsync();

                if (newGrade != null)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                newGrade = new Grade();

                newGrade.StudentId = _Grade.StudentId;
                newGrade.SectionId = _Grade.SectionId;
                newGrade.SchoolId = _Grade.SchoolId;
                newGrade.GradeCodeOccurrence = _Grade.GradeCodeOccurrence;
                newGrade.GradeTypeCode = _Grade.GradeTypeCode;
                newGrade.NumericGrade = _Grade.NumericGrade;
                newGrade.Comments = _Grade.Comments;

                _context.Add(newGrade);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(newGrade);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
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
