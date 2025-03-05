using blogPersonal.DbContext;
using blogPersonal.Entities;
using blogPersonal.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blogPersonal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly BlogService _blogService;

        public BlogController(AppDbContext context, BlogService blogService)
        {
            this._appDbContext = context;
            _blogService = blogService;
        }

        [HttpPost]
        public async Task<ActionResult<Blog>> CrearPost(Blog blog)
        {
                if (blog == null)
                {
                    return BadRequest("Los datos del blog no pueden estar vacíos.");
                }   
                _appDbContext.Blogs.Add(blog);
                await _appDbContext.SaveChangesAsync();
                return CreatedAtAction("ObtenerBlog", new { id = blog.Id }, blog);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Blog>> ObtenerBlog(int id)
        {
            var blog = await _appDbContext.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return blog;
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Blog>> ActulizarBlog(int id, Blog blog)
        {
            if (id != blog.Id)
            {
                return BadRequest();
            }

            _appDbContext.Entry(blog).State = EntityState.Modified;

            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExiste(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("EliminarBlog{id}")]
        public async Task<ActionResult<Blog>> EliminarBlog(int id)
        {
            bool eliminado = await _blogService.EliminarBlogAsync(id);
            if (!eliminado)
            {
                return NotFound(new { success = false, message = "Blog no encontrado" });
            }

            return Ok(new { success = true, message = "Blog eliminado correctamente" });
        }

        private bool BlogExiste(int id)
        {
            return _appDbContext.Blogs.Any(e => e.Id == id);
        }
    }
}
