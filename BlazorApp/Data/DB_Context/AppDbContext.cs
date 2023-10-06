using BlazorApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            //optionsBuilder.UseSqlServer("Data Source=RUDI-DELL-XPS\\GNTSQLSERVER;Initial Catalog=SYSTEMX_DB;User ID=sa;Password=sapwd;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            optionsBuilder.UseSqlServer("Server=tcp:system-x-azure-server-01.database.windows.net,1433;Initial Catalog=Azure-DB-01;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";");
        }
    }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<User>(entity =>
    //    {
    //        //entity.Property(e => e.CreatedByFkUserId).HasDefaultValueSql("((0))");

    //        //entity.Property(e => e.PwdExpInterval).HasDefaultValueSql("((0))");

    //        //entity.Property(e => e.RowTimeStamp).HasDefaultValueSql("(getdate())");
    //    });

    //    //modelBuilder.Entity<ClmUser>(entity =>
    //    //{
    //    //    entity.Property(e => e.UserCode).IsFixedLength();
    //    //});

    //    OnModelCreatingPartial(modelBuilder);
    //}

    //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
