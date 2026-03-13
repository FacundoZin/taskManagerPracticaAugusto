using Microsoft.AspNetCore.Mvc;
using TaskManagerProject.Dtos;
using TaskManagerProject.Modelos;
using TaskManagerProject.Services;

namespace TaskManagerProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TareasController : ControllerBase
    {
        private readonly TareaService _tareaService;

        public TareasController(TareaService tareaService)
        {
            _tareaService = tareaService;
        }

        // Caso de uso: Crear tarea
        [HttpPost]
        public async Task<ActionResult<Tarea>> Post([FromBody] CreateTareaDto request)
        {
            try
            {
                var tarea = await _tareaService.CrearTareaAsync(request);
                return CreatedAtAction(nameof(ListarPorUsuario), new { usuarioId = tarea.UsuarioId }, tarea);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Caso de uso: Editar tarea
        [HttpPut("{id}")]
        public async Task<ActionResult<Tarea>> Put(int id, [FromBody] Tarea request)
        {
            try
            {
                var tarea = await _tareaService.EditarTareaAsync(id, request.Titulo, request.Descripcion, request.FechaVencimiento);
                return Ok(tarea);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Caso de uso: Completar tarea
        [HttpPatch("{id}/completar")]
        public async Task<ActionResult<Tarea>> Completar(int id)
        {
            try
            {
                var tarea = await _tareaService.CompletarTareaAsync(id);
                return Ok(tarea);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Caso de uso: Listar tareas por usuario
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<List<Tarea>>> ListarPorUsuario(int usuarioId)
        {
            var tareas = await _tareaService.ListarTareasPorUsuarioAsync(usuarioId);
            return Ok(tareas);
        }

        // Caso de uso: Eliminar tarea
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _tareaService.EliminarTareaAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
