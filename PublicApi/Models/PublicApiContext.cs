using Microsoft.EntityFrameworkCore;

namespace PublicApi.Models
{
	public class PublicApiContext : DbContext
	{
		public DbSet<BlogPost> BlogPosts { get; set; }

		public PublicApiContext(DbContextOptions<PublicApiContext> contextOptions)
			: base(contextOptions)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<BlogPost>().HasIndex("Slug");
			modelBuilder.Entity<BlogPost>().Property("PrimaryKey").ValueGeneratedOnAdd();
		}
	}
}
