using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using ToDoAPI.Models;

// Docs for writing a custom formatter
// https://docs.microsoft.com/en-us/aspnet/core/mvc/advanced/custom-formatters

namespace TodoApi
{
	/// <summary>
	/// Custom HTTP formatter for CSV
	/// </summary>
	public class CsvFormatter : TextOutputFormatter
	{
		/// <summary>
		/// 
		/// </summary>
		public CsvFormatter()
		{
			SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
			SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/csv"));
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
			if (typeof(ToDoItem).IsAssignableFrom(type) || typeof(IEnumerable<ToDoItem>).IsAssignableFrom(type))
				return base.CanWriteType(type);
			else
				return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="selectedEncoding"></param>
		/// <returns></returns>
		public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
		{
			// Get logger
			IServiceProvider serviceProvider = context.HttpContext.RequestServices;
			var logger = serviceProvider.GetService(typeof(ILogger<ToDoItem>)) as ILogger;

			// Get HTTP response object
			var response = context.HttpContext.Response;
			var buffer = new StringBuilder();

			// Build the output
			if (context.Object is IEnumerable<ToDoItem>)
				foreach (var todoItem in (IEnumerable<ToDoItem>)context.Object)
					FormatCsv(buffer, todoItem);
			else
				FormatCsv(buffer, (ToDoItem)context.Object);

			// Write the output
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
