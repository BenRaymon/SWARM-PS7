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
    public class InstructorController : BaseController<Instructor>, iBaseController<Instructor>
    {
        public InstructorController(SWARMOracleContext context,
                IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("DeleteInstructor/{pInstructorId}")]
        public async Task<IActionResult> Delete(int pInstructorId)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Instructor itmInstructor = await _context.Instructors.Where(x => x.InstructorId == pInstructorId).FirstOrDefaultAsync();
                //INSTRUCTOR IS A FK IN SECTION - NEED TO HANDLE THAT
                _context.Remove(itmInstructor);
                await _context.SaveChangesAsync();
                //trans.Commit();
                return Ok();

            } catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("GetInstructor/{pInstructorId}")]
        public async Task<IActionResult> Get(int pInstructorId)
        {
            Instructor itmInst = await _context.Instructors.Where(x => x.InstructorId == pInstructorId).FirstOrDefaultAsync();
            return Ok(itmInst);
        }


        [HttpGet]
        [Route("GetInstructors")]
        public async Task<IActionResult> Get()
        {
            List<Instructor> lstInstructors = await _context.Instructors.ToListAsync();
            return Ok(lstInstructors);
        }

        [HttpPost]
        public Task<IActionResult> Post([FromBody] Instructor _Item)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public Task<IActionResult> Put([FromBody] Instructor _Item)
        {
            throw new NotImplementedException();
        }
    }
}
