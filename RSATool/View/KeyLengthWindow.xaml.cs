using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RSATool.View
{
    /// <summary>
    /// Interaction logic for KeyLengthWindow.xaml
    /// </summary>
    public partial class KeyLengthWindow : Window
    {
        public int BitLength = 128; //init bit length variable, this is default value
        public bool OK = false;     //boolean value, used when the window is close

       /*
       * constructor
       * */
        public KeyLengthWindow()
        {
            InitializeComponent();
            bitLength.Text = "128"; //set default value
        }

        /*
         * generate button event
         * */
        private void generateKeysBtn_Click(object sender, RoutedEventArgs e)
        {
            OK = true; //update the boolean value
            this.Close();
        }

        /*
        * cancel button event
        * */
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /* 
         *check if the length is changed immediately, is the user inserted not valid number than the 'generate' button is not available
         */
        private void bitLength_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                BitLength = Convert.ToInt32(bitLength.Text);

                if (BitLength < 1)
                {
                    throw new Exception("Negative number.");
                }

                generateKeysBtn.IsEnabled = true;
            }
            catch (Exception) //if exception thrown, the conversion is failed
            {
                generateKeysBtn.IsEnabled = false;
            }
        }

       
    }
}
