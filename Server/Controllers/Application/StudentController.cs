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
    public class StudentController : BaseController<Student>, iBaseController<Student>
    {
        public StudentController(SWARMOracleContext context,
                IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("DeleteStudent/{pStudentId}")]
        public async Task<IActionResult> Delete(int pStudentId)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Student itmStudent = await _context.Students.Where(x => x.StudentId == pStudentId).FirstOrDefaultAsync();

                var enrollment = await _context.Enrollments
                    .Where(x => x.StudentId == pStudentId).ToListAsync();
                foreach (var enr in enrollment)
                {
                    Section sec = await _context.Sections.Where(x => x.SectionId == enr.SectionId).FirstOrDefaultAsync();
                    deleteGrades(sec);

                    _context.Enrollments.Remove(enr);
                }

                _context.Remove(itmStudent);
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
        [Route("GetStudent/{pStudentId}")]
        public async Task<IActionResult> Get(int pStudentId)
        {
            Student itmStudent = await _context.Students.Where(x => x.StudentId == pStudentId).FirstOrDefaultAsync();
            return Ok(itmStudent);
        }


        [HttpGet]
        [Route("GetStudents")]
        public async Task<IActionResult> Get()
        {
            List<Student> lstStudents = await _context.Students.ToListAsync();
            return Ok(lstStudents);
        }

        [HttpPost]
        //NEED TO TEST
        public async Task<IActionResult> Post([FromBody] Student _Student)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var newStudent = await _context.Students.Where(x => x.StudentId == _Student.StudentId).FirstOrDefaultAsync();

                if (newStudent != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                newStudent = new Student();
                newStudent.Salutation = _Student.Salutation;
                newStudent.FirstName = _Student.FirstName;
                newStudent.LastName = _Student.LastName;
                newStudent.StreetAddress = _Student.StreetAddress;
                newStudent.Zip = _Student.Zip;
                newStudent.Phone = _Student.Phone;
                newStudent.Employer = _Student.Employer;
                newStudent.RegistrationDate = _Student.RegistrationDate;
                newStudent.SchoolId = _Student.SchoolId;

                _context.Add(newStudent);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Student.StudentId);
            } 
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        //NEED TO TEST
        public async Task<IActionResult> Put([FromBody] Student _Student)
        {
            bool exists = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existStudent = await _context.Students.Where(x => x.StudentId == _Student.StudentId).FirstOrDefaultAsync();

                if (existStudent == null)
                    existStudent = new Student();
                else
                    exists = true;

                existStudent.Salutation = _Student.Salutation;
                existStudent.FirstName = _Student.FirstName;
                existStudent.LastName = _Student.LastName;
                existStudent.StreetAddress = _Student.StreetAddress;
                existStudent.Zip = _Student.Zip;
                existStudent.Phone = _Student.Phone;
                existStudent.Employer = _Student.Employer;
                existStudent.RegistrationDate = _Student.RegistrationDate;
                existStudent.SchoolId = _Student.SchoolId;

                if (exists)
                    _context.Update(existStudent);
                else
                    _context.Add(existStudent);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Student.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
