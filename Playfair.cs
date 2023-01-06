using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17743_Mihajlo_Marjanovic_ZI_Projekat
{
    class Playfair
    {
        private readonly char[,] _keyTable;
        private readonly int _keyTableSize;

        public Playfair(string key)
        {
            _keyTableSize = 5;
            _keyTable = new char[_keyTableSize, _keyTableSize];
            CreateKeyTable(key);
        }

        private void CreateKeyTable(string key)
        {
            // Create a set to store unique characters in the key
            var keySet = new HashSet<char>();

            // Add all unique characters in the key to the set
            foreach (char c in key)
            {
                if (c != 'j' && !keySet.Contains(c))
                {
                    keySet.Add(c);
                }
            }

            // Add all remaining letters of the alphabet to the set
            for (char c = 'a'; c <= 'z'; c++)
            {
                if (c != 'j' && !keySet.Contains(c))
                {
                    keySet.Add(c);
                }
            }

            // Add the characters in the set to the key table
            int i = 0;
            int j = 0;
            foreach (char c in keySet)
            {
                _keyTable[i, j] = c;
                j++;
                if (j == _keyTableSize)
                {
                    i++;
                    j = 0;
                }
            }
        }

        private (int, int) GetCharPos(char c)
        {
            if (c == 'j')
                c = 'i';
            for (int i = 0; i < _keyTableSize; i++)
            {
                for (int j = 0; j < _keyTableSize; j++)
                {
                    if (_keyTable[i, j] == c)
                    {
                        return (i, j);
                    }
                }
            }

            // Vracamo Tuple
            return (-1, -1);
        }

        public string Encrypt(string plaintext)
        {
            // Remove spaces and non-alphabetic characters from the plaintext
            plaintext = plaintext.ToLower().Replace(" ", "");

            // Add an 'x' between consecutive identical characters
            var modifiedPlaintext = new List<char>();
            for (int i = 0; i < plaintext.Length; i++)
            {
                if (i < plaintext.Length - 1 && plaintext[i] == plaintext[i + 1])
                {
                    modifiedPlaintext.Add(plaintext[i]);
                    modifiedPlaintext.Add('x');
                }
                else
                {
                    modifiedPlaintext.Add(plaintext[i]);
                }
            }

            // If the length of the modified plaintext is odd, add an 'x' at the end
            if (modifiedPlaintext.Count % 2 == 1)
            {
                modifiedPlaintext.Add('x');
            }

            // Encrypt the modified plaintext
            var ciphertext = new List<char>();
            for (int i = 0; i < modifiedPlaintext.Count; i += 2)
            {
                var pos1 = GetCharPos(modifiedPlaintext[i]);
                var pos2 = GetCharPos(modifiedPlaintext[i + 1]);

                if (pos1.Item1 == pos2.Item1)
                {
                    // Same row
                    ciphertext.Add(_keyTable[pos1.Item1, (pos1.Item2 + 1) % _keyTableSize]);
                    ciphertext.Add(_keyTable[pos2.Item1, (pos2.Item2 + 1) % _keyTableSize]);
                }
                else if (pos1.Item2 == pos2.Item2)
                {
                    // Same column
                    ciphertext.Add(_keyTable[(pos1.Item1 + 1) % _keyTableSize, pos1.Item2]);
                    ciphertext.Add(_keyTable[(pos2.Item1 + 1) % _keyTableSize, pos2.Item2]);
                }
                else
                {
                    // Different row and column
                    // Pravimo kvadrat - uzimamo suprotne koordinate jer su na suprotnoj dijagonali
                    ciphertext.Add(_keyTable[pos1.Item1, pos2.Item2]);
                    ciphertext.Add(_keyTable[pos2.Item1, pos1.Item2]);
                }
            }

            return new string(ciphertext.ToArray());
        }

        public string Decrypt(string ciphertext)
        {
            // Decrypt the ciphertext
            var plaintext = new List<char>();
            for (int i = 0; i < ciphertext.Length; i += 2)
            {
                var pos1 = GetCharPos(ciphertext[i]);
                var pos2 = GetCharPos(ciphertext[i + 1]);

                if (pos1.Item1 == pos2.Item1)
                {
                    // Same row
                    plaintext.Add(_keyTable[pos1.Item1, (pos1.Item2 - 1 + _keyTableSize) % _keyTableSize]);
                    plaintext.Add(_keyTable[pos2.Item1, (pos2.Item2 - 1 + _keyTableSize) % _keyTableSize]);
                }
                else if (pos1.Item2 == pos2.Item2)
                {
                    // Same column
                    plaintext.Add(_keyTable[(pos1.Item1 - 1 + _keyTableSize) % _keyTableSize, pos1.Item2]);
                    plaintext.Add(_keyTable[(pos2.Item1 - 1 + _keyTableSize) % _keyTableSize, pos2.Item2]);
                }
                else
                {
                    // Different row and column
                    plaintext.Add(_keyTable[pos1.Item1, pos2.Item2]);
                    plaintext.Add(_keyTable[pos2.Item1, pos1.Item2]);
                }
            }

            // Remove any 'x' characters added during encryption
            return new string(plaintext.ToArray()).Replace("x", "");
        }
        
        public void GenerateDemoInputFile()
        {
            string plaintext = "Ovo je demonstracija Playfair algoritma";

            File.WriteAllText("playfair_demoInput.txt", plaintext);
        }

        public void EncryptFile(string inputFilePath, string outputFilePath)
        {
            // Read the plaintext from the input file
            string plaintext = File.ReadAllText(inputFilePath);

            // Encrypt the plaintext
            string ciphertext = Encrypt(plaintext);

            // Write the ciphertext to the output file
            File.WriteAllText(outputFilePath, ciphertext);
        }

        public void DecryptFile(string inputFilePath, string outputFilePath)
        {
            // Read the ciphertext from the input file
            string ciphertext = File.ReadAllText(inputFilePath);

            // Decrypt the ciphertext
            string plaintext = Decrypt(ciphertext);

            // Write the plaintext to the output file
            File.WriteAllText(outputFilePath, plaintext);
        }

        public void EncryptFileParallel(string inputFile, string outputFile, int numThreads)
        {
            // Read the input file and split it into chunks
            string[] chunks = ReadAndSplitFile(inputFile, numThreads);

            // Encrypt the chunks in parallel
            object lockObject = new object();
            var encryptedChunks = new List<string>();
            Parallel.ForEach(chunks, new ParallelOptions { MaxDegreeOfParallelism = numThreads }, chunk =>
            {
                // Encrypt the plaintext
                string ciphertext = Encrypt(chunk);

                // Add the encrypted chunk to the list of encrypted chunks
                lock (lockObject)
                {
                    encryptedChunks.Add(string.Join("", ciphertext));
                }
            });

            // Write the encrypted chunks to the output file
            WriteToFile(outputFile, encryptedChunks);
        }

        private string[] ReadAndSplitFile(string file, int numChunks)
        {
            // Read the entire file into a string
            string fileContent = File.ReadAllText(file);

            // Calculate the size of each chunk
            int chunkSize = fileContent.Length / numChunks;

            // Split the file content into chunks
            string[] chunks = new string[numChunks];
            for (int i = 0; i < numChunks; i++)
            {
                int startIndex = i * chunkSize;
                int endIndex = startIndex + chunkSize;
                if (i == numChunks - 1)
                {
                    // Last chunk may be smaller
                    endIndex = fileContent.Length;
                }
                chunks[i] = fileContent.Substring(startIndex, endIndex - startIndex);
            }

            return chunks;
        }

        private void WriteToFile(string file, List<string> chunks)
        {
            using (StreamWriter writer = new StreamWriter(file))
            {
                foreach (string chunk in chunks)
                {
                    writer.Write(chunk);
                }
            }
        }
    }
}


//public byte[] ReadDataFromImage(string imageFilePath)
//{

//    /*
//        The method reads the contents of the image file into a byte array 
//    using the File.ReadAllBytes method.
//        It then extracts the starting index of the pixel data in the byte array
//    using the BitConverter.ToInt32 method. In a 24-bit BMP image, 
//    the starting index of the pixel data is stored at the 10th byte in the file.
//        It also extracts the length of the pixel data using the BitConverter.ToInt32 method.
//    In a 24-bit BMP image, the length of the pixel data is stored at the 34th byte in the file.
//    It then uses the Array.Copy method to extract the pixel data from the byte array 
//    and store it in a new byte array.
//        Finally, it returns the pixel data as a byte array.
//    */
//    // Read the image file into a byte array
//    byte[] imageData = File.ReadAllBytes(imageFilePath);

//    // Get the starting index of the pixel data in the byte array
//    int pixelDataStartIndex = BitConverter.ToInt32(imageData, 10);

//    // Get the length of the pixel data
//    int pixelDataLength = BitConverter.ToInt32(imageData, 34);

//    // Extract the pixel data from the byte array
//    var pixelData = new byte[pixelDataLength];
//    Array.Copy(imageData, pixelDataStartIndex, pixelData, 0, pixelDataLength);

//    return pixelData;
//}

//public void CreateImageFromEncryptedData(string imageFilePath, byte[] encryptedData)
//{
//    // Read the header and metadata from the original image file
//    byte[] imageHeader = File.ReadAllBytes(imageFilePath).Take(54).ToArray();

//    // Update the length of the pixel data in the header
//    byte[] pixelDataLength = BitConverter.GetBytes(encryptedData.Length);
//    Array.Copy(pixelDataLength, 0, imageHeader, 34, 4);

//    // Concatenate the header, encrypted data, and padding to create the output image
//    byte[] padding = new byte[encryptedData.Length % 4];
//    byte[] outputImageData = imageHeader.Concat(encryptedData).Concat(padding).ToArray();

//    // Write the output image data to a file
//    File.WriteAllBytes(imageFilePath, outputImageData);
//}