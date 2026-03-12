namespace TaskManagerProject.Modelos
{
    public class Tarea
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public bool Completada { get; set; }

        public DateTime? FechaFinalizacion { get; set; }


        // Relación con Usuario

        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

    }
}
