using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows;
using Interfaces;

namespace DnDClient
{
    public partial class CubeWindow : Window
    {
        private ClientMainWindow _mw;
        private User user;
        private static RNGCryptoServiceProvider rngCsp;

        public CubeWindow(ClientMainWindow window, User user) {
            InitializeComponent();
            _mw = window;
            this.user = user;
            modifierBox.Items.Add("Bez Modyfikatora");
            modifierBox.SelectedIndex = 0;
        }

        private void randomizeButton_Click(object sender, RoutedEventArgs e) {
            byte throwsNum;          // Ilość rzutów
            byte cubeTypeNum;        // Ilość ścianek kostki

            try {
                throwsNum = byte.Parse(throws.Text);
                cubeTypeNum = byte.Parse(cubeType.Text);

            } catch (Exception) { return; }

            var list = new List<Int32>();               // Lista wyrzuconych oczek
            rngCsp = new RNGCryptoServiceProvider();    // Service Kryptograficzny

            for (int i = 0; i < throwsNum; i++) {
                byte roll = RollDice((byte)cubeTypeNum);    // Funkcja losująca
                list.Add(roll);
            }

            rngCsp.Dispose();


            int sum = 0;
            foreach (var _ in list)         // Suma wszystkich oczek
                sum += _;

            var message = user.Name + " wyrzucił [" + throwsNum + "k" + cubeTypeNum + "] " + sum;

            if (throwsNum > 1) {
                message += " (";

                for (int i = 0; i < throwsNum; i++) // Wypisanie poszczególnych rzutów
                    message += list[i] + ((i == throwsNum - 1) ? "" : ",  ");

                message += ")";
            }

            message += ".";

            _mw.cubeThrow(message);
        }

        #region Rzut Kostką
        public static byte RollDice(byte numberSides) {
            if (numberSides <= 0)
                throw new ArgumentOutOfRangeException("numberSides");

            // Create a byte array to hold the random value. 
            byte[] randomNumber = new byte[1];
            do {
                // Fill the array with a random value.
                rngCsp.GetBytes(randomNumber);
            }
            while (!IsFairRoll(randomNumber[0], numberSides));
            // Return the random number mod the number 
            // of sides.  The possible values are zero- 
            // based, so we add one. 
            return (byte)((randomNumber[0] % numberSides) + 1);
        }

        private static bool IsFairRoll(byte roll, byte numSides) {
            // There are MaxValue / numSides full sets of numbers that can come up 
            // in a single byte.  For instance, if we have a 6 sided die, there are 
            // 42 full sets of 1-6 that come up.  The 43rd set is incomplete. 
            int fullSetsOfValues = Byte.MaxValue / numSides;

            // If the roll is within this range of fair values, then we let it continue. 
            // In the 6 sided die case, a roll between 0 and 251 is allowed.  (We use 
            // < rather than <= since the = portion allows through an extra 0 value). 
            // 252 through 255 would provide an extra 0, 1, 2, 3 so they are not fair 
            // to use. 
            return roll < numSides * fullSetsOfValues;
        }
        #endregion
    }
}
