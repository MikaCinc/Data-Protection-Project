using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Drawing.Imaging;
using Image = System.Drawing.Image;

namespace _17743_Mihajlo_Marjanovic_ZI_Projekat
{
    class RSA
    {
        private BigInteger _e;
        private BigInteger _n;
        private BigInteger _d;
        private BigInteger _phi;

        public RSA(int keySize)
        {
            // Generate prime numbers p and q
            BigInteger p = GeneratePrime(keySize / 2);
            BigInteger q = GeneratePrime(keySize / 2);

            // Calculate n = p * q
            _n = BigInteger.Multiply(p, q);

            // Euklidov phi je broj integera manjih ili jednakih sa N koji ne dele nijedan zajednički faktor sa N
            // Npr: phi(8) = 4 // [1,3,5,7]
            // phi od prostog broja P je P-1
            // Pa je zato lako da se izračuna phi(n) koga čini proizvod dva prosta broja p i q, na sledeći način:
            // phi = (p - 1) * (q - 1)
            _phi = BigInteger.Multiply(BigInteger.Subtract(p, 1), BigInteger.Subtract(q, 1));

            // Generate a public key e such that gcd(e, phi) = 1
            // Neparan broj koji ne deli zajednički faktor sa phi i n
            _e = GeneratePublicKey(_phi);

            // Calculate d such that d * e = 1 mod phi
            // d= e^-1 mod phi(n)
            _d = ModInverse(_e, _phi);
        }

        public BigInteger PublicKey => _e;
        public BigInteger PrivateKey => _d;
        public BigInteger Modulus => _n;
        public BigInteger Phi => _phi;

        public RSA(BigInteger e, BigInteger n, BigInteger d, BigInteger phi)
        {
            // Validate the keys
            if (e <= 1 || e >= phi)
            {
                throw new Exception("Invalid public key");
            }
            if (d <= 0 || d >= phi)
            {
                throw new Exception("Invalid private key");
            }
            if (BigInteger.GreatestCommonDivisor(e, phi) != 1)
            {
                throw new Exception("Invalid public key");
            }
            if (d * e % phi != 1)
            {
                throw new Exception("Invalid private key");
            }

            _e = e;
            _n = n;
            _d = d;
            _phi = phi;
        }

        private BigInteger GeneratePublicKey(BigInteger phi)
        {
            // E je neparan broj koji ne deli zajednički faktor sa phi i n
            // Generate a random number in the range [2, phi - 1]
            BigInteger e = GenerateRandomInteger(2, phi - 1);

            // Keep generating random numbers until a number is found such that gcd(e, phi) = 1
            while (BigInteger.GreatestCommonDivisor(e, phi) != 1)
            {
                e = GenerateRandomInteger(2, phi - 1);
            }

            return e;
        }

        private BigInteger GeneratePrime(int keySize)
        {
            // Generate a random number of the specified key size
            BigInteger number = GenerateRandomInteger(keySize);

            // If the number is even, add 1 to make it odd
            if (number % 2 == 0)
            {
                number += 1;
            }

            // Keep generating random numbers until a prime is found
            while (!IsPrime(number))
            {
                number += 2;
            }

            return number;
        }

        private BigInteger GenerateRandomInteger(int keySize)
        {
            // Generate a random byte array of the specified key size
            var randomBytes = new byte[keySize / 8];
            //RNGCryptoServiceProvider is an implementation of a random number generator.
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes); // The array is now filled with cryptographically strong random bytes.
            }

            // Convert the byte array to a big integer
            BigInteger number = new BigInteger(randomBytes);

            // If the number is negative, negate it
            if (number < 0)
            {
                number = -number;
            }

            return number;
        }

        private BigInteger GenerateRandomInteger(BigInteger min, BigInteger max)
        {
            // Generate a random byte array of the same size as min and max
            var randomBytes = new byte[Math.Max(min.ToByteArray().Length, max.ToByteArray().Length)];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            // Convert the byte array to a big integer
            BigInteger number = new BigInteger(randomBytes);

            // If the number is negative, negate it
            if (number < 0)
            {
                number = -number;
            }

            // If the number is less than min, add min to it
            if (number < min)
            {
                number += min;
            }

            // If the number is greater than max, subtract max from it
            if (number > max)
            {
                number -= max;
            }

            return number;
        }

        private BigInteger ModInverse(BigInteger a, BigInteger b)
        {
            // Calculate the inverse of a mod b using the extended Euclidean algorithm
            BigInteger b0 = b, t, q;
            BigInteger x0 = 0, x1 = 1;
            if (b == 1) return 1;
            while (a > 1)
            {
                q = a / b;
                t = b;
                b = a % b; a = t;
                t = x0;
                x0 = x1 - q * x0;
                x1 = t;
            }
            if (x1 < 0) x1 += b0;
            return x1;
        }

        private bool IsPrime(BigInteger number)
        {
            // Check if the number is even
            if (number % 2 == 0)
            {
                return false;
            }

            // Check if the number is prime by trying to divide it by odd numbers in the range [3, number/2]
            for (BigInteger i = 3; i <= BigInteger.Divide(number, 2); i += 2)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public BigInteger Encrypt(BigInteger message)
        {
            // c = m^e mod n
            return BigInteger.ModPow(message, _e, _n);
        }

        public BigInteger Decrypt(BigInteger ciphertext)
        {
            // m = c^d mod n
            return BigInteger.ModPow(ciphertext, _d, _n);
        }

        public void GenerateDemoInputFile()
        {
            string plaintext = "78787878";

            File.WriteAllText("rsa_demoInput.txt", plaintext);
        }
        public void EncryptFile(string inputFilePath, string outputFilePath)
        {
            // Read the input file into a string
            string message = File.ReadAllText(inputFilePath);

            // Convert the ciphertext string to a big integer
            BigInteger number = BigInteger.Parse(message);

            // Encrypt the plaintext string
            BigInteger cipher = Encrypt(number);

            // Write the ciphertext to the output file
            File.WriteAllText(outputFilePath, cipher.ToString());
        }

        public void DecryptFile(string inputFilePath, string outputFilePath)
        {
            // Read the input file into a string
            string ciphertextString = File.ReadAllText(inputFilePath);

            // Convert the ciphertext string to a big integer
            BigInteger cipher = BigInteger.Parse(ciphertextString);

            // Decrypt the ciphertext
            BigInteger original = Decrypt(cipher);

            // Write the plaintext to the output file
            File.WriteAllText(outputFilePath, original.ToString());
        }
    }
}
