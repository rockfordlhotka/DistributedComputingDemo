using Autofac;
using KeyWatcher.Dependencies;
using KeyWatcher.Orleans.Contracts;
using Orleans;
using System;

namespace KeyWatcher.Azure
{
	public sealed class AzureModule
		: Module
	{
		private readonly bool useSignalForNotification;

		public AzureModule(bool useSignalForNotification) => 
			this.useSignalForNotification = useSignalForNotification;

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);
			builder.RegisterModule(new DependenciesModule(this.useSignalForNotification));
			builder.Register<Func<string, IUserGrain>>(c =>
				userName => GrainClient.GrainFactory.GetGrain<IUserGrain>(userName));
		}
	}
}