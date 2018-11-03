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
    class DecryptionVM : BaseVM
    {
        private string cipherText;  //cipher txt as a string
        private string message;     //message, used usually for error messages

        public string CipherText
        {
            get
            {
                return this.cipherText;
            }
            set
            {
                this.cipherText = value;
                this.OnPropertyChanged("CipherText");
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
        public DecryptionVM()
        {
        }

        /*
         * this function checks if the cipher txt is empty. if so, we set error message
         * */
        public bool isValidDecrytion()
        {
            if (String.IsNullOrEmpty(CipherText))
            {
                Message = "No data to decrypt";
                return false;
            }
            return true;
        }
    }
}
