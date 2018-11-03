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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RSATool.View.UserControlViews
{
    /// <summary>
    /// Interaction logic for DecryptionUC.xaml
    /// </summary>
    public partial class DecryptionUC : UserControl
    {
        public string CipherText
        {
            get { return (string)GetValue(CipherTextProperty); }
            set { SetValue(CipherTextProperty, value); }
        }

        public static readonly DependencyProperty CipherTextProperty =
            DependencyProperty.Register(
            "CipherText",
            typeof(string),
            typeof(DecryptionUC),
            new UIPropertyMetadata(null));


        /*
         * constructor
         * */
        public DecryptionUC()
        {
            InitializeComponent();
        }
    }
}
