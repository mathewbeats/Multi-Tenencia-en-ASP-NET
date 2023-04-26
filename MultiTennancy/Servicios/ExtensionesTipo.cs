using Microsoft.AspNetCore.Identity;
using MultiTennancy.Entidades;

namespace MultiTennancy.Servicios
{
    public static class ExtensionesTipo
    {
        public static bool DebeSaltarValidacionTnenat(this Type tipo)
        {
            var booleanos = new List<bool>()
            {
                tipo.IsAssignableFrom(typeof(IdentityRole)),
                tipo.IsAssignableFrom(typeof(IdentityRoleClaim<string>)),
                tipo.IsAssignableFrom(typeof(IdentityUser)),
                tipo.IsAssignableFrom(typeof(IdentityUserLogin<string>)),
                tipo.IsAssignableFrom(typeof(IdentityUserRole<string>)),
                tipo.IsAssignableFrom(typeof(IdentityUserToken<string>)),
                tipo.IsAssignableFrom(typeof(IdentityUserClaim<string>)),
                typeof(IEntidadComun).IsAssignableFrom(tipo)
            };

            var resultado = booleanos.Aggregate((b1, b2) => b1 || b2);

            return resultado;
        }
    }
}
