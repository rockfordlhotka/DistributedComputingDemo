using Autofac;
using KeyWatcher.Dependencies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace KeyWatcher.Azure
{
	public class Startup
	{
		public Startup(IConfiguration configuration) =>
			this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();
			services.AddSignalR();
		}

		public void ConfigureContainer(ContainerBuilder builder) => 
			builder.RegisterModule(new AzureModule(true));

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc();
			app.UseSignalR(routes =>
			{
				routes.MapHub<KeyWatcherHub>("keys");
			});
		}
	}
}
