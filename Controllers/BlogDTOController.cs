using blogPersonal.DbContext;
using blogPersonal.DTOs;
using blogPersonal.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace blogPersonal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogDTOController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public BlogDTOController(AppDbContext context)
        {
            this._appDbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogDTO>>> GetBlogs()
        {
            var blogDto = await _appDbContext.Blogs
                .Select(b => new BlogDTO
                {
                    Id = b.Id,
                    Titulo = b.Titulo,
                    Descripcion = b.Descripcion,
                    Enlace = b.Enlace,
                    Etiquetas = b.Etiquetas,
                    FechaDePublicacion = b.FechaDePublicacion
                })
                .ToListAsync();

            if (!blogDto.Any())
            {
                return NotFound("No hay blogs disponibles.");
            }

            return Ok(blogDto);
        }
    }
}
