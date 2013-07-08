using Interfaces;
using System;
using System.ServiceModel;
using System.Windows;

namespace DnDClient
{
    public partial class ClientLoginWindow : Window
    {
        private ClientMainWindow _MainWindow;
        public DuplexChannelFactory<IService> channelFactory;
        public IService server;

        public ClientLoginWindow(ClientMainWindow window) {
            InitializeComponent();
            _MainWindow = window;
        }

        private void submitButton_Click(object sender, RoutedEventArgs e) {
            var login = loginTextBox.Text;
            var pass = passwordTextBox.Text;

            try {
                channelFactory = new DuplexChannelFactory<IService>(new ClientImplementation(_MainWindow), "DnDServiceEndPoint");
                server = channelFactory.CreateChannel();

                if (server.Login(login, pass)) {
                    _MainWindow.InitializeServer(server.User, channelFactory, server);
                    this.DialogResult = true;
                } else {
                    statusLabel.Content = "Login lub hasło nie są poprawne!";
                    return;
                }

            } catch (Exception ex) {
                statusLabel.Content = "Nastąpił błąd! Spróbuj ponownie";
                System.IO.StreamWriter file = new System.IO.StreamWriter("log.txt");
                file.WriteLine(ex.ToString());
                file.Close();
                return;
            } 
            this.Close();
        }
    }
}
