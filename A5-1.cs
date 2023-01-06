using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing;
using System.Numerics;
using System.ComponentModel;
using System.Security.Policy;

namespace _17743_Mihajlo_Marjanovic_ZI_Projekat
{
    class A5_1
    {
        int[] key = null;

        int[] XOperationMembers = null;
        int[] YOperationMembers = null;
        int[] ZOperationMembers = null;

        int SIZEX = 19;
        int SIZEY = 22;
        int SIZEZ = 23;

        int[] x = null;
        int[] y = null;
        int[] z = null;

        public A5_1(int[] newKey)
        {
            XOperationMembers = new int[] { 13, 16, 17, 18 };
            YOperationMembers = new int[] { 20, 21 };
            ZOperationMembers = new int[] { 7, 20, 21, 22 };

            x = new int[SIZEX];
            y = new int[SIZEY];
            z = new int[SIZEZ];

            this.ReloadKey(newKey);
        }

        public void ReloadKey(int[] newKey = null)
        {
            if (newKey != null)
            {
                key = newKey;
            }

            int i = 0;
            int j = 0;

            while (i < 19)
            {
                x[j] = key[i];
                i++;
                j++;
            }
            j = 0;
            while (i < 41)
            {
                y[j] = key[i];
                i++;
                j++;
            }
            j = 0;
            while (i < 64)
            {
                z[j] = key[i];
                i++;
                j++;
            }
        }

        public void RegisterSteps(ref int[] sr, int[] OperationMembers)
        {
            int t = 0;
            foreach (int index in OperationMembers)
            {
                t ^= sr[index];
            }

            Shift(ref sr, t);
        }

        public int Shift(ref int[] reg, int newEntry)
        {
            int ret = reg[reg.Length - 1];

            for (int i = reg.Length - 1; i > 0; i--)
            {
                reg[i] = reg[i - 1];
            }

            reg[0] = newEntry;
            return ret;
        }

        public int[] Crypt(int[] source, bool shouldReload = true)
        {
            int[] result = new int[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                int c = source[i];
                int m = Majority(this.x[8], this.y[10], this.y[10]);

                if (this.x[8] == m)
                    RegisterSteps(ref x, XOperationMembers);

                if (this.y[10] == m)
                    RegisterSteps(ref y, YOperationMembers);

                if (this.z[10] == m)
                    RegisterSteps(ref z, ZOperationMembers);

                int s = 0;
                s ^= Output(x) ^ Output(y) ^ Output(z);
                result[i] = s ^ c;
            }

            if (shouldReload)
            {
                this.ReloadKey();
            }
            return result;
        }

        public int[] Decrypt(int[] source)
        {
            return this.Crypt(source);
        }

        public int Output(int[] reg)
        {
            return reg[reg.Length - 1];
        }

        private int Majority(int x, int y, int z)
        {
            int m = x;
            m += y;
            m += z;

            return m > 1 ? 1 : 0;
        }

        // function to generate small binary file which will be used in EncryptFile function as input
        public void GenerateTestBinaryFile()
        {
            FileStream fs = new FileStream("a5_1_input.bin", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);

            for (int i = 0; i < 3; i++)
            {
                // write random bit
                bw.Write((byte)1);
            }

            bw.Close();
            fs.Close();
        }

        // function to encrypt file using A5/1 algorithm
        public void EncryptFile(string inputFilePath, string outputFilePath)
        {
            FileStream fs = new FileStream(inputFilePath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);

            FileStream fs2 = new FileStream(outputFilePath, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs2);

            int[] input = new int[8];
            int[] output = new int[8];

            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (br.BaseStream.Position == br.BaseStream.Length)
                        break;
                    else
                        input[i] = br.ReadBoolean() ? 1 : 0;
                }

                output = this.Crypt(input, false);

                // print input and output

                //Console.Write("Input: ");

                //for (int i = 0; i < 8; i++)
                //{
                //    Console.Write(input[i]);
                //}

                //Console.Write(" Output: ");

                //for (int i = 0; i < 8; i++)
                //{
                //    Console.Write(output[i]);
                //}

                //Console.WriteLine();

                for (int i = 0; i < 8; i++)
                {
                    bw.Write(output[i] == 1 ? true : false);
                }
            }

            br.Close();
            fs.Close();

            bw.Close();
            fs2.Close();
            this.ReloadKey();
        }

        public void DecryptFile(string inputFilePath, string outputFilePath)
        {
            this.ReloadKey();
            EncryptFile(inputFilePath, outputFilePath);
        }


        //Metoda CreateBMPImage čita podatke sa 24-bitne bitmap slike i koristi metod Cript klase A5_1 za šifrovanje podataka.Zatim kreira novu 24-bitnu bitmap sliku od šifrovanih podataka.
        //Metod počinje učitavanjem ulazne slike u Bitmap objekat i dobijanjem njegove širine i visine. Zatim kreira niz podataka za skladištenje podataka o pikselima.
        //Zatim, metoda ponavlja svaki piksel na slici i izdvaja njene komponente crvene, zelene i plave boje. Zatim čuva ove komponente u nizu podataka kao pojedinačne bitove. 
        //Na primer, crvena komponenta piksela je uskladištena u prvih 8 elemenata podataka, 
        //zelena komponenta je uskladištena u sledećih 8 elemenata, 
        //a plava komponenta je uskladištena u poslednjih 8 elemenata.
        //Nakon što su svi podaci o pikselima ekstraktovani i uskladišteni u podacima, 
        //metoda poziva metodu Cript da šifruje podatke.
        //Zatim kreira novi Bitmap objekat i ponovo prelazi preko svakog piksela na slici, ovog puta izdvajajući šifrovane komponente boje iz podataka i postavljajući boje piksela u izlaznoj slici.
        //Konačno, izlazna slika se čuva u datoteci pod nazivom `A5_1_output.bmp
        public void CreateBMPImage(string filePath, ProgressBar progressBar)
        {
            // Load the BMP image into a Bitmap object
            Bitmap bmp = (Bitmap)Image.FromFile(filePath);

            // Get the width and height of the image
            int width = bmp.Width;
            int height = bmp.Height;

            // Create a BigInteger to store the data
            int[] data = new int[width * height * 24];

            // Iterate over each pixel in the image
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Get the pixel at the current position
                    Color pixel = bmp.GetPixel(x, y);

                    // Extract the red, green, and blue components of the pixel
                    byte r = pixel.R;
                    byte g = pixel.G;
                    byte b = pixel.B;

                    for (int i = 0; i < 8; i++)
                    {
                        data[y * width * 24 + x * 24 + i] = (r >> (7 - i)) & 1; // & 1 je maska za poslednji bit trenutnog shiftovanja u desno // ponavlja se 8 puta za ceo bajt
                        data[y * width * 24 + x * 24 + 8 + i] = (g >> (7 - i)) & 1;
                        data[y * width * 24 + x * 24 + 16 + i] = (b >> (7 - i)) & 1;
                    }
                }
            }

            progressBar.Value = 25;
            // Enkriptujemo trenutne podatke
            Console.WriteLine("bmp - message: " + data);
            data = this.Crypt(data);
            Console.WriteLine("bmp - enkriptovanje... " + data);
            progressBar.Value = 30;

            // Create a new Bitmap to store the converted image
            Bitmap output = new Bitmap(width, height);

            // Iterate over each pixel in the image
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Extract the red, green, and blue components of the pixel from the data array
                    byte r = 0;
                    byte g = 0;
                    byte b = 0;

                    for (int i = 0; i < 8; i++)
                    {
                        r |= (byte)(data[y * width * 24 + x * 24 + i] << (7 - i));
                        g |= (byte)(data[y * width * 24 + x * 24 + 8 + i] << (7 - i));
                        b |= (byte)(data[y * width * 24 + x * 24 + 16 + i] << (7 - i));
                    }

                    // Set the pixel at the current position to the extracted color
                    output.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            // Save the output image
            output.Save("A5_1_output.bmp", ImageFormat.Bmp);
            progressBar.Value = 50;
            // Enkriptujemo trenutne podatke
            data = Decrypt(data);
            Console.WriteLine("bmp - dekriptovanje... " + data);
            progressBar.Value = 55;

            // Create a new Bitmap to store the converted image
            output = new Bitmap(width, height);

            // Iterate over each pixel in the image
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Extract the red, green, and blue components of the pixel from the BigInteger
                    byte r = 0;
                    byte g = 0;
                    byte b = 0;

                    for (int i = 0; i < 8; i++)
                    {
                        r |= (byte)(data[y * width * 24 + x * 24 + i] << (7 - i));
                        g |= (byte)(data[y * width * 24 + x * 24 + 8 + i] << (7 - i));
                        b |= (byte)(data[y * width * 24 + x * 24 + 16 + i] << (7 - i));
                    }

                    // Set the pixel at the current position to the extracted color
                    output.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            // Save the output image
            output.Save("A5_1_decrypted_image.bmp", ImageFormat.Bmp);
            progressBar.Value = 80;
        }
    }
}
