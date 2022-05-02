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
    public class SchoolController : BaseController<School>, iBaseController<School>
    {
        public SchoolController(SWARMOracleContext context,
                IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("DeleteSchool/{pSchoolId}")]
        public async Task<IActionResult> Delete(int pSchoolId)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {

                School itmSchool = await _context.Schools.Where(x => x.SchoolId == pSchoolId).FirstOrDefaultAsync();

                //DELETE EVERYTHING ELSE FIRST

                _context.Remove(itmSchool);
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
        [Route("GetSchool/{pSchoolId}")]
        public async Task<IActionResult> Get(int pSchoolId)
        {
            School itmSchool = await _context.Schools.Where(x => x.SchoolId == pSchoolId).FirstOrDefaultAsync();
            return Ok(itmSchool);
        }

        [HttpGet]
        [Route("GetSchools")]
        public async Task<IActionResult> Get()
        {
            List<School> lstSchools = await _context.Schools.ToListAsync();
            return Ok(lstSchools);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] School _School)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var newSchool = await _context.Schools.Where(x => x.SchoolId == _School.SchoolId).FirstOrDefaultAsync();

                if (newSchool != null)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                newSchool = new School();
                newSchool.SchoolName = _School.SchoolName;
                newSchool.SchoolId = _School.SchoolId;

                _context.Add(newSchool);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_School);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut]
        public async Task<IActionResult> Put([FromBody] School _School)
        {
            bool exists = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existSchool = await _context.Schools.Where(x => x.SchoolId == _School.SchoolId).FirstOrDefaultAsync();

                if (existSchool == null)
                    existSchool = new School();
                else
                    exists = true;

                existSchool.SchoolName = _School.SchoolName;
                existSchool.SchoolId = _School.SchoolId;                

                if (exists)
                    _context.Update(existSchool);
                else
                    _context.Add(existSchool);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_School);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
