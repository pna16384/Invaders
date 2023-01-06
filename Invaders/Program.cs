// See https://aka.ms/new-console-template for more information

using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Invaders;

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

    string? utf8String = UTFEncoder.GenerateUTF8String(codePoint);
    string? utf16String = UTFEncoder.GenerateUTF16String(codePoint);

    if (utf8String is string str8)
        Console.WriteLine("UTF8 string is {0}", str8);

    if (utf16String is string str16)
        Console.WriteLine("UTF16 string is {0}", str16);
}










