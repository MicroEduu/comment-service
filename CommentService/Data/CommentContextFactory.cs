using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CommentService.Data
{
	public class CommentContextFactory : IDesignTimeDbContextFactory<CommentContext>
	{
		public CommentContext CreateDbContext(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			var optionsBuilder = new DbContextOptionsBuilder<CommentContext>();
			optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

			return new CommentContext(optionsBuilder.Options);
		}
	}
}