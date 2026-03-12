using Microsoft.EntityFrameworkCore;
using TaskManagerProject.DB;
using TaskManagerProject.Modelos;

namespace TaskManagerProject.Services
{
    public class UsuarioService
    {
        private readonly AppDBContext _context;

        public UsuarioService(AppDBContext context) 
        { 
            _context = context;
        }

        public async Task<Usuario> CrearUsuarioAsync(string nombre, string email)
        {
            var usuario = new Usuario
            {
                Nombre = nombre,
                Email = email
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task<Usuario> EditarUsuarioAsync(int id, string nombre, string email)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            usuario.Nombre = nombre;
            usuario.Email = email;

            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task EliminarUsuarioAsync(int idUsuario)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Tareas)
                .FirstOrDefaultAsync(u => u.Id == idUsuario);

            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            var tieneTareasActivas = usuario.Tareas.Any(t => !t.Completada);

            if (tieneTareasActivas)
            {
                throw new Exception("No se puede eliminar el usuario, tiene tareas activas");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Usuario>> ListarUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> ObtenerUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            return usuario;
        }
    }
}
