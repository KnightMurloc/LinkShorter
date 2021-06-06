using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LinkShorter
{
	public class Startup
	{
		public IConfiguration Configuration;

		public void ConfigureServices(IServiceCollection services)
		{
			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.SetBasePath(Directory.GetCurrentDirectory())
				   .AddJsonFile("appsettings.json");

			Configuration = builder.Build();
			string ConnectionStr = Configuration.GetConnectionString("DefaultConnection");
			services.AddDbContext<LinkContext>(options => options.UseSqlite(ConnectionStr));
			SQLitePCL.Batteries.Init();

			services.AddControllers();

			services.AddEntityFrameworkSqlite().AddDbContext<LinkContext>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.Map("/new", New);


			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.Run(async (context) =>
			{

				if (context.Request.Path == "/")
				{
					context.Response.Redirect("/new");
				}
				else
				{
					await context.Response.WriteAsync("not found");
				}

			});

		}

		public static void New(IApplicationBuilder app)
		{
			app.Run(async (context) =>
			{
				await context.Response.WriteAsync(File.ReadAllText("index.html"));
			});
		}
	}
}
