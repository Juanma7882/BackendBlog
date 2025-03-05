using Microsoft.EntityFrameworkCore;
using blogPersonal.Entities;


namespace blogPersonal.DbContext
{
    public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Administrador> Administradores { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Blog>(tb =>
            {
                tb.ToTable("TbBlog");
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(col => col.Titulo)
               .IsRequired()
               .HasMaxLength(255); // Limitar a 255 caracteres

                tb.Property(col => col.Parrafo)
                    .IsRequired()
                    .HasColumnType("TEXT"); // Permite texto largo

                tb.Property(col => col.FechaDePublicacion)
             .HasDefaultValueSql("GETUTCDATE()"); // Fecha automática en SQL Server
            });

            modelBuilder.Entity<Administrador>(tb =>
            {
                tb.ToTable("Administrador");
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(col => col.Usuario).IsRequired().HasMaxLength(50);
                tb.Property(col => col.Contraseña).IsRequired();
            });

        }
    }
}
