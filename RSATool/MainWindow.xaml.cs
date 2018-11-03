/*
 * This project created by Shlomo Sahar. 10/06/2015
 * Please send me your feedback to shlomo.sahar@gmail.com 
 * */
using RSAEngain;    //RSAEngain dll
using RSATool.View; //for using user control views
using RSATool.ViewModel;     //for using user view models
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace RSATool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BaseVM encryptionVM = new EncryptionVM();       //create encryption view model
        BaseVM decryptionVM = new DecryptionVM();       //create decryption view model
        BaseVM keysVM = new KeysVM();                   //create keys view model



        private Thread eThread;        //declare a thread for creating e
        private Thread pThread;        //declare a thread for creating p
        private Thread qThread;        //declare a thread for creating q

        /*
         * Constructor
         * */
        public MainWindow()
        {
            
            InitializeComponent();      //init component

            ucEncryption.DataContext = encryptionVM;    //set encryption tab data
            ucDecryption.DataContext = decryptionVM;    //set decryption tab data
            ucKeys.DataContext = keysVM;                //set keys tab data

        }

        /*
         * about button click event
         * */
        private void aboutBtn_Click(object sender, RoutedEventArgs e)
        {
            //show some data about the project
            MessageBox.Show(@"RSA Tool" +Environment.NewLine + "Version:" + 
                Assembly.GetEntryAssembly().GetName().Version.ToString()
                +"\nThis tool created by Shlomo Sahar\nFor any question or feedback please reach me at: shlomo.sahar@gmail.com","About");
        }

        /*
         * close button click event
         * */
        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void encryptBtn_Click(object sender, RoutedEventArgs e)
        {
           string msg;
           if ((msg = isValidEncryption()) != null) //check if the encryption is valid. That is, all the parameters are set properly
           {
               MessageBox.Show(msg); //show error msg
               return;
           }

           try
           {
               ThreadPool.QueueUserWorkItem((o) =>
               {
                   Dispatcher.Invoke((Action)(() => StartLongRunningProcess()));
                   //decrypt the data
                   encryptData();
               });
           }
           catch (Exception)
           {
               MessageBox.Show("Enter valid public key components to encrypt.", "Error");
           }
        }


        /*
         * this function checks if the encryption is valid. if it valid than the return value must be null otherwise error message 
         * */
        private string isValidEncryption()
        {
            if (!(encryptionVM as EncryptionVM).isValidEncryption())
            {
                return (encryptionVM as EncryptionVM).Message;
            }
            if (!(keysVM as KeysVM).isValidKeys())
            {
                return (keysVM as KeysVM).Message;
            }
            return null;
        }

        /*
         * decrypt button event
         **/ 
        private void decryptBtn_Click(object sender, RoutedEventArgs e)
        {
            string msg;

            if ((msg = isValidDecryption()) != null) //check if is valid decryption. if not show error msg and return
            {
                MessageBox.Show(msg);
                return;
            }

            if ((decryptionVM as DecryptionVM).CipherText.Length == 0)
            {
                return;
            }

            try
            {
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    Dispatcher.Invoke((Action)(() => StartLongRunningProcess()));
                    //decrypt the data
                    decryptData();

                    //Dispatcher.Invoke((Action)(() => info.Text = "step 2"));
                    //Dispatcher.Invoke((Action)(() => info.Text = "step 3"));
                });
                
            }
            catch (Exception)
            {
                MessageBox.Show("Enter valid private key components to decrypt.", "Error");
            }
        }

        /*
         * this function checks if the decryption is valid. if it valid the return value must be null otherwise error message 
         * */
        private string isValidDecryption()
        {
            if (!(decryptionVM as DecryptionVM).isValidDecrytion())
            {
                return (decryptionVM as DecryptionVM).Message;
            }
            if (!(keysVM as KeysVM).isValidKeys())
            {
                return (keysVM as KeysVM).Message;
            }
            return null;
        }

        /*
        * generate keys function
        * */
        private void generateKeysBtn_Click(object sender, RoutedEventArgs e)
        {
         
           var form = new KeyLengthWindow();
           form.ShowDialog();
            
            if (form.OK)
            {
                 ThreadPool.QueueUserWorkItem((o) =>
                 {
                     Dispatcher.Invoke((Action)(() => StartLongRunningProcess()));
                     // Generate keypair
                     getKeys(form);
                 });
            }
        }

        /*
         * save the cipher text into file
         * */
        private void saveCipherTxtBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty((decryptionVM as DecryptionVM).CipherText) && (decryptionVM as DecryptionVM).CipherText.Length > 0)
            {
                // Configure save file dialog box
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "data"; // Default file name
                dlg.DefaultExt = ".cipher"; // Default file extension
                dlg.Filter = "Cipher documents (.cipher)|*.cipher"; // Filter files by extension 

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results 
                if (result == true)
                {
                    // Save document 
                    string fileName = dlg.FileName;

                    TextWriter tw = new StreamWriter(fileName);

                    tw.Write((decryptionVM as DecryptionVM).CipherText);

                    tw.Close();
                }

                MessageBox.Show("Successfully saved current cipher.", "Success");
            }
            else
            {
                MessageBox.Show("No cipher data to save.", "Information");
            }
         
        }


        /*
         * load encrypted file function
         * */
        private void openFileBtn_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the open file dialog box.
            Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "Text Files (.cipher)|*.cipher|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = true;

            // Call the ShowDialog method to show the dialog box.
            bool? userClickedOK = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == true)
            {
                try
                {
                    // Open the selected file to read.
                    System.IO.Stream fileStream = openFileDialog1.OpenFile();//openFileDialog1.File.OpenRead();

                    using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                    {
                        // Read the first line from the file and write it the textbox.
                        (decryptionVM as DecryptionVM).CipherText = reader.ReadToEnd();
                    }
                    fileStream.Close();
                    tabControl.SelectedIndex = 1;
                    MessageBox.Show("Successfully opened encrypted file.", "Success");
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Can't open input file", "Error");
                }
                
            }
        }

        //Some method that is going to start something that is going to take a while
        void StartLongRunningProcess()
        {
            this.progressBar.IsIndeterminate = true; //messageStatusBar.Text = "Generating p and q prime numbers.";
        }

        //The method (delegate) that handles the result, usually from an event.
        //This method will handle the result of the asynchronous call 
        public void HandlerForLongRunningProcess()
        {
            //Do stuff with result from your asynchronous web service call etc...

            //Stop the animation for your progress bar
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate()
            {
                progressBar.IsIndeterminate = false;

                //WebMethods
            });
        }


        /*
         * Get keys function, the function get as a parameter key length window. it done because we need to read the length from this form 
         * */
        public void getKeys(KeyLengthWindow form)
        {
            // Generate two random numbers
            // Lets use our entropy based random generator
            Generator.Initialize(2);

            // For 1024 bit key length, we take 512 bits for first prime and 512 for second prime
            // Get same min and max
            BigInteger numMin = BigInteger.Pow(2, (form.BitLength / 2) - 1);
            BigInteger numMax = BigInteger.Pow(2, (form.BitLength / 2));

            // Create two prime numbers
            var p = new PrimeNumber();
            var q = new PrimeNumber();

            p.SetNumber(Generator.Random(numMin, numMin));
            q.SetNumber(Generator.Random(numMin, numMax));

            // Create threaded p and q searches
            pThread = new Thread(p.RabinMiller);
            qThread = new Thread(q.RabinMiller);

            // For timeout
            DateTime start = DateTime.Now;

            pThread.Start();
            qThread.Start();

            while (pThread.IsAlive || qThread.IsAlive)
            {
                TimeSpan ts = DateTime.Now - start;

                if (ts.TotalMilliseconds > (1000 * 60 * 5))
                {
                    try
                    {
                        pThread.Abort();
                    }
                    // ReSharper disable EmptyGeneralCatchClause
                    catch (Exception)
                    // ReSharper restore EmptyGeneralCatchClause
                    {
                    }

                    try
                    {
                        qThread.Abort();
                    }
                    // ReSharper disable EmptyGeneralCatchClause
                    catch (Exception)
                    // ReSharper restore EmptyGeneralCatchClause
                    {
                    }

                    MessageBox.Show("Key generating error: timeout.\r\n\r\nIs your bit length too large?", "Error");

                    break;
                }
            }


            // If we found numbers, we continue to create
            if (p.GetFoundPrime() && q.GetFoundPrime())
            {
                BigInteger n = p.GetPrimeNumber() * q.GetPrimeNumber();
                BigInteger euler = (p.GetPrimeNumber() - 1) * (q.GetPrimeNumber() - 1);

                this.Dispatcher.Invoke((Action)(() =>
                {
                    messageStatusBar.Text = "Generating e prime number ...";
                }));

                var primeNumber = new PrimeNumber();

                while (true)
                {
                    primeNumber.SetNumber(Generator.Random(2, euler - 1));

                    start = DateTime.Now;

                    eThread = new Thread(primeNumber.RabinMiller);
                    eThread.Start();

                    while (eThread.IsAlive)
                    {

                        TimeSpan ts = DateTime.Now - start;

                        if (ts.TotalMilliseconds > (1000 * 60 * 5))
                        {
                            MessageBox.Show("Key generating error: timeout.\r\n\r\nIs your bit length too large?", "Error");

                            break;
                        }
                    }

                    if (primeNumber.GetFoundPrime() && (BigInteger.GreatestCommonDivisor(primeNumber.GetPrimeNumber(), euler) == 1))
                    {
                        break;
                    }
                }

                if (primeNumber.GetFoundPrime())
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        messageStatusBar.Text = "Calculating key components ...";
                    }));
                 
                    // Calculate secret exponent D as inverse number of E
                    BigInteger d = MathExtended.ModularLinearEquationSolver(primeNumber.GetPrimeNumber(), 1, euler);
                    //BigInteger d = MathExtended.ModularLinearEquationSolver(83, 1, 120);

                    // Displaying keys
                    if (d > 0)
                    {
                        // N
                        (keysVM as KeysVM).PublicN = n.ToString();
                        (keysVM as KeysVM).PrivateN = n.ToString();

                        // E
                        (keysVM as KeysVM).PublicE = primeNumber.GetPrimeNumber().ToString();

                        // D
                        (keysVM as KeysVM).PrivateD = d.ToString();

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            tabControl.SelectedIndex = 2;

                            messageStatusBar.Text = "Successfully created key pair.";
                        }));
                     
                    }
                    else
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            messageStatusBar.Text = "Error: Modular equation solver fault.";
                        }));

                        MessageBox.Show(
                            "Error using mathematical extensions.\r\ne = " + primeNumber + "\r\neuler = " + euler + "\r\np = " +
                            p.GetPrimeNumber() + "\r\n" + q.GetPrimeNumber(), "Error");
                    }
                }
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    messageStatusBar.Text = "Idle";
                }));
            }
            this.Dispatcher.Invoke((Action)(() =>
            {
                this.progressBar.IsIndeterminate = false;

            }));
        }

        /*
         * Data decryption function
         * */       
        private void  decryptData()
        {
            BigInteger D = BigInteger.Parse((keysVM as KeysVM).PrivateD);   //get private d from keys view model 
            BigInteger N = BigInteger.Parse((keysVM as KeysVM).PrivateN);   //get private e from keys view model 

            (encryptionVM as EncryptionVM).PlainText = "";      // 'reset' the plain text

            string text = ""; // to hold the decrypted data

            try
            {
                string[] cipher = (decryptionVM as DecryptionVM).CipherText.Split(' '); // split the cipher text by space. each letter separate by space in ciphertext

                BigInteger m = BigInteger.ModPow(BigInteger.Parse(cipher[0]), D, N);    //we get the size of the cipher text

                if (cipher.Length == 1)
                {
                    (encryptionVM as EncryptionVM).PlainText = m.ToString();
                }
                else
                {
                    BigInteger length = m;

                    for (int i = 1; i < cipher.Length; i++)
                    {
                        m = BigInteger.ModPow(BigInteger.Parse(cipher[i]), D, N);   //for each encrypted letter x we calculate x^D mod N. since the data is encrypted as numbers we can do it

                        //M = (M % (BigInteger.Pow(2, 31)));

                        char chr = Convert.ToChar(Convert.ToInt32(m.ToString())); //convert the ascci number to char

                        text += chr;    //add the char to the plain text
                    }

                    if (length == text.Length)      //check that the length is match
                    {
                        (encryptionVM as EncryptionVM).PlainText = text;
                        ThreadPool.QueueUserWorkItem((o) =>
                        {
                            Dispatcher.Invoke((Action)(() =>  messageStatusBar.Text = "Decryption finished successfully"));
                           
                        });
                       
                    }
                    else
                    {
                        MessageBox.Show("Error decrypting: incomplete message.", "Error");
                    }
                }

                ThreadPool.QueueUserWorkItem((o) =>
                {
                    Dispatcher.Invoke((Action)(() =>  tabControl.SelectedIndex = 0));

                });

                ThreadPool.QueueUserWorkItem((o) =>
                {
                    Dispatcher.Invoke((Action)(() => progressBar.IsIndeterminate = false));
                });
               
            }
            catch (Exception)
            {
                MessageBox.Show("Error decrypting, invalid private key.", "Error");
            }
        }

        /*
         * Data encryption function
         * */
        private void encryptData()
        {
            BigInteger E = BigInteger.Parse((keysVM as KeysVM).PublicE);     //get e from keys view
            BigInteger N = BigInteger.Parse((keysVM as KeysVM).PrivateN);    //get n from keys view

            if ((encryptionVM as EncryptionVM).PlainText.Length > N)     //if the data is bigger than the key length
            {
                MessageBox.Show("Cannot encrypt data longer than: " + N + "\r\n\r\nUse key pair of bigger length.",
                                "Error");
                return;
            }

            if ((encryptionVM as EncryptionVM).PlainText.Length == 0)        //if there is no text to encrypt
            {
                return;
            }

            //Encrypt the length of the string. we need this value to ensure the cipher text is valid and not changed when we'll want to decrypt it. it done by calculating length^E mod N
            (decryptionVM as DecryptionVM).CipherText = BigInteger.ModPow((encryptionVM as EncryptionVM).PlainText.Length, E, N).ToString();

            //for each letter in the string we calculate the ascii value of the letter power E modulo N
            for (int i = 0; i < (encryptionVM as EncryptionVM).PlainText.Length; i++)
            {
                char chr = (encryptionVM as EncryptionVM).PlainText[i];

                BigInteger c = BigInteger.ModPow(Convert.ToInt32(chr), E, N);

                if ((decryptionVM as DecryptionVM).CipherText == "")
                {
                    (decryptionVM as DecryptionVM).CipherText = c.ToString();
                }
                else
                {
                    (decryptionVM as DecryptionVM).CipherText += " " + c.ToString(); //add the encrypt letter to the cipher text
                }
            }

            ThreadPool.QueueUserWorkItem((o) =>
            {
                Dispatcher.Invoke((Action)(() =>   messageStatusBar.Text = "Encryption finished successfully"));
            });

            ThreadPool.QueueUserWorkItem((o) =>
            {
                Dispatcher.Invoke((Action)(() => tabControl.SelectedIndex = 1));    //move to cipher tab 
            });
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Dispatcher.Invoke((Action)(() =>   progressBar.IsIndeterminate = false));
            });

          
           
        }

        /*
         * save keys function
         * */
        private void saveKeysBtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty((keysVM as KeysVM).PrivateD) || String.IsNullOrEmpty((keysVM as KeysVM).PrivateN)  ||
                String.IsNullOrEmpty((keysVM as KeysVM).PublicE) || String.IsNullOrEmpty((keysVM as KeysVM).PublicN))
            {
                MessageBox.Show("Enter your public and private keys to continue.", "Error");
                return;
            }

            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "KeysFile"; // Default file name
            dlg.DefaultExt = ".keys"; // Default file extension
            dlg.Filter = "RSA key pair (*.keys)|*.keys|Text file (*.txt)|*.txt"; // Filter files by extension 
            dlg.Title = "Save key pair to file";


            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results 
            if (result == true)
            {
                try
                {
                     TextWriter tw = new StreamWriter(dlg.FileName);

                     if (System.IO.Path.GetExtension(dlg.FileName) == ".txt")
                     {
                         tw.WriteLine((keysVM as KeysVM).PublicE + " " + (keysVM as KeysVM).PublicN);
                         tw.WriteLine((keysVM as KeysVM).PrivateD + " " + (keysVM as KeysVM).PrivateN);
                     }
                     else
                     {
                         tw.WriteLine("e = " + (keysVM as KeysVM).PublicE + ";");
                         tw.WriteLine("d = " + (keysVM as KeysVM).PrivateD + ";");
                         tw.WriteLine("n = " + (keysVM as KeysVM).PublicN + ";");
                      }

                      tw.Close();

                      MessageBox.Show("Key pair successfully saved to: " + dlg.FileName, "Success");
                }
                catch (Exception)
                {
                    MessageBox.Show("Error saving key pair to file. ", "Error");
                }

           
            }
        }

        /*
        * open saved keys function
        * */
        private void openKeysFileBtn_Click(object sender, RoutedEventArgs e)
        {

            // Create an instance of the open file dialog box.
            Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "RSA key pair (*.keys)|*.keys|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Title = "Open stored RSA key pair";

            openFileDialog1.Multiselect = false;

            // Call the ShowDialog method to show the dialog box.
            bool? userClickedOK = openFileDialog1.ShowDialog();

            if (userClickedOK == true)
            {
                bool opened = true;

                // Open the selected file to read.
                TextReader tr = new StreamReader(openFileDialog1.FileName);

                if (System.IO.Path.GetExtension(openFileDialog1.FileName) == ".txt")
                {
                    try
                    {
                        string readLine = tr.ReadLine();
                        if (readLine != null)
                        {
                            string[] publicKey = readLine.Split(' ');
                            string[] privateKey = readLine.Split(' ');

                            (keysVM as KeysVM).PublicE = publicKey[0];
                            (keysVM as KeysVM).PublicN = publicKey[1];

                            (keysVM as KeysVM).PrivateD = privateKey[0];
                            (keysVM as KeysVM).PrivateN = privateKey[1];
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Cannot open RSA keypair: File does not contain enough data.", "Error");
                    }
                }
                else
                {
                    string line;

                    while ((line = tr.ReadLine()) != null)
                    {
                        try
                        {
                            string[] parse = line.Split('=');

                            string cmd = parse[0].Trim();

                            string num = parse[1].Trim();

                            if (num.EndsWith(";"))
                            {
                                num = num.Substring(0, num.Length - 1);
                            }
                            else
                            {
                                throw new Exception("Parse error: Missing semicolon.");
                            }

                            BigInteger test = BigInteger.Parse(num);

                            if (cmd == "e")
                            {
                                (keysVM as KeysVM).PublicE = test.ToString();
                            }
                            else if (cmd == "d")
                            {
                                (keysVM as KeysVM).PrivateD = test.ToString();
                            }
                            else if (cmd == "n")
                            {
                                (keysVM as KeysVM).PublicN = (keysVM as KeysVM).PrivateN = test.ToString();
                            }
                            else
                            {
                                MessageBox.Show("Cannot open RSA keypair: File is invalid.", "Error");

                                opened = false;

                                break;
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Cannot open RSA keypair: File is corrupted.", "Error");

                            opened = false;

                            break;
                        }
                    }
                }
                    tabControl.SelectedIndex = 2;

                    if (opened)
                    {
                        MessageBox.Show("Successfully opened RSA keypair.", "Success");
                    }
                    else
                    {
                        (keysVM as KeysVM).PublicE = (keysVM as KeysVM).PublicN = (keysVM as KeysVM).PrivateD = (keysVM as KeysVM).PrivateN = "";
                    }

                tr.Close();
            }

        }
       

    }
}
