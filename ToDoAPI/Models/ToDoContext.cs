using Microsoft.EntityFrameworkCore;

namespace ToDoAPI.Models
{
	/// <summary>
	/// 
	/// </summary>
	public class ToDoContext : DbContext
	{
		/// <summary>
		///  Constructor with injected context options
		/// </summary>
		/// <param name="options"></param>
		public ToDoContext(DbContextOptions<ToDoContext> options)
				: base(options)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public DbSet<ToDoItem> ToDoItems { get; set; }

	} // class
} // namespace
