using System.Security.Claims;

namespace blogPersonal.Entities
{
    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string subject { get; set; }

        public static dynamic ValidarToken(ClaimsIdentity identity)
        {
            try
            {
                if (identity == null || !identity.Claims.Any())
                {
                    return new
                    {
                        success = false,
                        message = "Token inválido o vacío",
                        result = ""
                    };
                }

                // Extraer los valores de los claims
                var idClaim = identity.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
                var usuarioClaim = identity.Claims.FirstOrDefault(x => x.Type == "usuario")?.Value;

                if (string.IsNullOrEmpty(idClaim) || string.IsNullOrEmpty(usuarioClaim))
                {
                    return new
                    {
                        success = false,
                        message = "Claims no encontrados en el token",
                        result = ""
                    };
                }

                return new
                {
                    success = true,
                    message = "Token válido",
                    result = new
                    {
                        Id = idClaim,
                        Usuario = usuarioClaim
                    }
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = "Error al validar el token: " + ex.Message,
                    result = ""
                };
            }
        }
    }
}
