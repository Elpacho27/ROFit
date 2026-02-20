using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROFit.Model;
using ROFit.Service.Common;

namespace ROFit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> GetByEmail(Guid id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null) return NotFound();
            return Ok(client);
        }
    }
}
