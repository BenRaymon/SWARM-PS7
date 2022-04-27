﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWARM.Server.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseController<T> : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public BaseController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async void deleteEnrollments(Section sec)
        {
            var enrollment = await _context.Enrollments
                        .Where(x => x.SectionId == sec.SectionId).ToListAsync();
            foreach (var enr in enrollment)
            {
                _context.Enrollments.Remove(enr);
            }
        }

        public async void deleteGrades(Section sec)
        {
            var grades = await _context.Grades
                        .Where(x => x.SectionId == sec.SectionId).ToListAsync();
            foreach (var grade in grades)
            {
                _context.Grades.Remove(grade);
            }
        }

        public async void deleteGradeTypeWeights(Section sec)
        {
            var grade_type_weights = await _context.GradeTypeWeights
                        .Where(x => x.SectionId == sec.SectionId).ToListAsync();
            foreach (var gtw in grade_type_weights)
            {
                _context.GradeTypeWeights.Remove(gtw);
            }
        }
    }
}
