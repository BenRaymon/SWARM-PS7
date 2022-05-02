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
    public class ZipcodeController : BaseController<Zipcode>, iBaseController<Zipcode>
    {
        public ZipcodeController(SWARMOracleContext context,
                IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("DeleteZipcode/{pZip}")]
        public async Task<IActionResult> Delete(string pZip)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Zipcode itmZip = await _context.Zipcodes.Where(x => x.Zip == pZip).FirstOrDefaultAsync();
                //ZIP IS A NOT-NULL FK IN INSTRUCTOR - CANNOT DELETE WITHOUT VIOLATING FK CONSTRAINT

                if(itmZip == null)
                {
                    trans.Rollback();
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                _context.Remove(itmZip);
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
        [Route("GetZipcode/{pZip}")]
        public async Task<IActionResult> Get(string pZip)
        {
            Zipcode itmZip = await _context.Zipcodes.Where(x => x.Zip == pZip).FirstOrDefaultAsync();
            
            if(itmZip == null)
                return StatusCode(StatusCodes.Status404NotFound);
            else
                return Ok(itmZip);
        }

        [HttpGet]
        [Route("GetZipcodes")]
        public async Task<IActionResult> Get()
        {
            List<Zipcode> lstZips = await _context.Zipcodes.ToListAsync();
            return Ok(lstZips);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Zipcode _Zip)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var newZip = await _context.Zipcodes.Where(x => x.Zip == _Zip.Zip).FirstOrDefaultAsync();

                if (newZip != null)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                newZip = new Zipcode();
                newZip.Zip = _Zip.Zip;
                newZip.City = _Zip.City;
                newZip.State = _Zip.State;

                _context.Add(newZip);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Zipcode _Zip)
        {
            bool exists = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existZip = await _context.Zipcodes.Where(x => x.Zip == _Zip.Zip).FirstOrDefaultAsync();

                if (existZip == null)
                    existZip = new Zipcode();
                else
                    exists = true;

                existZip.Zip = _Zip.Zip;
                existZip.City = _Zip.City;
                existZip.State = _Zip.State;
                
                if (exists)
                    _context.Update(existZip);
                else
                    _context.Add(existZip);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //NOT IMPLEMENTED BECAUSE ZIP IS A STRING
        public Task<IActionResult> Get(int pZip)
        {
            throw new NotImplementedException();
        }

        //NOT IMPLEMENTED BECAUSE ZIP IS A STRING
        public Task<IActionResult> Delete(int pZip)
        {
            throw new NotImplementedException();
        }
    }
}
