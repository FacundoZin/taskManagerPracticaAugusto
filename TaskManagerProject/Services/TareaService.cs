using Microsoft.EntityFrameworkCore;
using TaskManagerProject.DB;
using TaskManagerProject.Dtos;
using TaskManagerProject.Modelos;

namespace TaskManagerProject.Services
{
    public class TareaService
    {
        private readonly AppDBContext _context;

        public TareaService(AppDBContext context)
        {
            _context = context;
        }

        // Caso de uso: Crear tarea
        public async Task<Tarea> CrearTareaAsync(CreateTareaDto nuevaTarea)
        {
            // Regla de negocio: No se puede crear una tarea con fecha vencida
            if (nuevaTarea.FechaVencimiento < DateTime.Now)
            {
                throw new Exception("No se puede crear una tarea con fecha de vencimiento en el pasado.");
            }

            // Validar que el usuario existe
            var usuario = await _context.Usuarios
                .Include(u => u.Tareas)
                .FirstOrDefaultAsync(u => u.Id == nuevaTarea.UsuarioId);

            if (usuario == null)
            {
                throw new Exception("El usuario especificado no existe.");
            }

            // Regla de negocio: Un usuario no puede tener más de 5 tareas pendientes
            var tareasPendientes = usuario.Tareas.Count(t => !t.Completada);
            if (tareasPendientes >= 5)
            {
                throw new Exception("El usuario ya tiene 5 tareas pendientes. Debe completar alguna antes de crear una nueva.");
            }

            nuevaTarea.FechaCreacion = DateTime.Now;
            nuevaTarea.Completada = false;

            var tarea = new Tarea
            {
                Titulo = nuevaTarea.Titulo,
                Descripcion = nuevaTarea.Descripcion,
                FechaCreacion = nuevaTarea.FechaCreacion,
                FechaVencimiento = nuevaTarea.FechaVencimiento,
                Completada = nuevaTarea.Completada,
                UsuarioId = nuevaTarea.UsuarioId
            };

            await _context.Tareas.AddAsync(tarea);
            await _context.SaveChangesAsync();

            return tarea;
        }

        // Caso de uso: Editar tarea
        public async Task<Tarea> EditarTareaAsync(int id, string titulo, string descripcion, DateTime fechaVencimiento)
        {
            var tarea = await _context.Tareas.FirstOrDefaultAsync(t => t.Id == id);
            if (tarea == null)
            {
                throw new Exception("Tarea no encontrada.");
            }

            if (fechaVencimiento < DateTime.Now && !tarea.Completada)
            {
                throw new Exception("No se puede establecer una fecha de vencimiento en el pasado.");
            }

            tarea.Titulo = titulo;
            tarea.Descripcion = descripcion;
            tarea.FechaVencimiento = fechaVencimiento;

            await _context.SaveChangesAsync();
            return tarea;
        }

        // Caso de uso: Completar tarea
        public async Task<Tarea> CompletarTareaAsync(int id)
        {
            var tarea = await _context.Tareas.FirstOrDefaultAsync(t => t.Id == id);
            if (tarea == null)
            {
                throw new Exception("Tarea no encontrada.");
            }

            if (tarea.Completada)
            {
                throw new Exception("La tarea ya está completada.");
            }

            // Regla de negocio: Cuando se completa una tarea se guarda FechaFinalizacion
            tarea.Completada = true;
            tarea.FechaFinalizacion = DateTime.Now;

            await _context.SaveChangesAsync();
            return tarea;
        }

        // Caso de uso: Listar tareas por usuario
        public async Task<List<Tarea>> ListarTareasPorUsuarioAsync(int usuarioId)
        {
            return await _context.Tareas
                .Where(t => t.UsuarioId == usuarioId)
                .ToListAsync();
        }

        // Caso de uso: Eliminar tarea
        public async Task EliminarTareaAsync(int id)
        {
            var tarea = await _context.Tareas.FirstOrDefaultAsync(t => t.Id == id);
            if (tarea == null)
            {
                throw new Exception("Tarea no encontrada.");
            }

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();
        }
    }
}
