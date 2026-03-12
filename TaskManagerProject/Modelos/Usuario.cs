namespace TaskManagerProject.Modelos
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Email { get; set; }

        public bool Activo { get; set; } = true;


        // relacion con tareas 
        public List<Tarea> Tareas { get; set; } = new List<Tarea>();

    }
}
