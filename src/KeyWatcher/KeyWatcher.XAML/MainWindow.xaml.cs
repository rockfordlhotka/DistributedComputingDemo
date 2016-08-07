using KeyWatcher.Messages;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using KeyWatcher.XAML.Extensions;

namespace KeyWatcher.XAML
{
	public partial class MainWindow : Window
	{
		private HubConnection connection;
		private IHubProxy proxy;
		private IDisposable resultsSubscription;

		public MainWindow()
		{
			this.InitializeComponent();
		}

		protected override void OnClosed(EventArgs e)
		{
			this.resultsSubscription.SafeDispose();
			this.connection.Stop();
			base.OnClosed(e);
		}

		private async void OnStartListeningClick(object sender, RoutedEventArgs e)
		{
			this.connection = new HubConnection("http://localhost:5944");
			this.proxy = this.connection.CreateHubProxy("KeyWatcherHub");
			await this.connection.Start();

			this.resultsSubscription = this.proxy
				.ObserveAs<SignalRNotificationMessage>("NotificationSent")
				.ObserveOn(SynchronizationContext.Current)
				.Subscribe(message =>
				{
					this.notifications.Items.Add(message.Message);
				});

			this.startListening.IsEnabled = false;
		}
	}
}
