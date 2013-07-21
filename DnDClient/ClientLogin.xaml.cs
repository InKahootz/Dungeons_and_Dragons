using Interfaces;
using System;
using System.Reflection;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;

namespace DnDClient
{
    public partial class ClientLoginWindow : Window
    {
        private ClientMainWindow _MainWindow;
        public DuplexChannelFactory<IService> channelFactory;
        public IService server;
        private Version GetRunningVersion() {
            try {
                return System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
            } catch {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        public ClientLoginWindow(ClientMainWindow window) {
            InitializeComponent();
            _MainWindow = window;
            versionLabel.Content = GetRunningVersion().ToString();
            Keyboard.Focus(loginTextBox);
        }

        private void submitButton_Click(object sender, RoutedEventArgs e) {
            var login = loginTextBox.Text;
            var pass = passwordTextBox.Password;

            Action<String> status = s => {
                statusLabel.Content = s;
                statusLabel.ToolTip = s;
            };

            try {
                channelFactory = new DuplexChannelFactory<IService>(new ClientImplementation(_MainWindow), "DnDServiceEndPoint");
                server = channelFactory.CreateChannel();

                if (server.Login(login, pass)) {
                    _MainWindow.InitializeServer(channelFactory, server);
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

        private void passwordTextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key != Key.Enter) return;
            e.Handled = true;
            submitButton_Click(sender, e);
        }
    }
}
