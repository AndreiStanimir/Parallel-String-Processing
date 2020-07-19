using System;
using System.IO;
using System.Linq;

namespace GenerateRandomFiles
{
    internal class Program
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static void Main()
        {
            const int lineLength = 1000;
            WriteTo("no_text.txt", 0, lineLength);
            WriteTo("10_lines.txt", 10, lineLength);
            WriteTo("1k_lines.txt", 1000, lineLength);
            WriteTo("10_short_lines.txt", 10, 10);
        }

        private static void WriteTo(string filename, int numberOfLines, int lineLength)
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