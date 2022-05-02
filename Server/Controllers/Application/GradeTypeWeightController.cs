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
    public class GradeTypeWeightController : BaseController<GradeTypeWeight>, iBaseController<GradeTypeWeight>
    {
        public GradeTypeWeightController(SWARMOracleContext context,
                IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }


        [HttpDelete]
        [Route("DeleteGradeTypeWeight/{pSectionId}/{pGTC}")]
        public async Task<IActionResult> Delete(int pSectionId, string pGTC)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                GradeTypeWeight itmGTW = await _context.GradeTypeWeights
                                .Where(x => x.SectionId == pSectionId && x.GradeTypeCode == pGTC)
                                .FirstOrDefaultAsync();

                if(itmGTW == null)
                {
                    trans.Rollback();
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                var grades = await _context.Grades
                        .Where(x => x.SectionId == pSectionId && x.GradeTypeCode == pGTC).ToListAsync();
                foreach (var grade in grades)
                {
                    _context.Grades.Remove(grade);
                }

                _context.Remove(itmGTW);
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
        [Route("GetGradeTypeWeight/{pSectionId}/{pGTC}")]
        public async Task<IActionResult> Get(int pSectionId, string pGTC)
        {
            GradeTypeWeight itmGTW = await _context.GradeTypeWeights
                                .Where(x => x.SectionId == pSectionId && x.GradeTypeCode == pGTC)
                                .FirstOrDefaultAsync();

            return Ok(itmGTW);
        }

        [HttpGet]
        [Route("GetGradeTypeWeights")]
        public async Task<IActionResult> Get()
        {
            List<GradeTypeWeight> lstGTW = await _context.GradeTypeWeights.ToListAsync();
            return Ok(lstGTW);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeTypeWeight _GradeTypeWeight)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var newGradeTypeWeight = await _context.GradeTypeWeights
                                        .Where(x => x.GradeTypeCode == _GradeTypeWeight.GradeTypeCode && x.SectionId == _GradeTypeWeight.SectionId)
                                        .FirstOrDefaultAsync();

                if (newGradeTypeWeight != null)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                newGradeTypeWeight = new GradeTypeWeight();
                newGradeTypeWeight.SchoolId = _GradeTypeWeight.SchoolId;
                newGradeTypeWeight.SectionId = _GradeTypeWeight.SectionId;
                newGradeTypeWeight.GradeTypeCode = _GradeTypeWeight.GradeTypeCode;
                newGradeTypeWeight.NumberPerSection = _GradeTypeWeight.NumberPerSection;
                newGradeTypeWeight.PercentOfFinalGrade = _GradeTypeWeight.PercentOfFinalGrade;
                newGradeTypeWeight.DropLowest = _GradeTypeWeight.DropLowest;

                
                _context.Add(newGradeTypeWeight);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeTypeWeight);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeTypeWeight _GradeTypeWeight)
        {
            bool exists = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeTypeWeight = await _context.GradeTypeWeights
                                        .Where(x => x.GradeTypeCode == _GradeTypeWeight.GradeTypeCode && x.SectionId == _GradeTypeWeight.SectionId)
                                        .FirstOrDefaultAsync();

                if (existGradeTypeWeight == null)
                    existGradeTypeWeight = new GradeTypeWeight();
                else
                    exists = true;

                existGradeTypeWeight.SchoolId = _GradeTypeWeight.SchoolId;
                existGradeTypeWeight.SectionId = _GradeTypeWeight.SectionId;
                existGradeTypeWeight.GradeTypeCode = _GradeTypeWeight.GradeTypeCode;
                existGradeTypeWeight.NumberPerSection = _GradeTypeWeight.NumberPerSection;
                existGradeTypeWeight.PercentOfFinalGrade = _GradeTypeWeight.PercentOfFinalGrade;
                existGradeTypeWeight.DropLowest = _GradeTypeWeight.DropLowest;

                if (exists)
                    _context.Update(existGradeTypeWeight);
                else
                    _context.Add(existGradeTypeWeight);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeTypeWeight);
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
