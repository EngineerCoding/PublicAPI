using Microsoft.EntityFrameworkCore;

namespace PublicApi.Utils
{
	public static class DbContextExtensions
	{
		/// <summary>
		/// Truncates the table by simply removing all entries. Note that this method is very,
		/// very inefficient on large tables!
		/// </summary>
		/// <typeparam name="T">The entity to truncate</typeparam>
		/// <param name="dbContext"></param>
		public static void Truncate<T>(this DbContext dbContext)
			where T : class
		{
			DbSet<T> set = dbContext.Set<T>();
			set.RemoveRange(set);
			dbContext.SaveChanges();
		}
	}
}
