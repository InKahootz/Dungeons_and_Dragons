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
            this.DataContext = user;
            refreshPlayer();
        }
        #endregion

        public void refreshPlayer() {
            //user = server.User;

            //labelName.Content = user["login"];
            //labelRace.Content = user["race"];
            //labelGender.Content = user["gender"];

            //labelClass.Content = user["class1"];
            //labelLevel.Content = user["class1_lvl"];
            //labelExp.Content = user["experience"];

            //labelStrength.Content = user["strength"];
            //labelStrengthModifier.Content = ((int.Parse(user["strengthModifier"]) > 0) ? "+" : "") + user["strengthModifier"];

            //labelDexterity.Content = user["dexterity"];
            //labelDexterityModifier.Content = ((int.Parse(user["dexterityModifier"]) > 0) ? "+" : "") + user["dexterityModifier"];

            //labelBuild.Content = user["build"];
            //labelBuildModifier.Content = ((int.Parse(user["buildModifier"]) > 0) ? "+" : "") + user["buildModifier"];

            //labelIntellect.Content = user["intellect"];
            //labelIntellectModifier.Content = ((int.Parse(user["intellectModifier"]) > 0) ? "+" : "") + user["intellectModifier"];

            //labelPrudence.Content = user["prudence"];
            //labelPrudenceModifier.Content = ((int.Parse(user["prudenceModifier"]) > 0) ? "+" : "") + user["prudenceModifier"];

            //labelCharisma.Content = user["charisma"];
            //labelCharismaModifier.Content = ((int.Parse(user["charismaModifier"]) > 0) ? "+" : "") + user["charismaModifier"];

            //labelPerseverance.Content = (int.Parse(user["perseverance"]) + int.Parse(user["buildModifier"])).ToString();
            //labelReflex.Content = (int.Parse(user["reflex"]) + int.Parse(user["dexterityModifier"])).ToString();
            //labelWill.Content = (int.Parse(user["will"]) + int.Parse(user["prudenceModifier"])).ToString();

            //textNotes.Text = user["notes"];
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
                server.SendGlobalMessage(message);
                chatListBox.Items.Add("Ty: " + message);
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
    }
}
