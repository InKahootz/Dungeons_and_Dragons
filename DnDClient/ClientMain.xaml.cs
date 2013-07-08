using Interfaces;
using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DnDClient
{
    public partial class ClientMainWindow : Window
    {
        public DuplexChannelFactory<IService> channelFactory;
        IService server;
        DispatcherTimer usersTimer;
        Boolean logged = false;
        User user;

        #region Class Constructor and Initialization of service variables
        public ClientMainWindow() {
            InitializeComponent();
            var loginWindow = new ClientLoginWindow(this);
            if (loginWindow.ShowDialog() == true) {
                logged = true;
            } else {
                this.Close();
                return;
            }

            usersTimer = new DispatcherTimer();
            usersTimer.Tick += new EventHandler(usersTimer_Tick);
            usersTimer.Interval = new TimeSpan(0, 0, 1);
            usersTimer.Start();
        }

        public void InitializeServer(User u, DuplexChannelFactory<IService> c, IService s) {
            user = u;
            channelFactory = c;
            server = s;
        }
        #endregion

        public void receiveMessage(String msg) {
            chatListBox.Items.Add(user.UserName + ": " + msg);
        }

        private void usersTimer_Tick(object sender, EventArgs e) {
            try {
                if (server.LoggedInUsers.Length == 0)
                    error("Wystąpił błąd. Nastąpi zamknięcie programu. Zaloguj się ponownie");

                usersListBox.Items.Clear();
                foreach (var user in server.LoggedInUsers)
                    usersListBox.Items.Add(user.UserName);

            } catch (Exception) {
                throw new System.Exception();
            }
        }

        #region Controls handling
        private void Button_Click(object sender, RoutedEventArgs e) {
            String message = chatTextBox.Text;
            if (string.IsNullOrEmpty(message)) return;

            try {
                server.SendGlobalMessage(message);
                chatListBox.Items.Add("Ty: " + message);
                chatTextBox.Text = "";

            } catch (Exception) {
                throw new System.Exception();
            }
        }
        private void chatTextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key != Key.Enter) return;
            e.Handled = true;
            Button_Click(sender, e);
        }

        private void Window_Closed(object sender, EventArgs e) {
            try {
                if (logged) {
                    usersTimer.Stop();
                    server.Logout();
                    channelFactory.Close();
                }

            } catch (Exception) { 
                throw new System.Exception(); 
            }
        }
        #endregion

        public void error(String message) {
            MessageBox.Show(message);
            this.Close();
        }
    }
}
