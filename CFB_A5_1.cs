using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17743_Mihajlo_Marjanovic_ZI_Projekat
{
    class CFB_A5_1
    {
        // Instanca šifre A5_1 i niz za povratnu spregu
        private A5_1 cipher;
        private int[] feedback;

        // Veličina bloka za jednu fazu šifrovanja
        private int blockSize;

        // Prima ključ, niz za inicijalizaciju i veličinu bloka
        public CFB_A5_1(int[] key, int[] iv, int blockSize)
        {
            cipher = new A5_1(key);
            feedback = iv;
            this.blockSize = blockSize;
        }


        // Funkcija za šifrovanje koja prima niz 0 i 1 i vraća niz šifrovanih 0 i 1
        public int[] Encrypt(int[] plaintext)
        {
            int[] ciphertext = new int[plaintext.Length];

            // Obradjujemo svaki blok
            for (int i = 0; i < plaintext.Length; i += blockSize)
            {
                // Šifrujemo feedback niz koristeći šifru A5/1
                feedback = cipher.Crypt(feedback, shouldReload: false);

                // XOR-ujemo prvi feedback niz sa svakim elementom u trenutnom bloku
                // i smeštamo rezultat u odgovarajući element niza šifrovanih 0 i 1
                for (int j = 0; j < blockSize; j++)
                {
                    ciphertext[i + j] = feedback[j] ^ plaintext[i + j];
                    feedback[j] = ciphertext[i + j];
                }
            }

            cipher.ReloadKey();
            return ciphertext;
        }

        // Funkcija za dešifrovanje koja prima niz šifrovanih 0 i 1 i vraća niz početnih 0 i 1
        public int[] Decrypt(int[] ciphertext)
        {
            int[] plaintext = new int[ciphertext.Length];

            // Obradjujemo svaki blok
            for (int i = 0; i < ciphertext.Length; i += blockSize)
            {
                // Šifrujemo feedback niz koristeći šifru A5/1
                feedback = cipher.Crypt(feedback, shouldReload: false);

                // XOR-ujemo prvi feedback niz sa svakim elementom u trenutnom bloku
                // i smeštamo rezultat u odgovarajući element niza dešifrovanih 0 i 1
                for (int j = 0; j < blockSize; j++)
                {
                    plaintext[i + j] = feedback[j] ^ ciphertext[i + j];
                    feedback[j] = ciphertext[i + j];
                }
            }

            cipher.ReloadKey();
            return plaintext;
        }
    }






    //private A5_1 cipher;
    //private int[] feedback;

    //public CFB_A5_1(int[] key)
    //{
    //    cipher = new A5_1(key);
    //    feedback = new int[key.Length];
    //}

    //public int[] Encrypt(int[] plaintext)
    //{
    //    int[] ciphertext = new int[plaintext.Length];

    //    for (int i = 0; i < plaintext.Length; i++)
    //    {
    //        feedback = cipher.Crypt(feedback, shouldReload: false);
    //        ciphertext[i] = feedback[0] ^ plaintext[i];
    //        feedback = Shift(feedback, ciphertext[i]);
    //    }

    //    cipher.ReloadKey();
    //    return ciphertext;
    //}

    //public int[] Decrypt(int[] ciphertext)
    //{
    //    int[] plaintext = new int[ciphertext.Length];

    //    for (int i = 0; i < ciphertext.Length; i++)
    //    {
    //        feedback = cipher.Crypt(feedback, shouldReload: false);
    //        plaintext[i] = feedback[0] ^ ciphertext[i];
    //        feedback = Shift(feedback, ciphertext[i]);
    //    }

    //    cipher.ReloadKey();
    //    return plaintext;
    //}

    //private int[] Shift(int[] feedback, int newEntry)
    //{
    //    for (int i = feedback.Length - 1; i > 0; i--)
    //    {
    //        feedback[i] = feedback[i - 1];
    //    }

    //    feedback[0] = newEntry;
    //    return feedback;
    //}
}
