using Microsoft.AspNetCore.Mvc;
using SistemaVenta.Api.Utilidad;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashBoardService _dashBoardService;
        public DashBoardController(IDashBoardService dashBoardService)
        {
            _dashBoardService = dashBoardService;
        }
        
        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var rsp = new Response<DashBoardDTO>();
            try
            {
                rsp.status = true;
                rsp.value = await _dashBoardService.Resumen();
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