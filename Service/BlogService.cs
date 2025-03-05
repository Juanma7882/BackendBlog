using blogPersonal.DbContext;
using blogPersonal.Entities;

namespace blogPersonal.Service
{
    public class BlogService
    {
        private readonly AppDbContext _appDbContext;

        public BlogService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> CrearBlog(Blog blog)
        {

        }

        public async Task<bool> EliminarBlogAsync(int id)
        {
            var blog = await _appDbContext.Blogs.FindAsync(id);
            if (blog == null)
            {
                return false; // Blog no encontrado
            }

            _appDbContext.Blogs.Remove(blog);
            await _appDbContext.SaveChangesAsync();
            return true; // Blog eliminado con éxito
        }

    }
}
