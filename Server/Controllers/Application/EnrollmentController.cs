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
                
                Enrollment itmEnr = await _context.Enrollments.Where(x => x.StudentId == pStudentId && x.SectionId == pSectionId).FirstOrDefaultAsync();
                DeleteGrades(pStudentId, pSectionId);

                _context.Remove(itmEnr);
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
        [Route("GetEnrollment/{pStudentId}/{pSectionId}")]
        public async Task<IActionResult> Get(int pStudentId, int pSectionId)
        {
            Enrollment itmEnr = await _context.Enrollments.Where(x => x.StudentId == pStudentId && x.SectionId == pSectionId).FirstOrDefaultAsync();
            return Ok(itmEnr);
        }

        [HttpGet]
        [Route("GetEnrollmentsByStudent/{pStudentId}")]
        public async Task<IActionResult> GetByStudentId(int pStudentId)
        {
            List<Enrollment> lstEnr = await _context.Enrollments.Where(x => x.StudentId == pStudentId).ToListAsync();
            return Ok(lstEnr);
        }

        [HttpGet]
        [Route("GetEnrollments")]
        public async Task<IActionResult> Get()
        {
            List<Enrollment> lstEnr = await _context.Enrollments.ToListAsync();
            return Ok(lstEnr);
        }

        public Task<IActionResult> Post([FromBody] Enrollment _Enrollment)
        {
            throw new NotImplementedException();
        }

        
        public Task<IActionResult> Put([FromBody] Enrollment _Enrollment)
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
