using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Text.RegularExpressions;

namespace Test
{
    class Program
    {
        public static Dictionary<string,int> LoadDictionary(string file)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            string[] lines = File.ReadAllLines(file);
            foreach (var item in lines)
            {
                dict.Add(item, item.Length);
            }
            return dict;
        }
        static void Main(string[] args)
        {
            var dict = LoadDictionary("words.txt");
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            
            byte[] data = new byte[1024];
            string input = "";
            string stringData = "";
            string wordToGuess = "";
            int wordToGuessLength = 0;
            bool isFilteredByLength = false;
            TcpClient server;
            var regex = new Regex(@"\n\n(.*)\n\n");
            

            try
            {
                server = new TcpClient("52.49.91.111", 9988);
            }
            catch (SocketException)
            {
                Console.WriteLine("Unable to connect to server");
                return;
            }
            NetworkStream ns = server.GetStream();

            int recv = ns.Read(data, 0, data.Length);
            stringData = Encoding.ASCII.GetString(data, 0, recv);
            Console.WriteLine(stringData);

            while (true)
            {
                if (stringData.Contains("Press enter to continue"))
                {
                    input = "\n";
                }

                if (stringData.Contains("OVER"))
                {
                    Console.WriteLine("GAME OVER WITH DICT LENGTH : {0}", dict.Count);
                    Console.Read();
                }

                var match = regex.Match(stringData);
                if (match.Success)
                {
                    wordToGuess = match.Groups[1].Value.Replace(" ","");
                    wordToGuessLength = wordToGuess.Length;
                    // Filter length
                    if (!isFilteredByLength)
                    {
                        dict = dict.Where(x => x.Value == wordToGuessLength).ToDictionary(x => x.Key, x => x.Value);
                        isFilteredByLength = true;
                    }
                    var subRegex = new Regex(wordToGuess.Replace("_", "\\w"));
                    dict = dict.Where(x => subRegex.Match(x.Key).Success).ToDictionary(x => x.Key, x => x.Value);
                    var randomWord = dict.ElementAt(random.Next(dict.Count)).Key;
                    var leftIndex = wordToGuess.IndexOf("_");
                    input = randomWord[leftIndex].ToString();
                    Console.WriteLine("input: {0},", input);
                }

                

                ns.Write(Encoding.ASCII.GetBytes(input), 0, input.Length);
                ns.Flush();

                data = new byte[1024];
                recv = ns.Read(data, 0, data.Length);
                stringData = Encoding.ASCII.GetString(data, 0, recv);
                Console.WriteLine(stringData);
            }

            Console.WriteLine("Disconnecting from server...");
            ns.Close();
            server.Close();
        }
    }
}
