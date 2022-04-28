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

                _context.Remove(itmZip);
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
        [Route("GetZipcode/{pZip}")]
        public async Task<IActionResult> Get(string pZip)
        {
            Zipcode itmZip = await _context.Zipcodes.Where(x => x.Zip == pZip).FirstOrDefaultAsync();
            return Ok(itmZip);
        }

        [HttpGet]
        [Route("GetZipcodes")]
        public async Task<IActionResult> Get()
        {
            List<Zipcode> lstZips = await _context.Zipcodes.ToListAsync();
            return Ok(lstZips);
        }

        public async Task<IActionResult> Post([FromBody] Zipcode _Item)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> Put([FromBody] Zipcode _Item)
        {
            throw new NotImplementedException();
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
