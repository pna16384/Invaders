using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invaders
{

    internal interface CharacterEncoder
    {
        abstract static string? GenerateUTF8String(uint? cp);
        abstract static string? GenerateUTF16String(uint? cp);
    }

    internal class UTFEncoder : CharacterEncoder
    {
        public static string? GenerateUTF8String(uint? cp)
        {
            byte[]? bytes = null;

            // Strip bit sequences for leading and trailing utf8 bytes and bitwise-or bit sequences into utf8 byte sequence
            switch (cp)
            {
                case <= 0x7f:
                    //0xxxxxxx
                    bytes = new byte[] { (byte)cp };
                    break;

                case >= 0x80 and <= 0x7ff:
                    //110xxxxx	10xxxxxx
                    bytes = new byte[] {
                (byte)(0xC0 | (0x1f & cp >> 6)),
                (byte)(0x80 | (0x3f & cp))
            };
                    break;

                case >= 0x800 and <= 0xffff:
                    //1110xxxx	10xxxxxx	10xxxxxx
                    bytes = new byte[] {
                (byte)(0xe0 | (0x0f & cp >> 12)),
                (byte)(0x80 | (0x3f & cp >> 6)),
                (byte)(0x80 | (0x3f & cp))
            };
                    break;

                case >= 0x10000 and <= 0x10ffff:
                    //11110xxx	10xxxxxx	10xxxxxx	10xxxxxx
                    bytes = new byte[] {
                (byte)(0xf0 | (0x07 & cp >> 18)),
                (byte)(0x80 | (0x3f & cp >> 12)),
                (byte)(0x80 | (0x3f & cp >> 6)),
                (byte)(0x80 | (0x3f & cp))
            };
                    break;
            }

            if (bytes is byte[])
                return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            else
                return null;
        }

        public static string? GenerateUTF16String(uint? cp)
        {
            byte[]? bytes = null;

            switch (cp)
            {
                case >= 0x0 and <= 0xffff:
                    bytes = new byte[] { (byte)(cp & 0xff), (byte)(cp >> 8) };
                    break;

                case >= 0x10000 and <= 0x10FFFF:
                    cp -= 0x10000;
                    ushort u0 = (ushort)(0xD800 | (cp >> 10));
                    ushort u1 = (ushort)(0xDC00 | (cp & 0x3FF));

                    bytes = new byte[] { (byte)(u0 & 0xff), (byte)(u0 >> 8 & 0xff), (byte)(u1 & 0xff), (byte)(u1 >> 8 & 0xff) };
                    break;
            }

            if (bytes is byte[])
                return Encoding.Unicode.GetString(bytes, 0, bytes.Length);
            else
                return null;
        }
    }
}
