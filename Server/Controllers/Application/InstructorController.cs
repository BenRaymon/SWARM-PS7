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

                if(itmInstructor == null)
                {
                    trans.Rollback();
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                _context.Remove(itmInstructor);
                await _context.SaveChangesAsync();
                trans.Commit();
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
            
            if(itmInst == null)
                return StatusCode(StatusCodes.Status404NotFound);
            else
                return Ok(itmInst);
        }


        [HttpGet]
        [Route("GetInstructors")]
        public async Task<IActionResult> Get()
        {
            List<Instructor> lstInstructors = await _context.Instructors.OrderBy(x=>x.InstructorId).ToListAsync();
            return Ok(lstInstructors);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Instructor _Instructor)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var newInst = await _context.Instructors.Where(x => x.InstructorId == _Instructor.InstructorId).FirstOrDefaultAsync();

                if (newInst != null)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                newInst = new Instructor();
                newInst.Salutation = _Instructor.Salutation;
                newInst.FirstName = _Instructor.FirstName;
                newInst.LastName = _Instructor.LastName;
                newInst.StreetAddress = _Instructor.StreetAddress;
                newInst.Zip = _Instructor.Zip;
                newInst.Phone = _Instructor.Phone;
                newInst.SchoolId = _Instructor.SchoolId;

                _context.Add(newInst);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Instructor);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Instructor _Instructor)
        {
            bool exists = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existInst = await _context.Instructors.Where(x => x.InstructorId == _Instructor.InstructorId).FirstOrDefaultAsync();

                if (existInst == null)
                    existInst = new Instructor();
                else
                    exists = true;

                existInst.Salutation = _Instructor.Salutation;
                existInst.FirstName = _Instructor.FirstName;
                existInst.LastName = _Instructor.LastName;
                existInst.StreetAddress = _Instructor.StreetAddress;
                existInst.Zip = _Instructor.Zip;
                existInst.Phone = _Instructor.Phone;
                existInst.SchoolId = _Instructor.SchoolId;

                if (exists)
                    _context.Update(existInst);
                else
                    _context.Add(existInst);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Instructor);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
