using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Contest
{
    public class Tape
    {
        public Tape(string label, string content)
        {
            this.Label = label;
            this.Content = content;
        }
        public string Label { get; set; }
        public string Content { get; set; }
    }
    public class Directive
    {
        public Directive(char label)
        {
            this.DirectiveLabel = label;
        }
        public char DirectiveLabel { get; set; }
        public string Write { get; set; }
        public string Move { get; set; }
        public string NextStateLabel { get; set; }
    }
    public class State
    {
        public State(string label)
        {
            this.Label = label;
            directives = new List<Directive>();
        }
        public string Label { get; set; }
        public List<Directive> directives { get; set; }
    }
    class Program
    {
        public static List<Tape> GetTapes(string[] codes)
        {
            List<Tape> lstTape = new List<Tape>();
            var startLineOfTape = Array.IndexOf(codes, codes.FirstOrDefault(x => x == "tapes:")) + 1;
            for (int i = startLineOfTape; i < codes.Length - 1; i++)
            {
                string labelContent = codes[i].Trim();
                string label = labelContent.Substring(0, labelContent.IndexOf(":"));
                string content = labelContent.Substring(labelContent.IndexOf("'") + 1, labelContent.LastIndexOf("'") - labelContent.IndexOf("'") - 1);
                lstTape.Add(new Tape(label, content));
            }
            return lstTape;
        }
        public static List<State> ParseCode(string[] codes)
        {
            List<State> lstState = new List<State>();
            try
            {
                for (int i = 2; i < codes.Length && !codes[i].Contains("tapes:"); i++)
                {
                    if (codes[i].Contains("      "))
                    {
                        if (codes[i].Contains("write"))
                        {
                            lstState.Last().directives.Last().Write = codes[i].Substring(14,1);
                        }
                        else if (codes[i].Contains("move"))
                        {
                            lstState.Last().directives.Last().Move = codes[i].Substring(12);
                        }
                        else if (codes[i].Contains("state"))
                        {
                            lstState.Last().directives.Last().NextStateLabel = codes[i].Substring(13);
                        }
                    }
                    else if (codes[i].Contains("    "))
                    {
                        lstState.Last().directives.Add(new Directive(codes[i].Trim()[1]));
                    }
                    else if (codes[i].Contains("  "))
                    {
                        string label = codes[i].Trim().Replace(":", "");
                        lstState.Add(new State(label));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(lstState.Count);
                Console.WriteLine(ex.ToString());
            }
            return lstState;
        }

        public static void Interpreter(List<State> lstState, List<Tape> lstTape)
        {
            StringBuilder result = new StringBuilder();
            foreach (var tape in lstTape)
            {
                StringBuilder sb = new StringBuilder();
                var content = tape.Content;
                sb.Append(content);
                var netxStateLabel = "start";
                State currentState;
                int i = 0;
                while (netxStateLabel != "end")
                {
                    currentState = lstState.FirstOrDefault(x => x.Label == netxStateLabel);
                    Directive currentDirective;
                    if (i < 0 || i >= sb.Length)
                    {
                        currentDirective = currentState.directives.FirstOrDefault(x => x.DirectiveLabel == ' ');
                    }
                    else
                    {
                        currentDirective = currentState.directives.FirstOrDefault(x => x.DirectiveLabel == sb[i]);
                    }

                    if (string.IsNullOrEmpty(currentDirective.Write))
                    {
                    }
                    else if (currentDirective.DirectiveLabel == ' ')
                    {
                        sb.Append(currentDirective.Write[0]);
                    }
                    else
                    {
                        sb[i] = currentDirective.Write[0];
                    }

                    if (string.IsNullOrEmpty(currentDirective.Move))
                    {
                    }
                    else
                    {
                        if (currentDirective.Move == "left")
                        {
                            i--;
                        }
                        if (currentDirective.Move == "right")
                        {
                            i++;
                        }
                    }

                    if (string.IsNullOrEmpty(currentDirective.NextStateLabel))
                    {
                    }
                    else
                    {
                        netxStateLabel = currentDirective.NextStateLabel;
                    }
                    
                }
                result.Append(String.Format("Tape #{0}: {1}\n", tape.Label, sb.ToString()));
            }
            File.WriteAllText("submitResult.txt", result.ToString().Trim());
            Console.WriteLine(result.ToString());
        }

        public static List<string> LoadCombos()
        {
            List<string> Combos = new List<string>();
            //Combos.Add("L-LD-D-RD-R-P");
            //Combos.Add("R-RD-D-LD-L-K");
            Combos.Add("D-RD-R-P");
            Combos.Add("R-D-RD-P");
            Combos.Add("D-LD-L-K");
            return Combos;
        }

        public static int GetNumberOfFailedCombos(List<string> Combos, string session)
        {
            int result = 0;
            List<string> MissedCombos = new List<string>();
            foreach (var combo in Combos)
            {
                int missedComboCount = 0;
                int matchedComboCount = 0;
                var missCombo = combo.Substring(0, combo.Length - 2);

                missedComboCount = Regex.Matches(session, missCombo).Count;
                matchedComboCount = Regex.Matches(session, combo).Count;

                result += (missedComboCount - matchedComboCount);
            }
            return result;
        }
        static void Main(string[] args)
        {
            try
            {
                string[] words = File.ReadAllLines("words.txt");
                Random random = new Random((int)DateTime.Now.Ticks);
                string wordToGuess = words[random.Next(0, words.Length)];
                Console.WriteLine("word to guess: {0}", wordToGuess);
                string wordToGuessUppercase = wordToGuess.ToUpper();
                StringBuilder displayToPlayer = new StringBuilder(wordToGuess.Length);
                for (int i = 0; i < wordToGuess.Length; i++)
                    displayToPlayer.Append('_');

                List<char> correctGuesses = new List<char>();
                List<char> incorrectGuesses = new List<char>();
                int lives = 5;
                bool won = false;
                int lettersRevealed = 0;

                string input;
                char guess;

                while (!won && lives > 0)
                {
                    Console.Write("Guess a letter: ");

                    input = Console.ReadLine().ToUpper();
                    guess = input[0];

                    if (correctGuesses.Contains(guess))
                    {
                        Console.WriteLine("You've already tried '{0}', and it was correct!", guess);
                        continue;
                    }
                    else if (incorrectGuesses.Contains(guess))
                    {
                        Console.WriteLine("You've already tried '{0}', and it was wrong!", guess);
                        continue;
                    }

                    if (wordToGuessUppercase.Contains(guess))
                    {
                        correctGuesses.Add(guess);

                        for (int i = 0; i < wordToGuess.Length; i++)
                        {
                            if (wordToGuessUppercase[i] == guess)
                            {
                                displayToPlayer[i] = wordToGuess[i];
                                lettersRevealed++;
                            }
                        }

                        if (lettersRevealed == wordToGuess.Length)
                            won = true;
                    }
                    else
                    {
                        incorrectGuesses.Add(guess);

                        Console.WriteLine("Nope, there's no '{0}' in it!", guess);
                        lives--;
                    }

                    Console.WriteLine(displayToPlayer.ToString());
                }

                if (won)
                    Console.WriteLine("You won!");
                else
                    Console.WriteLine("You lost! It was '{0}'", wordToGuess);

                Console.Write("Press ENTER to exit...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
