using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace _17743_Mihajlo_Marjanovic_ZI_Projekat
{
    internal class SHA256
    {
        private static readonly uint[] K = new uint[64]
        {
            1116352408,
            1899447441,
            3049323471,
            3921009573,
            961987163,
            1508970993,
            2453635748,
            2870763221,
            3624381080,
            310598401,
            607225278,
            1426881987,
            1925078388,
            2162078206,
            2614888103,
            3248222580,
            3835390401,
            4022224774,
            264347078,
            604807628,
            770255983,
            1249150122,
            1555081692,
            1996064986,
            2554220882,
            2821834349,
            2952996808,
            3210313671,
            3336571891,
            3584528711,
            113926993,
            338241895,
            666307205,
            773529912,
            1294757372,
            1396182291,
            1695183700,
            1986661051,
            2177026350,
            2456956037,
            2730485921,
            2820302411,
            3259730800,
            3345764771,
            3516065817,
            3600352804,
            4094571909,
            275423344,
            430227734,
            506948616,
            659060556,
            883997877,
            958139571,
            1322822218,
            1537002063,
            1747873779,
            1955562222,
            2024104815,
            2227730452,
            2361852424,
            2428436474,
            2756734187,
            3204031479,
            3329325298
        };

        private static readonly uint[] H = new uint[8]
        {
            1779033703,
            3144134277,
            1013904242,
            2773480762,
            1359893119,
            2600822924,
            528734635,
            1541459225
        };

        private static uint Ch(uint x, uint y, uint z)
        {
            return (x & y) ^ (~x & z);
        }

        private static uint Maj(uint x, uint y, uint z)
        {
            return (x & y) ^ (x & z) ^ (y & z);
        }

        private static uint Sigma0(uint x)
        {
            return (x >> 2 | x << 30) ^ (x >> 13 | x << 19) ^ (x >> 22 | x << 10);
        }

        private static uint Sigma1(uint x)
        {
            return (x >> 6 | x << 26) ^ (x >> 11 | x << 21) ^ (x >> 25 | x << 7);
        }

        //ComputeHash funkcija se koristi u SHA-256 algoritmu da izračuna hash vrednost za dati ulazni niz bajtova.
        //Ona prima niz 32-bitnih vrednosti message, koji predstavlja ulaznu poruku koju želimo da hash-ujemo.
        //Prvo, pravi se novi niz od osam 32-bitnih vrednosti - result, koji se inicijalizuje kopiranjem vrednosti iz niza H u result. 
        //Zatim, u for petlji od 0 do 63 se izvršavaju određene operacije sa vrednostima u result nizu i sa vrednostima iz niza message.
        //Na kraju, result niz se vraća kao rezultat funkcije.

        private static uint[] ComputeHash(uint[] message)
        {
            uint[] result = new uint[8];
            Array.Copy(H, result, 8);
            for (int i = 0; i < 64; i++)
            {
                uint t1 = result[7] + Sigma1(result[4]) + Ch(result[4], result[5], result[6]) + K[i] + message[i];
                uint t2 = Sigma0(result[0]) + Maj(result[0], result[1], result[2]);
                result[7] = result[6];
                result[6] = result[5];
                result[5] = result[4];
                result[4] = result[3] + t1;
                result[3] = result[2];
                result[2] = result[1];
                result[1] = result[0];
                result[0] = t1 + t2;
            }
            return result;
        }

        //ComputeHash funkcija prima niz bajtova message i vraća niz bajtova koji predstavlja hash vrednost za datu ulaznu poruku.
        //Ova druga funkcija prvo kreira niz od 16 32-bitnih vrednosti - array, koji se inicijalizuje kopiranjem vrednosti iz niza H u array.
        //Zatim, računa se broj ciklusa for petlje koji su neophodni za obradu celog ulaznog niza bajtova. 
        //U svakom ciklusu, pravi se novi niz od 64 32-bitne vrednosti - array2, koji se inicijalizuje kopiranjem dela ulaznog niza bajtova u array2. 
        //Zatim se poziva ComputeHash funkcija sa array2 kao argumentom i rezultat se smešta u novi niz od osam 32-bitnih vrednosti - array3.
        //Dalje, u for petlji od 0 do 7 se vrši sabiranje vrednosti iz niza array3 (rezultata poziva ComputeHash funkcije sa array2 kao argumentom) sa odgovarajućim vrednostima iz niza array.
        //Na kraju, pravi se novi niz od 32 bajta - array4 i u for petlji od 0 do 7 se kopiraju odgovarajuće vrednosti iz array niza u array4 niz. 
        //Na kraju se array4 niz vraća kao rezultat poziva druge ComputeHash funkcije.

        public static byte[] ComputeHash(byte[] message)
        {
            uint[] array = new uint[16];
            Array.Copy(H, array, 8);
            int num = message.Length / 64;
            
            for (int i = 0; i < num; i++)
            {
                uint[] array2 = new uint[64];
                Array.Copy(message, i * 64, array2, 0, 64);
                uint[] array3 = ComputeHash(array2);
                for (int j = 0; j < 8; j++)
                {
                    array[j] += array3[j];
                }
            }

            // Ukoliko je broj ciklusa iznad (num) bio 0, radimo ovaj pomocni deo da ne bi hash za 0 ciklusa svima bio isti
            int remainingBytes = message.Length % 64;
            if (remainingBytes != 0)
            {
                uint[] array2 = new uint[64];
                Array.Copy(message, num * 64, array2, 0, remainingBytes);
                uint[] array3 = ComputeHash(array2);
                for (int j = 0; j < 8; j++)
                {
                    array[j] += array3[j];
                }
            }

            byte[] array4 = new byte[32];
            for (int k = 0; k < 8; k++)
            {
                array4[k * 4] = (byte)(array[k] >> 24);
                array4[k * 4 + 1] = (byte)(array[k] >> 16);
                array4[k * 4 + 2] = (byte)(array[k] >> 8);
                array4[k * 4 + 3] = (byte)array[k];
            }
            return array4;
        }

        public static byte[] ComputeHash(string message)
        {
            return ComputeHash(Encoding.UTF8.GetBytes(message));
        }
    }
}
