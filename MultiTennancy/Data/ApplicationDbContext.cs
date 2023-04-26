using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MultiTennancy.Entidades;
using MultiTennancy.Servicios;
using System.Linq.Expressions;
using System.Reflection;

namespace MultiTennancy.Data
{

    //inyectamos en el constructor de la clase la interface IservicioTenant
    //y creamos el campo tenantId

    //despues dentro del constructor activamos el campo tenantId
    //tenantId = servicioTenant.ObtenerTenant();

    //De esta manera podemos llamar al metodo que contiene la interfaz
    //Iserviciotenant que la llamamos serviciotenant como variable para inicializar
    //ponemos el . y agregamos obtenertenant() que es metodo que contiene la interfaz

    public class ApplicationDbContext : IdentityDbContext
    {
        private string tenantId;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IServicioTenant servicioTenant)
            : base(options)
        {
            tenantId = servicioTenant.ObtenerTenant();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var item in ChangeTracker.Entries().Where(e => e.State == EntityState.Added
            && e.Entity is IEntidadTenant))
            {
                if (string.IsNullOrEmpty(tenantId))
                {
                    throw new Exception("TenantId no encontrado al momento de crear el registro");
                }

                var entidad = item.Entity as IEntidadTenant;
                entidad!.TenantId = tenantId;
            }
            return base.SaveChangesAsync(cancellationToken); 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Pais>().HasData(new Pais[]
                {
                    new Pais{Id = 1, Nombre = "España"},
                    new Pais{Id = 2, Nombre = "Estados Unidos"},
                    new Pais{Id = 3, Nombre = "Republica Dominicana"}
                });

            foreach (var entidad in builder.Model.GetEntityTypes())
            {
                var tipo = entidad.ClrType;

                if(typeof(IEntidadTenant).IsAssignableFrom(tipo))
                {
                    //armar el filtro
                    var metodo = typeof(ApplicationDbContext)
                        .GetMethod(nameof(ArmarFiltroGlobalTenantent),
                        BindingFlags.NonPublic | BindingFlags.Static
                        )?.MakeGenericMethod(tipo);

                    var filtro = metodo?.Invoke(null, new object[] { this })!; 
                    entidad.SetQueryFilter((LambdaExpression)filtro);
                    entidad.AddIndex(entidad.FindProperty(nameof(IEntidadTenant.TenantId))!);

                    
                }
                else if (tipo.DebeSaltarValidacionTnenat())
                {
                    continue;
                }
                else
                {
                    throw new Exception($"La entidad {entidad} no ha sido marcada como tenant");
                }
            }
        }

        private static LambdaExpression ArmarFiltroGlobalTenantent<TEntidad>(ApplicationDbContext context)
            where TEntidad: class, IEntidadTenant
        {
            Expression<Func<TEntidad, bool>> filtro = x => x.TenantId == context.tenantId;
            return filtro;
        }

        //agregamos los dbset correspondientes, en este caso a las dos entidades pais y producto
        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Pais> Paises => Set<Pais>();
    }
}