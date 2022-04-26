using Microsoft.AspNetCore.Mvc;
using SWARM.Shared;
using SWARM.Shared.DTO;
using System.Threading.Tasks;
using Telerik.DataSource;

namespace SWARM.Server.Controllers.Base
{
    public interface iBaseController<T>
    {
        Task<IActionResult> Delete(int itemID);
        Task<IActionResult> Get(int itemID);
        Task<IActionResult> Get();
        Task<IActionResult> Post([FromBody] T _Item);
        Task<IActionResult> Put([FromBody] T _Item);
    }
}