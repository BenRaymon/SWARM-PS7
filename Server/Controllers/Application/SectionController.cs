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
                
                DeleteEnrollments(itmSection);
                DeleteGrades(itmSection);
                DeleteGradeTypeWeights(itmSection);
                
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
        //NEED TO TEST
        public async Task<IActionResult> Post([FromBody] Section _Section)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var newSection = await _context.Sections.Where(x => x.SectionId == _Section.SectionId).FirstOrDefaultAsync();

                if (newSection != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                newSection = new Section();
                newSection.CourseNo = _Section.CourseNo;
                newSection.SectionNo = _Section.SectionNo;
                newSection.StartDateTime = _Section.StartDateTime;
                newSection.Location = _Section.Location;
                newSection.InstructorId = _Section.InstructorId;
                newSection.Capacity = _Section.Capacity;
                newSection.SchoolId = _Section.SchoolId;

                _context.Add(newSection);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Section.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        //NEED TO TEST
        public async Task<IActionResult> Put([FromBody] Section _Section)
        {
            bool exists = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existSection = await _context.Sections.Where(x => x.SectionId == _Section.SectionId).FirstOrDefaultAsync();

                if (existSection == null)
                    existSection = new Section();
                else
                    exists = true;

                existSection.CourseNo = _Section.CourseNo;
                existSection.SectionNo = _Section.SectionNo;
                existSection.StartDateTime = _Section.StartDateTime;
                existSection.Location = _Section.Location;
                existSection.InstructorId = _Section.InstructorId;
                existSection.Capacity = _Section.Capacity;
                existSection.SchoolId = _Section.SchoolId;

                if (exists)
                    _context.Update(existSection);
                else
                    _context.Add(existSection);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Section.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
