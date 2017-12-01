using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.Models
{
	/// <summary>
	/// Record definition
	/// </summary>
	public class ToDoItem
	{
		/// <summary>
		/// Key
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Textual name
		/// </summary>
		[Required]
		public string Name { get; set; }

		/// <summary>
		/// Set this True
		/// </summary>
		public bool IsComplete { get; set; }
	}
}