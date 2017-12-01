using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ToDoAPI.Models;


// Swagger: https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?tabs=visual-studio

namespace TodoAPI.Controllers
{
	/// <summary>
	/// The ToDo controller
	/// </summary>
	[Route("api/ToDo")]
	//[Produces("application/json")]
	//[Produces("text/xml")]
	public class TodoController : Controller
	{
		private readonly ToDoContext _context;

		/// <summary>
		/// ToDo constructor, with injected db context
		/// </summary>
		/// <param name="context"></param>
		public TodoController(ToDoContext context)
		{
			_context = context;

			if (_context.ToDoItems.Count() == 0)
			{
				for (int i = 0; i < 9; i++)
					_context.ToDoItems.Add(new ToDoItem { Name = $"Item {i}, made in the controller constructor" });
				_context.SaveChanges();
			}
		}

		/// <summary>
		/// Creates a record from JSON body, and returns URL to new item
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     POST /ToDo
		///     {
		///        "id": 1,						// optional key, if omitted then db uses default
		///        "name": "Some item text",
		///        "isComplete": true
		///     }
		///
		/// </remarks>
		/// <param name="item"></param>
		/// <response code="201">Success; returns the newly-created item URL including id.</response>
		/// <response code="400">Supplied item is null.</response>
		/// <response code="500">Specified id already exists; record insertion failed.</response>
		[HttpPost]
		[ProducesResponseType(typeof(ToDoItem), 201)]
		[ProducesResponseType(typeof(ToDoItem), 400)]
		[ProducesResponseType(typeof(ToDoItem), 500)]
		public IActionResult Create([FromBody] ToDoItem item)
		{
			if (item == null)
			{
				return BadRequest();
			}

			_context.ToDoItems.Add(item);
			_context.SaveChanges();

			return CreatedAtRoute("GetToDo", new { id = item.Id }, item);
		}

		/// <summary>
		/// Updates specified record with new JSON body
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     PUT /ToDo/{id}
		///     {
		///        "id": 1,									// Must match URL param
		///        "name": "Some new text",
		///        "isComplete": true
		///     }
		///
		/// </remarks>
		/// <param name="id"></param>
		/// <param name="item"></param>
		/// <response code="204">Item was updated.</response>
		/// <response code="400">Specified item id did not match URL id.</response>
		/// <response code="404">Specified item was not found.</response>
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(ToDoItem), 204)]
		[ProducesResponseType(typeof(ToDoItem), 400)]
		[ProducesResponseType(typeof(ToDoItem), 404)]
		public IActionResult Update(long id, [FromBody] ToDoItem item)
		{
			if (item == null || item.Id != id)
			{
				return BadRequest();
			}

			var todo = _context.ToDoItems.FirstOrDefault(t => t.Id == id);
			if (todo == null)
			{
				return NotFound();
			}

			todo.IsComplete = item.IsComplete;
			todo.Name = item.Name;

			_context.ToDoItems.Update(todo);
			_context.SaveChanges();
			return new NoContentResult();
		}

		/// <summary>
		/// Deletes specified record
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     DELETE /ToDo/{id}
		///
		/// </remarks>
		/// <param name="id"></param>
		/// <response code="204">Specified item was deleted.</response>
		/// <response code="404">Specified item was not found.</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(ToDoItem), 204)]
		[ProducesResponseType(typeof(ToDoItem), 404)]
		public IActionResult Delete(long id)
		{
			var todo = _context.ToDoItems.FirstOrDefault(t => t.Id == id);
			if (todo == null)
			{
				return NotFound();
			}

			_context.ToDoItems.Remove(todo);
			_context.SaveChanges();
			return new NoContentResult();
		}

		/// <summary>
		/// Gets all records
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     GET /ToDo/
		///
		/// </remarks>
		/// <response code="200">All items in db are returned. Might be empty set.</response>
		[HttpGet]
		[ProducesResponseType(typeof(ToDoItem), 200)]
		public IEnumerable<ToDoItem> GetAll()
		{
			return _context.ToDoItems.ToList();
		}

		/// <summary>
		/// Gets a record by id key
		/// </summary>
		/// <param name="id"></param>
		/// <remarks>
		/// Sample request:
		///
		///     GET /ToDo/{id}
		///
		/// </remarks>
		/// <response code="200">Specified item is returned.</response>
		/// <response code="404">Specified item was not found.</response>
		[HttpGet("{id}", Name = "GetToDo")]
		[ProducesResponseType(typeof(ToDoItem), 200)]
		[ProducesResponseType(typeof(ToDoItem), 404)]
		public IActionResult GetById(long id)
		{
			var item = _context.ToDoItems.FirstOrDefault(t => t.Id == id);
			if (item == null)
			{
				return NotFound();
			}
			return new ObjectResult(item);
		}

	} // class
} // namespace