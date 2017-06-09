using KeyWatcher.Messages;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Reactive.Linq;
using System.Windows;
using KeyWatcher.XAML.Extensions;

namespace KeyWatcher.XAML
{
	public partial class MainWindow : Window
	{
		private HubConnection connection;
		private IHubProxy proxy;
		private IDisposable resultsSubscription;

		public MainWindow() =>
			this.InitializeComponent();

		protected override void OnClosed(EventArgs e)
		{
			this.resultsSubscription.SafeDispose();

			if(this.connection != null)
			{
				this.connection.Stop();
			}

			base.OnClosed(e);
		}

		private async void OnStartListeningClick(object sender, RoutedEventArgs e)
		{
			// For console app locally...
			//this.connection = new HubConnection("http://localhost:5944");

			// For WebApi app locally...
			this.connection = new HubConnection("http://localhost:6344");

			// For Azure instance...
			//this.connection = new HubConnection("http://keywatcher.azurewebsites.net");

			this.proxy = this.connection.CreateHubProxy("KeyWatcherHub");
			await this.connection.Start();

			this.proxy.On<SignalRNotificationMessage>(
				"NotificationSent", message =>
				{
					this.Dispatcher.Invoke(() =>
						this.notifications.Items.Add($"On() - {message.Message}"));
				});

			this.resultsSubscription = this.proxy
				.ObserveAs<SignalRNotificationMessage>("NotificationSent")
				.Where(message => message.Message.Contains("cotton"))
				.Take(3)
				.Delay(TimeSpan.FromSeconds(2))
				.Subscribe(message => 
				{
					this.Dispatcher.Invoke(() =>
						this.notifications.Items.Add($"Subscribe() - {message.Message}"));
				});

			this.startListening.IsEnabled = false;
		}
	}
}
