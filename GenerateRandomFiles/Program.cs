using System;
using System.Linq;
using System.IO;
namespace GenerateRandomFiles
{
    class Program
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        static void Main(string[] args)
        {
            const int lineLength = 1000;
            WriteTo("no_text.txt", 0, lineLength);
            WriteTo("10_lines.txt", 10, lineLength);
            WriteTo("1k_lines.txt", 1000, lineLength);
            WriteTo("10_short_lines.txt", 10, 10);
        }
        static void WriteTo(string filename, int numberOfLines,int lineLength)
        {
            File.Delete(filename);
            var writer = File.AppendText(filename);
            for (int i = 0; i < numberOfLines; i++)
            {
                writer.WriteLine(RandomString(lineLength));
            }
            writer.Flush();
            writer.Close();
            
        }
    }
}
