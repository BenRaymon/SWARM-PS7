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
    public class GradeConversionController : BaseController<GradeConversionController>, iBaseController<GradeConversion>
    {

        public GradeConversionController(SWARMOracleContext context,
                IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }


        [HttpDelete]
        [Route("DeleteGradeConversion/{pLetterGrade}")]
        public async Task<IActionResult> Delete(string pLetterGrade)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                GradeConversion itmGradeConv = await _context.GradeConversions.Where(x => x.LetterGrade == pLetterGrade).FirstOrDefaultAsync();

                if(itmGradeConv == null)
                {
                    trans.Rollback();
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                _context.Remove(itmGradeConv);
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
        [Route("GetGradeConversion/{pLetterGrade}")]
        public async Task<IActionResult> Get(string pLetterGrade)
        {
            GradeConversion itmGradeConv = await _context.GradeConversions.Where(x => x.LetterGrade == pLetterGrade).FirstOrDefaultAsync();
    
            if(itmGradeConv == null)
                return StatusCode(StatusCodes.Status404NotFound);
            else
                return Ok(itmGradeConv);
        }

        [HttpGet]
        [Route("GetGradeConversions")]
        public async Task<IActionResult> Get()
        {
            List<GradeConversion> lstGradeConvs = await _context.GradeConversions.ToListAsync();
            return Ok(lstGradeConvs);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeConversion _GradeConversion)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var newGradeConv = await _context.GradeConversions.Where(x => x.LetterGrade == _GradeConversion.LetterGrade).FirstOrDefaultAsync();

                if (newGradeConv != null)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                newGradeConv = new GradeConversion();
                newGradeConv.SchoolId = _GradeConversion.SchoolId;
                newGradeConv.LetterGrade = _GradeConversion.LetterGrade;
                newGradeConv.GradePoint = _GradeConversion.GradePoint;
                newGradeConv.MaxGrade = _GradeConversion.MaxGrade;
                newGradeConv.MinGrade = _GradeConversion.MinGrade;

                _context.Add(newGradeConv);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeConversion);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeConversion _GradeConversion)
        {
            bool exists = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeConv = await _context.GradeConversions.Where(x => x.LetterGrade == _GradeConversion.LetterGrade).FirstOrDefaultAsync();

                if (existGradeConv == null)
                    existGradeConv = new GradeConversion();
                else
                    exists = true;

                existGradeConv.SchoolId = _GradeConversion.SchoolId;
                existGradeConv.LetterGrade = _GradeConversion.LetterGrade;
                existGradeConv.GradePoint = _GradeConversion.GradePoint;
                existGradeConv.MaxGrade = _GradeConversion.MaxGrade;
                existGradeConv.MinGrade = _GradeConversion.MinGrade;

                if (exists)
                    _context.Update(existGradeConv);
                else
                    _context.Add(existGradeConv);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeConversion);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        //NOT IMPLEMENTED BECAUSE STRING PK
        public Task<IActionResult> Get(int itemID)
        {
            throw new NotImplementedException();
        }

        //NOT IMPLEMENTED BECAUSE STRING PK
        public Task<IActionResult> Delete(int itemID)
        {
            throw new NotImplementedException();
        }
    }
}
