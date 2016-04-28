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
            string[] lines = File.ReadAllLines("submitInput94.txt");
            for (uint i = 1; i < lines.Length; i++)
            {
                uint number = Convert.ToUInt32(lines[i]);
                BigInteger immiscibleNumber = FindImmiscibleNumber(number);
                testCaseResult = String.Format("Case #{0}: {1}", i, GetNumberOfOnesZeroes(immiscibleNumber));
                sb.AppendLine(testCaseResult);
                Console.WriteLine(testCaseResult);
                File.WriteAllText("submitImmiscibleNumbersResult94.txt", sb.ToString());
            }
        }

        private static string GetNumberOfOnesZeroes(BigInteger immiscibleNumber)
        {
            string strImmiscibleNumber = immiscibleNumber.ToString();
            int numberOfOnes = Regex.Matches(strImmiscibleNumber, "1").Count;
            int numberOfZeroes = strImmiscibleNumber.Length - numberOfOnes;
            return numberOfOnes + " " + numberOfZeroes;
        }

        private static BigInteger FindImmiscibleNumber(uint number)
        {
            if(number > 0)
            {
                string binary = Convert.ToString(number, 2);
                int numberOfZeroes = binary.Length - binary.LastIndexOf('1');
                BigInteger immiscibleNumber = new BigInteger(Math.Pow(10, numberOfZeroes + 20));
                bool isNotFound = true;
                bool isNotMin = true;
                while (isNotFound)
                {
                    //Console.WriteLine(immiscibleNumber);
                    if (immiscibleNumber % number == 0)
                    {
                        while (isNotMin)
                        {
                            BigInteger prevImmiscibleNumber = immiscibleNumber;
                            immiscibleNumber = immiscibleNumber / 10;
                            if (immiscibleNumber % number != 0 || immiscibleNumber == 0)
                            {
                                immiscibleNumber = prevImmiscibleNumber;
                                isNotMin = false;
                            }
                        }
                        isNotFound = false;
                    }
                    else
                    {
                        immiscibleNumber = GetNextImmiscibleNumber(immiscibleNumber);
                    }
                }
                return immiscibleNumber;
            }
            else
            {
                return 0;
            }
        }

        private static BigInteger GetNextImmiscibleNumber(BigInteger immiscibleNumber)
        {
            BigInteger nextImmiscibleNumber = 0;
            StringBuilder sbImmiscibleNumber = new StringBuilder(immiscibleNumber.ToString());
            int numberOfDitgits = sbImmiscibleNumber.Length;
            int indexOfLastOne = sbImmiscibleNumber.ToString().LastIndexOf('1');
            //Console.WriteLine("index Of LastOne : {0}", indexOfLastOne);
            Console.WriteLine("leng of number : {0}", numberOfDitgits);
            nextImmiscibleNumber = BigInteger.Parse(sbImmiscibleNumber.Insert(0, "1").ToString());
            //if (indexOfLastOne == numberOfDitgits - 1)
            //{
            //    //nextImmiscibleNumber = Convert.ToUInt64(sbImmiscibleNumber.Replace("1", "0").Insert(0, "1").ToString());
            //    nextImmiscibleNumber = BigInteger.Parse(sbImmiscibleNumber.Replace("1", "0").Insert(0, "1").ToString());
            //}
            //else
            //{
            //    sbImmiscibleNumber[indexOfLastOne + 1] = '1';
            //    nextImmiscibleNumber = BigInteger.Parse(sbImmiscibleNumber.ToString());
            //}
            return nextImmiscibleNumber;
        }
    }
}
