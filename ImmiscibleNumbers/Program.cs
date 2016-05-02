using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImmiscibleNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            string testCaseResult = string.Empty;
            string[] lines = File.ReadAllLines("submitInput.txt");
            for (int i = 1; i < lines.Length; i++)
            {
                UInt64 number = Convert.ToUInt32(lines[i]);
                testCaseResult = String.Format("Case #{0}: {1}", i, FindImmiscibleNumber(number));
                sb.AppendLine(testCaseResult);
                Console.WriteLine(testCaseResult);
                File.WriteAllText("submitImmiscibleNumbersResult.txt", sb.ToString());
            }
        }

        private static string GetNumberOfOnesZeroes(BigInteger immiscibleNumber)
        {
            string strImmiscibleNumber = immiscibleNumber.ToString();
            int numberOfOnes = Regex.Matches(strImmiscibleNumber, "1").Count;
            int numberOfZeroes = strImmiscibleNumber.Length - numberOfOnes;
            return numberOfOnes + " " + numberOfZeroes;
        }

        private static string FindImmiscibleNumber(UInt64 number)
        {
            if(number > 0)
            {
                int powerOfTwo = 0;
                int powerOfFive = 0;
                while (number % 2 == 0)
                {
                    number = number / 2;
                    powerOfTwo ++;
                }
                while (number % 5 == 0)
                {
                    number = number / 5;
                    powerOfFive ++;
                }
                
                int numberOfZeroes = Math.Max(powerOfTwo,powerOfFive);
                BigInteger immiscibleNumber = new BigInteger(Math.Pow(10, numberOfZeroes));
                UInt64 numberOfOnes = GetNumberOfOnes(number);
                return String.Format("{0} {1}", numberOfOnes, numberOfZeroes);
            }
            else
            {
                return String.Format("{0} {1}", 0, 1);
            }
        }


        private static UInt64 GetNumberOfOnes(UInt64 number)
        {
            UInt64 numberOfOnes = 0;
            UInt64 f = 0;

            do
            {
                f = f * 10 + 1;
                f = f % number;
                numberOfOnes++;
            } while (f != 0);

            //while (numberOfOnes % number != 0)
            //{
            //    numberOfOnes = numberOfOnes % number;
            //    numberOfOnes = ((numberOfOnes * 10) + 1);                
            //}
            return numberOfOnes;
        }
    }
}
