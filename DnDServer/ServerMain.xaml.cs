using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Media;

namespace DnDServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ServerMainWindow : Window
    {
        ServiceHost host;
        ServerImplementation chatService;
        Boolean opened = false;

        public ServerMainWindow() {
            InitializeComponent();

            statusLabel.Content = "Not running";
            statusLabel.Foreground = Brushes.Red;

            chatService = new ServerImplementation(this);
            host = new ServiceHost(chatService);
        }

        public void Message(String msg) {
            statusListBox.Items.Add(msg);
        }

        private void toggleButton_Click(object sender, RoutedEventArgs e) {
            if (opened) {
                chatService.Disconnect();
                host.Close();
                opened = false;
                toggleButton.Content = "Włącz";
                statusLabel.Content = "Not running";
                statusLabel.Foreground = Brushes.Red;
            } else {
                host = new ServiceHost(chatService);
                host.Open();
                opened = true;
                toggleButton.Content = "Wyłącz";
                statusLabel.Content = "Running";
                statusLabel.Foreground = Brushes.Green;
            }
        }

        private void Window_Closed(object sender, EventArgs e) {
            if (opened) host.Close();
        }
    }
}
