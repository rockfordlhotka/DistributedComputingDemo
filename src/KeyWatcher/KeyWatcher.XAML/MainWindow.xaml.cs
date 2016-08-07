using KeyWatcher.Messages;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;

namespace KeyWatcher.XAML
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private async void button_Click(object sender, RoutedEventArgs e)
		{
			var connection = new HubConnection("http://localhost:5944");
			var proxy = connection.CreateHubProxy("KeyWatcherHub");
			await connection.Start();

			var observable = from item in proxy.Observe("NotificationSent")
								  let m = item[0].ToObject<SignalRNotificationMessage>()
								  select m;
			var resultsSubscription = observable
				.ObserveOn(SynchronizationContext.Current)
				.Subscribe(message =>
				{
					this.listBox.Items.Add(message.Message);
				});
		}
	}
}
