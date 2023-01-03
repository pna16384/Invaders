// See https://aka.ms/new-console-template for more information

using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;
using System.Security;
using System.Security.Cryptography;
using System.Text;


Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("Window Width = {0}; window height = {1}", Console.WindowWidth, Console.WindowHeight);

try
{
    Console.SetWindowSize(80, 40);
}
catch (SecurityException e)
{
    Console.WriteLine(e.ToString());
}
finally
{
    Console.WriteLine("Window Width = {0}; window height = {1}", Console.WindowWidth, Console.WindowHeight);
}

string invader = new string("👾"); // U+1F47E
string invader2 = "\ud83c\udf81"; //utf16
Console.WriteLine(invader + "  ;  " + invader2);
//1f499 (blue heart)


string? GenerateUTF8String(uint? cp)
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

string? GenerateUTF16String(uint? cp)
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


// Test
bool quit = false;

while(!quit)
{
    Console.Write("Enter Unicode codepoint as hexadecimal value: U+");
    string? characterString = Console.ReadLine();

    if (characterString == "quit")
        break;

    uint? codePoint = null;

    try
    {
        codePoint = uint.Parse(characterString!, System.Globalization.NumberStyles.HexNumber);
    }
    catch (Exception e)
    {
        Console.WriteLine("*** " + e.Message);
    }

    string? utf8String = GenerateUTF8String(codePoint);
    string? utf16String = GenerateUTF16String(codePoint);

    if (utf8String is string str8)
        Console.WriteLine("UTF8 string is {0}", str8);

    if (utf16String is string str16)
        Console.WriteLine("UTF16 string is {0}", str16);
}










