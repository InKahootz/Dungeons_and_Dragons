using Interfaces;
using System;
using System.Collections.Generic;
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
        Boolean isGM = false;

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

        public void InitializeServer(DuplexChannelFactory<IService> c, IService s) {
            channelFactory = c;
            server = s;
            user = server.User;
            isGM = user.isGM;
            this.DataContext = user;

            if (isGM) {
                tabAtrybuty.Visibility = System.Windows.Visibility.Hidden;
                tabUmiejetnosci.Visibility = System.Windows.Visibility.Hidden;
                tabWyposazenie.Visibility = System.Windows.Visibility.Hidden;
                tabControl.SelectedIndex = 3;
                classPanel.Visibility = System.Windows.Visibility.Hidden;
                labelRace.Visibility = System.Windows.Visibility.Hidden;
                labelGender.Visibility = System.Windows.Visibility.Hidden;
                
            }
        }
        #endregion

        public void refreshPlayer() {
            user = server.User;
        }

        public void receiveMessage(String msg) {
            chatListBox.Items.Insert(0, msg);
        }

        private void usersTimer_Tick(object sender, EventArgs e) {
            try {
                if (server.LoggedInUsers.Length == 0)
                    error("Wystąpił błąd. Nastąpi zamknięcie programu. Zaloguj się ponownie");

                usersListBox.Items.Clear();
                foreach (var user in server.LoggedInUsers)
                    usersListBox.Items.Add(user.Name);                

            } catch (Exception ee) {
                throw ee;
            }
        }

        #region Controls handling

        private void ChatSubmitButton_Click(object sender, RoutedEventArgs e) {
            String message = chatTextBox.Text;
            if (string.IsNullOrEmpty(message)) return;

            try {
                server.SendGlobalMessage(user.Name + ": " + message);
                chatListBox.Items.Insert(0, "Ty: " + message);
                chatTextBox.Text = "";

            } catch (Exception ee) {
                throw ee;
            }
        }
        private void chatTextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key != Key.Enter) return;
            e.Handled = true;
            ChatSubmitButton_Click(sender, e);
        }

        private void Window_Closed(object sender, EventArgs e) {
            try {
                if (logged) {
                    usersTimer.Stop();
                    server.Logout();
                    channelFactory.Close();
                }

            } catch (Exception ee) { 
                throw ee; 
            }
        }

        private void buttonCube_Click(object sender, RoutedEventArgs e) {            
            var loginWindow = new CubeWindow(this, user);
            loginWindow.Show();
        }

        private void buttonSaveNotes_Click(object sender, RoutedEventArgs e) {
            server.Update("`users`", "`notes`='" + textNotes.Text + "'", "`id`='" + user.Id + "'");
        }
        #endregion

        public void cubeThrow(String message) {
            try {
                server.SendGlobalMessage(message);
                chatListBox.Items.Insert(0, message);

            } catch (Exception) { throw; }
        }

        public void error(String message) {
            MessageBox.Show(message);
            this.Close();
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e) {
            try {
                server.RefreshPlayer(isGM);
            } catch (Exception) {
                
                throw;
            } 
        }
    }
}
