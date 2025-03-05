namespace blogPersonal.Entities
{
    public class Administrador
    {
        public int Id { get; set; }
        public required string Usuario { get; set; }
        public required string Contraseña { get; set; }
    }
}
