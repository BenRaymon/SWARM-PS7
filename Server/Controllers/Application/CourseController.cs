using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
using SWARM.Server.Models;
using SWARM.Shared;
using SWARM.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Controllers.Application
{
    
    public class CourseController : BaseController<Course>, iBaseController<Course>
    {
        public CourseController(SWARMOracleContext context,
                IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("GetCourses")]
        public async Task<IActionResult> Get()
        {
            List<Course> lstCourses = await _context.Courses.ToListAsync();
            return Ok(lstCourses);
        }

        [HttpGet]
        [Route("GetCourse/{pCourseNo}")]
        public async Task<IActionResult> Get(int pCourseNo)
        {
            Course itmCourse = await _context.Courses.Where(x => x.CourseNo == pCourseNo).FirstOrDefaultAsync();
            return Ok(itmCourse);
        }

        [HttpDelete]
        [Route("DeleteCourse/{pCourseNo}")]
        public async Task<IActionResult> Delete(int pCourseNo)
        {
            Course itmCourse = await _context.Courses.Where(x => x.CourseNo == pCourseNo).FirstOrDefaultAsync();
            _context.Remove(itmCourse);
            await _context.SaveChangesAsync();
            return Ok();
        }

        //TODO: Put and Post do the same thing right now. Fix POST functionality. 
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Course _Course)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existCourse = await _context.Courses.Where(x => x.CourseNo == _Course.CourseNo).FirstOrDefaultAsync();

                existCourse.Cost = _Course.Cost;
                existCourse.Description = _Course.Description;
                existCourse.Prerequisite = _Course.Prerequisite;
                existCourse.PrerequisiteSchoolId = _Course.PrerequisiteSchoolId;
                existCourse.SchoolId = _Course.SchoolId;
                _context.Update(existCourse);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Course.CourseNo);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Course _Course)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existCourse = await _context.Courses.Where(x => x.CourseNo == _Course.CourseNo).FirstOrDefaultAsync();

                existCourse.Cost = _Course.Cost;
                existCourse.Description = _Course.Description;
                existCourse.Prerequisite = _Course.Prerequisite;
                existCourse.PrerequisiteSchoolId = _Course.PrerequisiteSchoolId;
                existCourse.SchoolId = _Course.SchoolId;
                _context.Update(existCourse);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Course.CourseNo);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
