using host.MMF.BigArray;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test_big_array1
{
    class Program
    {
        static void Main(string[] args)
        {
            long size = 0x1FFFFFFFF;
            //long size = UInt32.MaxValue;
            //long size = int.MaxValue;
            //byte[] countryCodes = new byte[size];

            BigByteArray baInt = new BigByteArray(size);
            long len = baInt.Length;

            for (long i = 0; i < len; i++)
            {
                baInt[i] = 255;
                Console.Title = i.ToString();
            }

            Console.WriteLine("baInt[len/2] = " + baInt[len / 2]);


            Console.ReadKey();
        }
    }
}
