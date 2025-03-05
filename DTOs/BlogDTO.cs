using Microsoft.EntityFrameworkCore;

 

namespace blogPersonal.DTOs
{
    public class BlogDTO
    {
        //OBJETOS DE TRANSFERENCIA DE DATOS = DATA TRANSFER OBJECTS = DTO
        // con esta clase podemos mostrar en la respuesta de la api los datos nesesarios
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Descripcion { get; set; }
        public required string Enlace { get; set; }
        public List<string> Etiquetas { get; set; } = new List<string>();
        public DateTime FechaDePublicacion { get; set; } = DateTime.UtcNow;
    }
}
