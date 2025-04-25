using System;
using System.Threading.Tasks;
using CollaborativeBoardApi.Hubs;
using CollaborativeBoardApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CollaborativeBoardApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardController : ControllerBase
    {
        private readonly IHubContext<BoardHub> _hubContext;

        public BoardController(IHubContext<BoardHub> hubContext)
        {
            _hubContext = hubContext;
        }

        // Ejemplo de endpoint para limpiar el tablero
        [HttpPost("clear")]
        public async Task<IActionResult> ClearBoard()
        {
            await _hubContext.Clients.All.SendAsync("BoardCleared");
            return Ok(new { message = "Board cleared successfully" });
        }

        // Podríamos añadir más endpoints según sea necesario
    }
}