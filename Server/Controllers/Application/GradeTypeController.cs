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
    public class GradeTypeController : BaseController<GradeType>, iBaseController<GradeType>
    {
        public GradeTypeController(SWARMOracleContext context,
                IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }


        [HttpDelete]
        [Route("DeleteGradeType/{pGTC}")]
        public async Task<IActionResult> Delete(string pGTC)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                GradeType itmGradeType = await _context.GradeTypes.Where(x => x.GradeTypeCode == pGTC).FirstOrDefaultAsync();

                if(itmGradeType == null)
                {
                    trans.Rollback();
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                //delete grade first
                //then delete grade type weight

                _context.Remove(itmGradeType);
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
        [Route("GetGradeType/{pGTC}")]
        public async Task<IActionResult> Get(string pGTC)
        {
            GradeType itmGradeType = await _context.GradeTypes.Where(x => x.GradeTypeCode == pGTC).FirstOrDefaultAsync();

            if(itmGradeType == null)
                return StatusCode(StatusCodes.Status404NotFound);
            else
                return Ok(itmGradeType);
        }

        [HttpGet]
        [Route("GetGradeTypes")]
        public async Task<IActionResult> Get()
        {
            List<GradeType> lstGradeTypes = await _context.GradeTypes.ToListAsync();
            return Ok(lstGradeTypes);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeType _GradeType)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var newGradeType = await _context.GradeTypes.Where(x => x.GradeTypeCode == _GradeType.GradeTypeCode).FirstOrDefaultAsync();

                if (newGradeType != null)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                newGradeType = new GradeType();
                newGradeType.SchoolId = _GradeType.SchoolId;
                newGradeType.GradeTypeCode = _GradeType.GradeTypeCode;
                newGradeType.Description = _GradeType.Description;

                _context.Add(newGradeType);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeType);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeType _GradeType)
        {
            bool exists = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeType = await _context.GradeTypes.Where(x => x.GradeTypeCode == _GradeType.GradeTypeCode).FirstOrDefaultAsync();

                if (existGradeType == null)
                    existGradeType = new GradeType();
                else
                    exists = true;

                existGradeType.SchoolId = _GradeType.SchoolId;
                existGradeType.GradeTypeCode = _GradeType.GradeTypeCode;
                existGradeType.Description = _GradeType.Description;

                if (exists)
                    _context.Update(existGradeType);
                else
                    _context.Add(existGradeType);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeType);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //NOT IMPLEMENTED BECAUSE STRING PK
        public Task<IActionResult> Delete(int itemID)
        {
            throw new NotImplementedException();
        }

        //NOT IMPLEMENTED BECAUSE STRING PK
        public Task<IActionResult> Get(int itemID)
        {
            throw new NotImplementedException();
        }
    }
}
