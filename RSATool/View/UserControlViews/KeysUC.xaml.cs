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
    /// Interaction logic for KeysUC.xaml
    /// </summary>
    public partial class KeysUC : UserControl
    {
        public string PublicE
        {
            get { return (string)GetValue(PublicEProperty); }
            set { SetValue(PublicEProperty, value); }
        }

        public static readonly DependencyProperty PublicEProperty =
            DependencyProperty.Register(
            "PublicE",
            typeof(string),
            typeof(KeysUC),
            new UIPropertyMetadata(null));

        public string PublicN
        {
            get { return (string)GetValue(PublicNProperty); }
            set { SetValue(PublicNProperty, value); }
        }

        public static readonly DependencyProperty PublicNProperty =
            DependencyProperty.Register(
            "PublicN",
            typeof(string),
            typeof(KeysUC),
            new UIPropertyMetadata(null));

        public string PrivateD
        {
            get { return (string)GetValue(PrivateDProperty); }
            set { SetValue(PrivateDProperty, value); }
        }

        public static readonly DependencyProperty PrivateDProperty =
            DependencyProperty.Register(
            "PrivateD",
            typeof(string),
            typeof(KeysUC),
            new UIPropertyMetadata(null));

        public string PrivateN
        {
            get { return (string)GetValue(PrivateNProperty); }
            set { SetValue(PrivateNProperty, value); }
        }

        public static readonly DependencyProperty PrivateNProperty =
            DependencyProperty.Register(
            "PrivateN",
            typeof(string),
            typeof(KeysUC),
            new UIPropertyMetadata(null));


        /*
         * constructor
         * */
        public KeysUC()
        {
            InitializeComponent();
        }
    }
}
