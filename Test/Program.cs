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

        public static Dictionary<int, string> LoadVowelsDict()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            dict.Add(1, "AI");
            dict.Add(2, "AOEIUMBH");
            dict.Add(3, "AEOIUYHBCK");
            dict.Add(4, "AEOIUYSBF");
            dict.Add(5, "SEAOIUYH");
            dict.Add(6, "EAIOUSY");
            dict.Add(7, "EIAOUS");
            dict.Add(8, "EIAOU");
            dict.Add(9, "EIAOU");
            dict.Add(10, "EIOAU");
            dict.Add(11, "EIOAD");
            dict.Add(12, "EIOAF");
            dict.Add(13, "IEOA");
            dict.Add(14, "IEO");
            dict.Add(15, "IEA");
            dict.Add(16, "IEH");
            dict.Add(17, "IER");
            dict.Add(18, "IEA");
            dict.Add(19, "IEA");
            dict.Add(20, "IE");
            return dict;
        }

        public static string FindMostLikelyLetter(Dictionary<string, int> dict, string guessedLetters, string letters)
        {
            string result = string.Empty;
            Dictionary<string, int> mostLikelyLettersDict = new Dictionary<string, int>();
            StringBuilder sb = new StringBuilder();
            foreach (var item in guessedLetters)
            {
                letters = letters.Replace(item.ToString(),"");
            }
            foreach (var item in letters)
            {
                mostLikelyLettersDict.Add(item.ToString(), dict.Where(x => x.Key.Contains(item)).Count());
            }
            return mostLikelyLettersDict.OrderByDescending(x => x.Value).First().Key;
        }

        static void Main(string[] args)
        {
            var originDict = LoadDictionary("words.txt");
            var strategyDict = LoadVowelsDict();
            Dictionary<string, int> dict = new Dictionary<string, int>();
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            
            byte[] data = new byte[1024];
            string input = "";
            string stringData = "";
            string wordToGuess = "";
            int wordToGuessLength = 0;
            bool isFilteredByLength = false;
            int numberOfGuess = 0;
            string StrategyChars = string.Empty;
            string guessedletters = string.Empty;
            string matchedLetters = string.Empty;
            string missedLetters = string.Empty;
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
                    dict = originDict;
                    isFilteredByLength = false;
                    StrategyChars = string.Empty;
                    guessedletters = string.Empty;
                    matchedLetters = string.Empty;
                    missedLetters = string.Empty;
                    numberOfGuess = 0;
                }
                else if (stringData.Contains("OVER"))
                {
                    Console.WriteLine("GAME OVER WITH DICT LENGTH : {0}", dict.Count);
                    Console.Read();
                }
                else
                {
                    var match = regex.Match(stringData);
                    if (match.Success)
                    {
                        wordToGuess = match.Groups[1].Value.Replace(" ", "");
                        wordToGuessLength = wordToGuess.Length;
                        if (string.IsNullOrEmpty(StrategyChars))
                        {
                            StrategyChars = strategyDict[wordToGuess.Length];
                        }
                        // Filter length
                        if (!isFilteredByLength)
                        {
                            dict = dict.Where(x => x.Value == wordToGuessLength).ToDictionary(x => x.Key, x => x.Value);
                            isFilteredByLength = true;
                        }
                        // Filter By matched letters
                        matchedLetters = wordToGuess.Replace("_", "");
                        missedLetters = guessedletters;
                        foreach (var item in matchedLetters)
                        {
                            missedLetters = missedLetters.Replace(item.ToString(), "");
                        }

                        var subRegex = new Regex(wordToGuess.Replace("_", "\\w"));
                        dict = dict.Where(x => subRegex.Match(x.Key).Success).ToDictionary(x => x.Key, x => x.Value);
                        foreach (var item in missedLetters)
                        {
                            dict = dict.Where(x => !x.Key.Contains(item.ToString())).ToDictionary(x => x.Key, x => x.Value);
                        }

                        Console.WriteLine("word To Guess: {0}", wordToGuess);
                        Console.WriteLine("guessed letters: {0}", guessedletters);
                        Console.WriteLine("matched letters: {0}", matchedLetters);
                        Console.WriteLine("missed letters: {0}", missedLetters);
                        Console.WriteLine("dict length: {0}", dict.Count);

                        if (wordToGuess.Replace("_", "").Length >= 1 || numberOfGuess >= StrategyChars.Length)// found at least one letter
                        {
                            if (dict != null && dict.Count > 0)
                            {
                                input = FindMostLikelyLetter(dict, matchedLetters, letters);
                                Console.WriteLine("Find Most Likely Letter: {0}", input);
                                //var randomWord = dict.ElementAt(random.Next(dict.Count)).Key;
                                //var leftIndex = wordToGuess.IndexOf("_");
                                //input = randomWord[leftIndex].ToString();
                                guessedletters += input;
                            }
                        }
                        else
                        {
                            input = StrategyChars[numberOfGuess].ToString();
                            guessedletters += input;
                            numberOfGuess++;
                        }

                        Console.WriteLine("input: {0},", input);
                    }
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
