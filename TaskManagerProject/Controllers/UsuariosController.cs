using Microsoft.AspNetCore.Mvc;
using TaskManagerProject.Modelos;
using TaskManagerProject.Services;

namespace TaskManagerProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> Get()
        {
            var usuarios = await _usuarioService.ListarUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> Get(int id)
        {
            try
            {
                var usuario = await _usuarioService.ObtenerUsuarioAsync(id);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> Post([FromBody] Usuario request)
        {
            var usuario = await _usuarioService.CrearUsuarioAsync(request.Nombre, request.Email);
            return CreatedAtAction(nameof(Get), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Usuario>> Put(int id, [FromBody] Usuario request)
        {
            try
            {
                var usuario = await _usuarioService.EditarUsuarioAsync(id, request.Nombre, request.Email);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _usuarioService.EliminarUsuarioAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
