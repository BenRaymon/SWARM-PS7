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
    public class EnrollmentController : BaseController<Enrollment>, iBaseController<Enrollment>
    {
        public EnrollmentController(SWARMOracleContext context,
                IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("DeleteEnrollment/{pStudentId}/{pSectionId}")]
        public async Task<IActionResult> Delete(int pStudentId, int pSectionId)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                
                Enrollment itmEnr = await _context.Enrollments
                                        .Where(x => x.StudentId == pStudentId && x.SectionId == pSectionId)
                                        .FirstOrDefaultAsync();

                if(itmEnr == null)
                {
                    trans.Rollback();
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                DeleteGrades(pStudentId, pSectionId);

                _context.Remove(itmEnr);
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
        [Route("GetEnrollment/{pStudentId}/{pSectionId}")]
        public async Task<IActionResult> Get(int pStudentId, int pSectionId)
        {
            Enrollment itmEnr = await _context.Enrollments
                                    .Where(x => x.StudentId == pStudentId && x.SectionId == pSectionId)
                                    .FirstOrDefaultAsync();
            if(itmEnr == null)
                return StatusCode(StatusCodes.Status404NotFound);
            else
                return Ok(itmEnr);
        }

        [HttpGet]
        [Route("GetEnrollmentsByStudent/{pStudentId}")]
        public async Task<IActionResult> GetByStudentId(int pStudentId)
        {
            List<Enrollment> lstEnrollments = await _context.Enrollments.Where(x => x.StudentId == pStudentId).OrderBy(x => x.StudentId).ToListAsync();
            if(lstEnrollments.Count == 0)
                return StatusCode(StatusCodes.Status404NotFound);
            else
                return Ok(lstEnrollments);
        }

        [HttpGet]
        [Route("GetEnrollmentsBySection/{pSectionId}")]
        public async Task<IActionResult> GetBySectionId(int pSectionId)
        {
            List<Enrollment> lstEnrollments = await _context.Enrollments.Where(x => x.SectionId == pSectionId).OrderBy(x => x.SectionId).ToListAsync();
            if (lstEnrollments.Count == 0)
                return StatusCode(StatusCodes.Status404NotFound);
            else
                return Ok(lstEnrollments);
        }

        [HttpGet]
        [Route("GetEnrollments")]
        public async Task<IActionResult> Get()
        {
            List<Enrollment> lstEnrollments = await _context.Enrollments.ToListAsync();
            return Ok(lstEnrollments);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Enrollment _Enrollment)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var newEnrollment = await _context.Enrollments
                                        .Where(x => x.SectionId == _Enrollment.SectionId && x.StudentId == _Enrollment.StudentId)
                                        .FirstOrDefaultAsync();

                if (newEnrollment != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                newEnrollment = new Enrollment();
                newEnrollment.StudentId = _Enrollment.StudentId;
                newEnrollment.SectionId = _Enrollment.SectionId;
                newEnrollment.EnrollDate = _Enrollment.EnrollDate;
                newEnrollment.FinalGrade = _Enrollment.FinalGrade;
                newEnrollment.SchoolId = _Enrollment.SchoolId;

                _context.Add(newEnrollment);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Enrollment);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Enrollment _Enrollment)
        {
            bool exists = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existEnrollment = await _context.Enrollments
                                        .Where(x => x.SectionId == _Enrollment.SectionId && x.StudentId == _Enrollment.StudentId)
                                        .FirstOrDefaultAsync();

                if (existEnrollment == null)
                    existEnrollment = new Enrollment();
                else
                    exists = true;

                existEnrollment.StudentId = _Enrollment.StudentId;
                existEnrollment.SectionId = _Enrollment.SectionId;
                existEnrollment.EnrollDate = _Enrollment.EnrollDate;
                existEnrollment.FinalGrade = _Enrollment.FinalGrade;
                existEnrollment.SchoolId = _Enrollment.SchoolId;

                if (exists)
                    _context.Update(existEnrollment);
                else
                    _context.Add(existEnrollment);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Enrollment);
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
