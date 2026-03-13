namespace TaskManagerProject.Dtos
{
    public class CreateTareaDto
    {
        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public bool Completada { get; set; } = false;


        // Relación con Usuario
        public int UsuarioId { get; set; }
    }
}
