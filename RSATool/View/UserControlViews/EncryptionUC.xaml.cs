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
    /// Interaction logic for EncryptionUC.xaml
    /// </summary>
    public partial class EncryptionUC : UserControl
    {
        public string PlainText
        {
            get { return (string)GetValue(PlainTextProperty); }
            set { SetValue(PlainTextProperty, value); }
        }

        public static readonly DependencyProperty PlainTextProperty =
            DependencyProperty.Register(
            "PlainText",
            typeof(string),
            typeof(EncryptionUC),
            new UIPropertyMetadata(null));


        /*
         * constructor
         * */
        public EncryptionUC()
        {
            InitializeComponent();
        }
    }
}
