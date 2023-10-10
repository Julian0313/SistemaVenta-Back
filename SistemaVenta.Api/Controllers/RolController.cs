using Microsoft.AspNetCore.Mvc;
using SistemaVenta.Api.Utilidad;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : Controller
    {
        private readonly IRolService _rolService;
        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult>Lista()
        {
            var rsp = new Response<List<RolDTO>>();

            try
            {
                rsp.status = true;
                rsp.value = await _rolService.Lista();
            }
            catch(Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }
    }
}