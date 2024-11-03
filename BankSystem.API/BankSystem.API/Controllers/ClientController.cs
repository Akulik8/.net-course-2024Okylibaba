using BankSystem.App.DTOs;
using BankSystem.App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers
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

        [HttpGet]
        public async Task<IActionResult> GetClientAsync([FromQuery] Guid id)
        {
            return Ok(await _clientService.GetClientAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddClient([FromQuery] ClientDto client)
        {
            if (client == null)
            {
                return BadRequest("Client cannot be null.");
            }

            await _clientService.AddClientAsync(client);
            return Created();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateClient(Guid id, [FromQuery] ClientDto clientDto)
        {
            await _clientService.UpdateClientAsync(id, clientDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteClient([FromQuery] Guid guid)
        {
            await _clientService.RemoveClientAsync(guid);
            return NoContent();
        }

        [HttpGet("FindClient")]
        public async Task<IActionResult> FindClient(string? name, string? surname, string? phoneNumber, string? pasNumber, DateOnly? date)
        {
            ClientDto response = await _clientService.FindClientAsync(name, surname, phoneNumber, pasNumber, date);
            if (response == null)
                return NotFound();

            return Ok(response);
        }

    }
}
