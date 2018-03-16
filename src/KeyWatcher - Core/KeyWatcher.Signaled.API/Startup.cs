using Autofac;
using KeyWatcher.Orleans.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Runtime;
using Polly;
using System;
using System.Threading.Tasks;

namespace KeyWatcher.Signaled.API
{
	public class Startup
	{
		private static readonly TimeSpan Retry = TimeSpan.FromSeconds(1);
		private const int RetryAttempts = 5;

		public Startup(IConfiguration configuration) => 
			this.Configuration = configuration;

		public void ConfigureServices(IServiceCollection services) => 
			services.AddMvc();

		public void ConfigureContainer(ContainerBuilder builder) => 
			builder.RegisterModule(new ApiModule(Startup.GetClusterClient().Result));

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc();
		}

		private static async Task<IClusterClient> GetClusterClient(
			int initializeAttemptsBeforeFailing = Startup.RetryAttempts)
		{
			async Task<IClusterClient> GetClientAsync()
			{
				var client = new ClientBuilder()
					.UseLocalhostClustering()
					.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IUserGrain).Assembly).WithReferences())
					.Build();

				await client.Connect();

				return client;
			}

			var policy = Policy.Handle<SiloUnavailableException>().WaitAndRetryAsync(
				initializeAttemptsBeforeFailing, retryAttempt =>
				{
					return Startup.Retry;
				});
			return await policy.ExecuteAsync(GetClientAsync);
		}

		public IConfiguration Configuration { get; }
	}
}
