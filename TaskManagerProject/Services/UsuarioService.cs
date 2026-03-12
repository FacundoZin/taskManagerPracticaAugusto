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


        public Usuario CrearUsuario(string nombre, string email)
        {
            var usario = new Usuario
            {
                Nombre = nombre,
                Email = email
            };

            _context.Usuarios.Add(usario);

            _context.SaveChanges();

            return usario;
        }

        public Usuario EditarUsuario(int Id, string nombre, string email)
        {
            var usuario = _context.Usuarios.Where(u => u.Id == Id).FirstOrDefault();

            usuario.nombre = nombre;
            usuario.Email = email;

            _context.SaveChanges();

            return usuario;

        }

        public void EliminarUsuario(int IdUsuario)
        {
            var usuario = _context.Usuarios.Where(u => u.Id == IdUsuario).FirstOrDefault();

            if(usuario == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            var tareasActivas = usuario.tareas.where(t => t.completada == false).ToList();

            if(tareasActivas.conunt > 0)
            {
                throw new Exception("No se puede eliminar el usuario, tiene tareas activas");
            }

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();

            return;
        }

        public List<Usuario> ListarUsuarios()
        {
            var usaurios = _context.Usuarios.toList();

            return usaurios;
        }

        public Usuario ObtenerUsuario(int Id)
        {
            var usuario = _context.Usuarios.Where(u => u.Id == Id).FirstOrDefault();

            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            return usuario;
        }
    }
}
