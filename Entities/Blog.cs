namespace blogPersonal.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public required string  Titulo { get; set; }
        public required string  Parrafo { get; set; }
        public DateTime FechaDePublicacion { get; set; } = DateTime.UtcNow;
        public required string Descripcion { get; set; }
        public required string Enlace { get; set; }
        public List<string> Etiquetas { get; set; } = new List<string>();
    }
}
