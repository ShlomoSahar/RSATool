/*
 Written by Shlomo Sahar, 2015
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
namespace RSATool.ViewModel
{
    /*
     * this class contains the keys that required for RSA algorithm
     * */
    class KeysVM : BaseVM
    {
        private string publicE; 
        private string publicN;
        private string privateD;
        private string privateN;
        private string message;


        public string PublicE
        {
            get
            {
                return this.publicE;
            }
            set
            {
                this.publicE = value;
                this.OnPropertyChanged("PublicE");
            }
        }

        public string PublicN
        {
            get
            {
                return this.publicN;
            }
            set
            {
                this.publicN = value;
                this.OnPropertyChanged("PublicN");
            }
        }

        public string PrivateD
        {
            get
            {
                return this.privateD;
            }
            set
            {
                this.privateD = value;
                this.OnPropertyChanged("PrivateD");
            }
        }

        public string PrivateN
        {
            get
            {
                return this.privateN;
            }
            set
            {
                this.privateN = value;
                this.OnPropertyChanged("PrivateN");
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
        public KeysVM()
        {
        }

        /*
        * this function checks if the keys are valid. if not set error message
        * */
        public bool isValidKeys()
        {
            BigInteger bi = new BigInteger();

            // convert key values to BigInteger
            if (!BigInteger.TryParse(PublicE, out bi)) 
            {
                Message = "Public key e is not valid";
                return false;
            }
            if (!BigInteger.TryParse(PublicN, out bi))
            {
                Message = "Public key n is not valid";
                return false;
            }
            if (!BigInteger.TryParse(PrivateD, out bi))
            {
                Message = "Private key d is not valid";
                return false;
            }
            if (!BigInteger.TryParse(PrivateN, out bi))
            {
                Message = "Private key n is not valid";
                return false;
            }

            return true;
        }


    }
}
