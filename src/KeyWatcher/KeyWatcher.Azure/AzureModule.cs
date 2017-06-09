using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.WebApi;
using KeyWatcher.Dependencies;
using KeyWatcher.Orleans.Contracts;
using Microsoft.AspNet.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using System;
using SR = System.Reflection;

namespace KeyWatcher.Azure
{
	public sealed class AzureModule
		: Module
	{
		private readonly IHubContext hub;
		private readonly IServiceCollection services;

		public AzureModule(IHubContext hub, IServiceCollection services)
		{
			this.hub = hub;
			this.services = services;
		}

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);
			builder.RegisterApiControllers(SR.Assembly.GetExecutingAssembly());
			builder.Populate(this.services);
			builder.RegisterModule(new DependenciesModule(this.hub));
			builder.Register<Func<string, IUserGrain>>(c =>
				userName => GrainClient.GrainFactory.GetGrain<IUserGrain>(userName));
		}
	}
}