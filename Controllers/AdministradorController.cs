using blogPersonal.DbContext;
using blogPersonal.Entities;
using blogPersonal.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace blogPersonal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradorController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public IConfiguration _configuration { get; set; }
        private readonly BlogService _blogService;


        public AdministradorController(AppDbContext appDbContext, IConfiguration configuration, BlogService blogService)
        {
            this._appDbContext = appDbContext;
            this._configuration = configuration;
            _blogService = blogService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> IniciarSesion([FromBody] Administrador administrador)
        {
            try
            {

                if (administrador == null || string.IsNullOrEmpty(administrador.Usuario) || string.IsNullOrEmpty(administrador.Contraseña))
                {
                    return BadRequest(new { success = false, message = "Usuario y contraseña requeridos" });
                }

                // Buscar el usuario en la base de datos
                var data = await _appDbContext.Administradores.FirstOrDefaultAsync(a => a.Usuario == administrador.Usuario);

                if (data == null || data.Contraseña != administrador.Contraseña) // Aquí deberías usar hash en producción
                {
                    return Unauthorized(new { success = false, message = "Credenciales incorrectas" });
                }

                // Generar Token JWT
                var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key no configurada");
                var jwtIssuer = _configuration["Jwt:Issuer"];
                var jwtAudience = _configuration["Jwt:Audience"];

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, data.Usuario),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", data.Id.ToString()),
                new Claim("usuario", data.Usuario)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    jwtIssuer,
                    jwtAudience,
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(5), // Token válido por 30 min
                    signingCredentials: signIn
                );

                return Ok(new
                {
                    success = true,
                    message = "Inicio de sesión exitoso",
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            catch (Exception ex)
            {
                return BadRequest("error en login" + ex);
            }
        }


        [HttpPost]
        [Route("AgregarUnAdministrador")]
        public async Task<ActionResult<Administrador>> CrearPost(Administrador administrador)
        {
            if (administrador == null)
            {
                return BadRequest("Los datos del administrador no pueden estar vacíos.");
            }
            _appDbContext.Administradores.Add(administrador);
            await _appDbContext.SaveChangesAsync();

            return CreatedAtAction("ObtenerAdministrador", new { id = administrador.Id }, administrador);
        }


        //[HttpGet("{id}")]
        //[Route("ObtenerAdministrador")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Administrador>> ObtenerAdministrador(int id)
        {
            var administrador = await _appDbContext.Administradores.FindAsync(id);
            if (administrador == null)
            {
                return NotFound();
            }
            return Ok(administrador);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Administrador>> ActulizarAdministrador(int id, Administrador administrador)
        {
            if (id != administrador.Id)
            {
                return BadRequest();
            }

            _appDbContext.Entry(administrador).State = EntityState.Modified;

            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdministradorExiste(id))
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<Administrador>> EliminarAdministrador(int id)
        {
            var administrador = await _appDbContext.Administradores.FindAsync(id);
            if (administrador == null)
            {
                return NotFound();
            }

            _appDbContext.Administradores.Remove(administrador);
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool AdministradorExiste(int id)
        {
            return _appDbContext.Administradores.Any(e => e.Id == id);
        }

        //[Route("OptenerToken")]
        [HttpDelete("EliminarBlog{Id}")]
        [Authorize]
        public async Task<IActionResult> AdministradorEliminarBlog(int Id)
        {
            try
            {

                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null)
                {
                    return NotFound(new { success = true, message = "Eliminar BLOG SUCCCES ERROR" });

                }

                var resultado = Jwt.ValidarToken(identity);

                if (!resultado.success)
                {
                    return Unauthorized(resultado);
                }

                bool eliminado = await _blogService.EliminarBlogAsync(Id);
                if (!eliminado)
                {
                    return NotFound(new { success = false, message = "Blog no encontrado" });
                }
                return Ok(new { success = true, message = "Blog eliminado correctamente" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

    }
}
