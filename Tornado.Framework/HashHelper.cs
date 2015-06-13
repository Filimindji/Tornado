using System;
using System.IO;
using System.Text;

namespace Tornado.Framework
{

    public class HashHelper
    {
        public static string ComputeHash(string filename)
        {
            if (File.Exists(filename) == false)
                return null;

            ulong result;
            using (Stream input = File.OpenRead(filename))
            {
                result = ComputeHash(input);
            }
            return ToHexadecimal(result);
        }

        //public byte[] ComputeHash(Stream input)
        //{
        //    if (input == null)
        //        return null;

        //    long streamsize = input.Length;
        //    long lhash = streamsize;

        //    long i = 0;
        //    byte[] buffer = new byte[sizeof(long)];
        //    while (i < 65536 / sizeof(long) && (input.Read(buffer, 0, sizeof(long)) > 0))
        //    {
        //        i++;
        //        unchecked
        //        {
        //            lhash += BitConverter.ToInt64(buffer, 0);

        //        }
        //    }

        //    input.Position = Math.Max(0, streamsize - 65536);
        //    i = 0;
        //    while (i < 65536 / sizeof(long) && (input.Read(buffer, 0, sizeof(long)) > 0))
        //    {
        //        i++;
        //        unchecked
        //        {
        //            lhash += BitConverter.ToInt64(buffer, 0);
        //        }
        //    }
        //    input.Close();
        //    byte[] result = BitConverter.GetBytes(lhash);
        //    Array.Reverse(result);
        //    return result;
        //}

        private static ulong ComputeHash(Stream input)
        {
            if (input == null)
                return 0;


            ulong lhash = (ulong)input.Length;
            byte[] buf = new byte[65536 * 2];

            input.Read(buf, 0, 65536);
            input.Position = Math.Max(0, input.Length - 65536);
            input.Read(buf, 65536, 65536);

            for (int i = 0; i < 2 * 65536; ) unchecked
                {
                    //source data is always considered little endian, BitConverter won't correctly convert that on big endian platforms -> convert it manually
                    lhash += (ulong)buf[i++] << 0 | (ulong)buf[i++] << 8 | (ulong)buf[i++] << 16 | (ulong)buf[i++] << 24 | (ulong)buf[i++] << 32 | (ulong)buf[i++] << 40 | (ulong)buf[i++] << 48 | (ulong)buf[i++] << 56;
                }
            return lhash;

        }



        public static string ToHexadecimal(ulong l)
        {
            StringBuilder hexBuilder = new StringBuilder();
            for (int shift = 56; shift >= 0; shift -= 8)
            {
                hexBuilder.Append((l >> shift & 0xFF).ToString("x2"));
            }
            return hexBuilder.ToString();
        }


    }


}