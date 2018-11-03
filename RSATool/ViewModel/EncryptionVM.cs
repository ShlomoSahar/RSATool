/*
 Written by Shlomo Sahar, 2015
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSATool.ViewModel
{
    class EncryptionVM : BaseVM
    {
        private string plainText;   //plain txt as a string
        private string message;     //message, used usually for error messages

        public string PlainText
        {
            get
            {
                return this.plainText;
            }
            set
            {
                this.plainText = value;
                this.OnPropertyChanged("PlainText");
            }
        }


        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = value;
                this.OnPropertyChanged("Message");
            }
        }

        /*
         * constructor
         * */
        public EncryptionVM()
        {
        }

        /*
         * this function checks if the plain txt is empty. if so, we set suite error message
         * */
        public bool isValidEncryption()
        {
            if(String.IsNullOrEmpty(PlainText))
            {
                Message = "No data to encrypt";
                return false;
            }
            return true;
        }
    }
}
