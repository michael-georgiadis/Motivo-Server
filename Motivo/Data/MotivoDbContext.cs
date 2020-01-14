using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Motivo.Data
{
	public class MotivoDbContext : IdentityDbContext<MotivoUser>
	{
		public DbSet<GoalDataModel> Goals { get; set; }

		public MotivoDbContext(DbContextOptions<MotivoDbContext> dbContextOptions) : base(dbContextOptions)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
