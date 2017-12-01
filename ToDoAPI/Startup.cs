using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Net.Http.Headers;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using ToDoAPI.Models;

// From https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api

namespace ToDoAPI
{
	/// <summary>
	/// Startup class
	/// </summary>
	public class Startup
	{
		/// <summary>
		/// Startup class constructor, with injected config
		/// </summary>
		/// <param name="configuration"></param>
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		/// <summary>
		/// 
		/// </summary>
		public IConfiguration Configuration { get; }

		/// <summary>
		/// This method gets called by the runtime. Use this method to add services to the container.
		/// </summary>
		/// <param name="services"></param>
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ToDoContext>(opt => opt.UseInMemoryDatabase("ToDoList")); // in-memory database

			// Respect request for XML or JSON responses from us
			// https://blog.jeremylikness.com/5-rest-api-designs-in-dot-net-core-1-29a8527e999c
			services.AddMvc(options =>
			{
				options.RespectBrowserAcceptHeader = true;
				options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("text/xml"));
				options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));
				options.Filters.Add(new Microsoft.AspNetCore.Mvc.RequireHttpsAttribute());
			})
			.AddXmlSerializerFormatters();

			// Register the Swagger generator, defining one or more Swagger documents
			services.AddSwaggerGen(c =>
				{
					c.SwaggerDoc("v1", new Info
					{
						Version = "v1",
						Title = "ToDo API",
						Description = "A simple example ASP.NET Core Web API",
						TermsOfService = "None",
						Contact = new Contact { Name = "Jimmy Bobby", Email = "", Url = "https://twitter.com/jimselders11" },
						License = new License { Name = "Unlicensed!", Url = "https://cbsnews.com" }
					});

					// Set the comments path for the Swagger JSON and UI.
					var basePath = PlatformServices.Default.Application.ApplicationBasePath;
					var xmlPath = Path.Combine(basePath, "ToDoApi.xml");
					c.IncludeXmlComments(xmlPath);
				});
		}

		/// <summary>
		/// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		/// </summary>
		/// <param name="app"></param>
		/// <param name="env"></param>
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				// Force TLS if not a development build/mode
				//options.Filters.Add(new Microsoft.AspNetCore.Mvc.RequireHttpsAttribute());
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();
			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API");
			});

			app.UseMvc();
		}

	} // class
} // namespace

