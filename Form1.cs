using _17743_Mihajlo_Marjanovic_ZI_Projekat;
using System.Numerics;
using System.Text;
using System.Windows.Forms;

namespace _17743_Mihajlo_Marjanovic_ZI_WinForms_Projekat
{
    public partial class Form1 : Form
    {

        private int[] a5_1_demo_key = { 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 1,
                1, 1, 1, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0,
                1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0,
                0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0 };
        private string playfair_demo_key = "inicijativa";
        private int rsa_demo_keySize = 48;
        private RSA? rsa = null;

        private int[] cfb_demo_key = { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1,
                0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0,
                1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 };
        private int[] cfb_iv_demo = { 0, 1, 1, 0, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0 };
        public Form1()
        {
            InitializeComponent();
        }

        static void compareTwoFilesWithSHA256(string path1, string path2, TextBox tb)
        {
            tb.Text += "Poredimo fajlove: " + path1 + " i " + path2 + " pomocu SHA-256 hash funkcije:" + "\r\n";

            string hash1 = StringHashOfTheFile(path1);
            string hash2 = StringHashOfTheFile(path2);

            tb.Text += "Hash 1. fajla: " + hash1 + "\r\n";
            tb.Text += "Hash 2. fajla: " + hash2 + "\r\n";

            if (hash1 == hash2)
            {
                tb.Text += "Fajlovi su isti" + "\r\n";
            }
            else
            {
                tb.Text += "Fajlovi su razliciti" + "\r\n";
            }
        }
        static string StringHashOfTheFile(string fileName)
        {
            // read all bytes from file
            byte[] fileBytes = File.ReadAllBytes(fileName);

            // compute hash
            byte[] hash = SHA256.ComputeHash(fileBytes);

            //return Convert.ToBase64String(hash);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        // --------------- A5/1

        private A5_1 createA5_1ObjectFromKeyInput()
        {
            //  convert a5_1_keyInput to int[] and throw error if empty or if not cinsisted of  0 and 1
            int[] a5_1_key = a5_1_keyInput.Text.Select(c => int.Parse(c.ToString())).ToArray();
            if (a5_1_key.Length == 0)
            {
                MessageBox.Show("Key is empty!");
                return null;
            }
            foreach (int i in a5_1_key)
            {
                if (i != 0 && i != 1)
                {
                    MessageBox.Show("Key is not consisted of 0 and 1!");
                    return null;
                }
            }

            //  create a5_1 object
            A5_1 a5_1 = new A5_1(a5_1_key);

            return a5_1;
        }

        private void a5_1_fillKey_Click(object sender, EventArgs e)
        {
            a5_1_keyInput.Text = string.Join("", a5_1_demo_key);
        }

        private void a5_1_demo_Click(object sender, EventArgs e)
        {
            int[] a5_1_text = { 1, 0, 0, 1, 0 };

            A5_1 a5_1 = new A5_1(a5_1_demo_key);
            int[] a5_1_cipher = a5_1.Crypt(a5_1_text);
            int[] a5_1_decrypted_value = a5_1.Decrypt(a5_1_cipher);

            // populate the textboxes
            a5_1_keyInput.Text = string.Join("", a5_1_demo_key);
            a5_1_input.Text = string.Join("", a5_1_text);
            a5_1_output.Text = string.Join("", a5_1_cipher);
            a5_1_decrypted.Text = string.Join("", a5_1_decrypted_value);

            a5_1.ReloadKey();
            a5_1_generateTestBinaryFile_Click(sender, e);
            a5_1_fileEncrypt_Click(sender, e);
            a5_1_bmpEncryptBtn_Click(sender, e);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void a5_1_encryptButton_Click(object sender, EventArgs e)
        {
            //  convert a5_1_keyInput to int[]
            int[] a5_1_key = a5_1_keyInput.Text.Select(c => int.Parse(c.ToString())).ToArray();
            //  convert a5_1_input to int[]
            int[] a5_1_text = a5_1_input.Text.Select(c => int.Parse(c.ToString())).ToArray();

            A5_1 a5_1 = new A5_1(a5_1_key);
            int[] a5_1_cipher = a5_1.Crypt(a5_1_text);
            int[] a5_1_decrypted_value = a5_1.Decrypt(a5_1_cipher);

            // populate the textboxes
            a5_1_output.Text = string.Join("", a5_1_cipher);
            a5_1_decrypted.Text = string.Join("", a5_1_decrypted_value);

            a5_1.ReloadKey();
        }

        private void a5_1_decryptButton_Click(object sender, EventArgs e)
        {
            // convert a5_1_keyInput to int[]
            int[] a5_1_key = a5_1_keyInput.Text.Select(c => int.Parse(c.ToString())).ToArray();
            // convert a5_1_output to int[]
            int[] a5_1_cipher = a5_1_output.Text.Select(c => int.Parse(c.ToString())).ToArray();

            A5_1 a5_1 = new A5_1(a5_1_key);
            int[] a5_1_decrypted_value = a5_1.Decrypt(a5_1_cipher);

            // populate the textboxes
            a5_1_decrypted.Text = string.Join("", a5_1_decrypted_value);

            a5_1.ReloadKey();
        }

        private void a5_1_generateTestBinaryFile_Click(object sender, EventArgs e)
        {
            //  create a5_1 object
            A5_1 a5_1 = createA5_1ObjectFromKeyInput();
            if (a5_1 == null) return;

            a5_1.GenerateTestBinaryFile();

            // populate a5_1_inputFileName
            a5_1_inputFileName.Text = "a5_1_input.bin";

            // show success message // TODO
            //if (showMessage)
            //{
            //    MessageBox.Show("Fajl 'a5_1_input.bin' kreiran!");
            //}
        }

        private void a5_1_fileEncrypt_Click(object sender, EventArgs e)
        {
            //  create a5_1 object
            A5_1 a5_1 = createA5_1ObjectFromKeyInput();
            if (a5_1 == null) return;

            // Encrypt a file
            string a5_1_inputFilePath = a5_1_inputFileName.Text;
            if (a5_1_outputFileName.Text.Length == 0)
            {
                a5_1_outputFileName.Text = "a5_1_encrypted.bin";
            }
            string a5_1_outputFilePath = a5_1_outputFileName.Text;
            a5_1.EncryptFile(a5_1_inputFilePath, a5_1_outputFilePath);

            // Decrypt the encrypted file
            if (a5_1_decryptFileName.Text.Length == 0)
            {
                a5_1_decryptFileName.Text = "a5_1_decrypted.bin";
            }
            a5_1_inputFilePath = a5_1_outputFileName.Text;
            a5_1_outputFilePath = a5_1_decryptFileName.Text;
            a5_1.DecryptFile(a5_1_inputFilePath, a5_1_outputFilePath);

            TextBox hashTb = a5_1_hash;
            hashTb.Text = "";
            compareTwoFilesWithSHA256(a5_1_inputFileName.Text, a5_1_outputFileName.Text, hashTb);
            compareTwoFilesWithSHA256(a5_1_inputFileName.Text, a5_1_decryptFileName.Text, hashTb);
        }

        private void a5_1_bmpEncryptBtn_Click(object sender, EventArgs e)
        {
            a5_1_bmpEncryptBtn.Enabled = false;
            bmpOutputImage.Image = null;
            bmpDecryptedImage.Image = null;

            //  create a5_1 object
            A5_1 a5_1 = createA5_1ObjectFromKeyInput();
            if (a5_1 == null) return;

            if (a5_1_bmpImageInputTextBox.Text.Length == 0)
            {
                a5_1_bmpImageInputTextBox.Text = "input.bmp";
            }
            string a5_1_inputFilePath = a5_1_bmpImageInputTextBox.Text;

            ProgressBar progressBar = BMP_ProgressBar;
            progressBar.Value = 0;

            bmpInputImage.Image = Image.FromFile(a5_1_inputFilePath);
            a5_1.CreateBMPImage(a5_1_inputFilePath, progressBar);

            // populate output and decrypt images
            bmpOutputImage.Image = Image.FromFile("A5_1_output.bmp");
            //bmpOutputImage.ImageLocation = "A5_1_output.bmp";
            bmpDecryptedImage.Image = Image.FromFile("A5_1_decrypted_image.bmp");
            //bmpDecryptedImage.ImageLocation = "A5_1_decrypted_image.bmp";

            a5_1.ReloadKey();
            progressBar.Value = 100;
            a5_1_bmpEncryptBtn.Enabled = true;
        }

        // --------------- PLAYFAIR

        private Playfair createPlayfairObjectFromKeyInput()
        {
            //  convert playfair_keyInput to string and throw error if empty or if anything except letters
            string playfair_key = playfair_keyInput.Text;
            if (playfair_key.Length == 0)
            {
                MessageBox.Show("Unesite kljuc!");
                return null;
            }
            if (!playfair_key.All(char.IsLetter))
            {
                MessageBox.Show("Kljuc mora sadrzati samo slova!");
                return null;
            }

            //  create playfair object
            Playfair playfair = new Playfair(playfair_key);
            return playfair;
        }

        private void playfair_demoKey_Click(object sender, EventArgs e)
        {
            // populate the textboxes
            playfair_keyInput.Text = playfair_demo_key;
        }

        private void playfair_demo_Click(object sender, EventArgs e)
        {
            // populate the textboxes
            playfair_keyInput.Text = playfair_demo_key;
            playfair_input.Text = "MicrosoftAzureOfficeBingWindows";

            playfair_encrypt_Click(sender, e);
            playfair_generateDemoFile_Click(sender, e);
            playfair_fileEncrypt_Click(sender, e);



        }

        private void playfair_encrypt_Click(object sender, EventArgs e)
        {
            // create playfair object
            Playfair playfair = createPlayfairObjectFromKeyInput();
            if (playfair == null) return;

            // encrypt playfair_input
            string playfair_inputText = playfair_input.Text;
            string playfair_encryptedText = playfair.Encrypt(playfair_inputText);
            string playfair_decryptedText = playfair.Decrypt(playfair_encryptedText);

            // populate the textboxes
            playfair_output.Text = playfair_encryptedText;
            playfair_decrypted.Text = playfair_decryptedText;
        }

        private void playfair_decrypt_Click(object sender, EventArgs e)
        {
            // create playfair object
            Playfair playfair = createPlayfairObjectFromKeyInput();
            if (playfair == null) return;

            // decrypt playfair_output
            string playfair_inputText = playfair_output.Text;
            string playfair_decryptedText = playfair.Decrypt(playfair_inputText);

            // populate the textboxes
            playfair_decrypted.Text = playfair_decryptedText;
        }

        private void playfair_generateDemoFile_Click(object sender, EventArgs e)
        {
            // create playfair object
            Playfair playfair = createPlayfairObjectFromKeyInput();
            if (playfair == null) return;

            // generate a file
            playfair.GenerateDemoInputFile();

            // populate playfair_inputFileName
            playfair_fileInput.Text = "playfair_demoInput.txt";
        }

        private void playfair_fileEncrypt_Click(object sender, EventArgs e)
        {
            // create playfair object
            Playfair playfair = createPlayfairObjectFromKeyInput();
            if (playfair == null) return;

            // encrypt a file
            string playfair_inputFilePath = playfair_fileInput.Text;

            if (playfair_fileOutput.Text.Length == 0)
            {
                playfair_fileOutput.Text = "playfair_encrypted.txt";
            }
            string playfair_outputFilePath = playfair_fileOutput.Text;


            // check if parallel
            if (playfair_multithreadCheckbox.Checked)
            {
                int numOfThreads = (int)playfair_numOfThreads.Value;
                playfair.EncryptFileParallel(playfair_inputFilePath, playfair_outputFilePath, numOfThreads);
            }
            else
            {
                playfair.EncryptFile(playfair_inputFilePath, playfair_outputFilePath);
            }

            // decrypt the encrypted file
            if (playfair_fileDecrypted.Text.Length == 0)
            {
                playfair_fileDecrypted.Text = "playfair_decrypted.txt";
            }
            playfair_inputFilePath = playfair_fileOutput.Text;
            playfair_outputFilePath = playfair_fileDecrypted.Text;
            playfair.DecryptFile(playfair_inputFilePath, playfair_outputFilePath);

            // compare the original file with the decrypted file
            TextBox hashTb = playfair_hash;
            hashTb.Text = "";
            compareTwoFilesWithSHA256(playfair_fileInput.Text, playfair_fileOutput.Text, hashTb);
            compareTwoFilesWithSHA256(playfair_fileInput.Text, playfair_fileDecrypted.Text, hashTb);
        }

        private void playfair_multithreadCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (playfair_multithreadCheckbox.Checked)
            {
                playfair_numOfThreads.Enabled = true;
            }
            else
            {
                playfair_numOfThreads.Enabled = false;
            }
        }

        // --------------- RSA

        private RSA getRSAInstance(bool force = false)
        {
            if (rsa != null && !force) return rsa;

            rsa_state.Text = "Generisanje kljuceva... Ovo moze potrajati. Sto je veci keySize duze ce trajati. Za brzo generisanje preporuceno je 48 bita.";

            int keySize = (int)rsa_keySize.Value;
            if (keySize < 24 || keySize > 2048)
            {
                MessageBox.Show("Key size must be between 24 and 2048");
                keySize = rsa_demo_keySize;
                rsa_keySize.Value = rsa_demo_keySize;
            }


            rsa = new RSA(keySize);
            rsa_state.Text = "Instanca RSA kreirana.";
            return rsa;
        }

        private void rsa_newInstance_Click(object sender, EventArgs e)
        {
            rsa = null;
            rsa = getRSAInstance(true);
        }

        private void rsa_demo_Click(object sender, EventArgs e)
        {
            getRSAInstance();
            rsa_encrypt_Click(sender, e);
            rsa_generateDemoFile_Click(sender, e);
            rsa_encryptFile_Click(sender, e);
        }

        private void rsa_encrypt_Click(object sender, EventArgs e)
        {
            getRSAInstance();

            string input = rsa_input.Text;
            // if input is empty use demo input
            if (input.Length == 0)
            {
                input = "123456";
                rsa_input.Text = input;
            }
            // check if input contains only numbers
            if (!input.All(char.IsDigit))
            {
                MessageBox.Show("Input must contain only numbers!");
                return;
            }
            // convert input string to BigInteger
            BigInteger inputBigInt = BigInteger.Parse(input);
            BigInteger encrypted = rsa.Encrypt(inputBigInt);
            rsa_output.Text = encrypted.ToString();
            BigInteger decryptedMessage = rsa.Decrypt(encrypted);
            rsa_decrypted.Text = decryptedMessage.ToString();
        }

        private void rsa_decrypt_Click(object sender, EventArgs e)
        {
            getRSAInstance();

            string input = rsa_output.Text;
            // if input is empty use demo input
            if (input.Length == 0)
            {
                input = "123456";
                rsa_output.Text = input;
            }
            // check if input contains only numbers
            if (!input.All(char.IsDigit))
            {
                MessageBox.Show("Input must contain only numbers!");
                return;
            }
            // convert input string to BigInteger
            BigInteger inputBigInt = BigInteger.Parse(input);
            BigInteger decryptedMessage = rsa.Decrypt(inputBigInt);
            rsa_decrypted.Text = decryptedMessage.ToString();
        }

        private void rsa_generateDemoFile_Click(object sender, EventArgs e)
        {
            // create playfair object
            getRSAInstance();

            // generate a file
            rsa.GenerateDemoInputFile();

            // populate playfair_inputFileName
            rsa_fileInput.Text = "rsa_demoInput.txt";
        }

        private void rsa_encryptFile_Click(object sender, EventArgs e)
        {
            // rsa
            getRSAInstance();

            // encrypt a file
            string rsa_inputFilePath = rsa_fileInput.Text;

            if (rsa_fileOutput.Text.Length == 0)
            {
                rsa_fileOutput.Text = "rsa_encrypted.txt";
            }

            string rsa_outputFilePath = rsa_fileOutput.Text;
            rsa.EncryptFile(rsa_inputFilePath, rsa_outputFilePath);

            // decrypt the encrypted file
            if (rsa_fileDecrypted.Text.Length == 0)
            {
                rsa_fileDecrypted.Text = "rsa_decrypted.txt";
            }

            rsa_inputFilePath = rsa_fileOutput.Text;
            rsa_outputFilePath = rsa_fileDecrypted.Text;
            rsa.DecryptFile(rsa_inputFilePath, rsa_outputFilePath);

            // compare the original file with the decrypted file
            TextBox hashTb = rsa_hash;
            hashTb.Text = "";
            compareTwoFilesWithSHA256(rsa_fileInput.Text, rsa_fileOutput.Text, hashTb);
            compareTwoFilesWithSHA256(rsa_fileInput.Text, rsa_fileDecrypted.Text, hashTb);
        }

        private void rsa_decryptFile_Click(object sender, EventArgs e)
        {
            // rsa
            getRSAInstance();

            // decrypt a file
            string rsa_inputFilePath = rsa_fileOutput.Text;

            if (rsa_fileDecrypted.Text.Length == 0)
            {
                rsa_fileDecrypted.Text = "rsa_decrypted.txt";
            }

            string rsa_outputFilePath = rsa_fileDecrypted.Text;
            rsa.DecryptFile(rsa_inputFilePath, rsa_outputFilePath);
        }

        // --------------- CFB

        private CFB_A5_1 createCFB_A5_1ObjectFromKeyInput()
        {
            //  convert cfb_keyInput to int[] and throw error if empty or if not cinsisted of  0 and 1
            int[] cfb_key = cfb_keyInput.Text.Select(c => int.Parse(c.ToString())).ToArray();
            if (cfb_key.Length == 0)
            {
                MessageBox.Show("Key is empty! Koristicemo demo kljuc");
                cfb_key = cfb_demo_key;
                cfb_keyInput.Text = string.Join("", cfb_key);
            }
            foreach (int i in cfb_key)
            {
                if (i != 0 && i != 1)
                {
                    MessageBox.Show("Key is not consisted of 0 and 1!");
                    return null;
                }
            }

            // get cfb_numOfBits
            int cfb_blockSize = (int)cfb_numOfBits.Value;

            // get iv vector
            int[] iv = cfb_iv_input.Text.Select(c => int.Parse(c.ToString())).ToArray();
            if (iv.Length == 0)
            {
                MessageBox.Show("IV is empty! Koristicemo demo IV vektor");
                iv = cfb_iv_demo;
                cfb_iv_input.Text = string.Join("", iv);
            }

            //  create a5_1 object
            CFB_A5_1 cfb_a5_1 = new CFB_A5_1(cfb_key, iv, cfb_blockSize);

            return cfb_a5_1;
        }

        private void cfb_fillKey_Click(object sender, EventArgs e)
        {
            cfb_keyInput.Text = string.Join("", cfb_demo_key);
            cfb_iv_input.Text = string.Join("", cfb_iv_demo);
        }

        private void cfb_demo_Click(object sender, EventArgs e)
        {
            int[] message = { 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0 };
            cfb_input.Text = string.Join("", message);
            cfb_encrypt_Click(sender, e);
        }

        private void cfb_encrypt_Click(object sender, EventArgs e)
        {
            CFB_A5_1 cfb = createCFB_A5_1ObjectFromKeyInput();
            if (cfb == null)
            {
                return;
            }

            // get input
            string input = cfb_input.Text;
            if (input.Length == 0)
            {
                input = "11001100";
                cfb_input.Text = input;
            }

            //  check if input is consisted of 0 and 1
            foreach (char c in input)
            {
                if (c != '0' && c != '1')
                {
                    MessageBox.Show("Input must be consisted of 0 and 1!");
                    return;
                }
            }

            // convert input to int[]
            int[] inputInt = input.Select(c => int.Parse(c.ToString())).ToArray();

            // encrypt
            int[] encrypted = cfb.Encrypt(inputInt);

            // convert int[] to string
            string encryptedString = string.Join("", encrypted);

            // populate cfb_output
            cfb_output.Text = encryptedString;

            // decrypt
            int[] decrypted = cfb.Decrypt(encrypted);

            // convert int[] to string
            string decryptedString = string.Join("", decrypted);

            // populate cfb_decrypted
            cfb_decrypted.Text = decryptedString;
        }

        // --------------- SHA
        private void sha_picker_Click(object sender, EventArgs e)
        {
            //  open shaFilePicker
            shaFilePicker.ShowDialog();

            string fileName = shaFilePicker.FileName;

            // check if file is correctly selected and exists
            if (fileName.Length == 0)
            {
                MessageBox.Show("File is not selected!");
                return;
            }

            if (!File.Exists(fileName))
            {
                MessageBox.Show("File does not exist!");
                return;
            }

            string hash = StringHashOfTheFile(fileName);

            //  populate hash textbox
            sha_results.Text += fileName + " - hash: " + hash + "\r\n";
        }
    }
}