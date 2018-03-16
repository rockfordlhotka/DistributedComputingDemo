using Autofac;
using KeyWatcher.Orleans.Contracts;
using Orleans;
using System;

namespace KeyWatcher.Signaled.API
{
	public sealed class ApiModule
		: Module
	{
		private readonly IClusterClient client;

		public ApiModule(IClusterClient client) => 
			this.client = client ?? throw new ArgumentNullException(nameof(client));

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);
			builder.Register<Func<string, IUserGrain>>(c =>
				userName => this.client.GetGrain<IUserGrain>(userName));
		}
	}
}
