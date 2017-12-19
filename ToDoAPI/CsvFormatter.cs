using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using ToDoAPI.Models;

namespace TodoApi
{
	/// <summary>
	/// 
	/// </summary>
	public class CsvFormatter : TextOutputFormatter
	{
		/// <summary>
		/// 
		/// </summary>
		public CsvFormatter()
		{
			SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
			SupportedEncodings.Add(Encoding.UTF8);
			SupportedEncodings.Add(Encoding.Unicode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		protected override bool CanWriteType(System.Type type)
		{
			return type == typeof(ToDoItem);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="selectedEncoding"></param>
		/// <returns></returns>
		public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
		{
			var response = context.HttpContext.Response;
			var buffer = new StringBuilder();
			if (context.Object is IEnumerable<ToDoItem>)
			{
				foreach (var todoItem in (IEnumerable<ToDoItem>)context.Object)
				{
					FormatCsv(buffer, todoItem);
				}
			}
			else
			{
				FormatCsv(buffer, (ToDoItem)context.Object);
			}

			using (var writer = context.WriterFactory(response.Body, selectedEncoding))
			{
				return writer.WriteAsync(buffer.ToString());
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="item"></param>
		private static void FormatCsv(StringBuilder buffer, ToDoItem item)
		{
			buffer.AppendLine($"{item.Id},\"{item.Name}\",{item.IsComplete}");
		}

	} // class
} // namespace
