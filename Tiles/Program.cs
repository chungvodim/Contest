using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiles
{
    class Program
    {
       
        public static int ConvertLetterToNumber(char letter)
        {
            int result = 0;
            switch (letter)
            {
                case 'A': result = 1; break;
                case 'B': result = 2; break;
                case 'C': result = 3; break;
                case 'D': result = 4; break;
                case 'E': result = 5; break;
                case 'F': result = 6; break;
                case 'G': result = 7; break;
                case 'H': result = 8; break;
                case 'I': result = 9; break;
                case 'J': result = 10; break;
                case 'K': result = 11; break;
                case 'L': result = 12; break;
                case 'M': result = 13; break;
                case 'N': result = 14; break;
                case 'O': result = 15; break;
                case 'P': result = 16; break;
                case 'Q': result = 17; break;
                case 'R': result = 18; break;
                case 'S': result = 19; break;
                case 'T': result = 20; break;
                case 'U': result = 21; break;
                case 'V': result = 22; break;
                case 'W': result = 23; break;
                case 'X': result = 24; break;
                case 'Y': result = 25; break;
                case 'Z': result = 26; break;
                case '.': result = 0; break;
                case 'a': result = -1; break;
                case 'b': result = -2; break;
                case 'c': result = -3; break;
                case 'd': result = -4; break;
                case 'e': result = -5; break;
                case 'f': result = -6; break;
                case 'g': result = -7; break;
                case 'h': result = -8; break;
                case 'i': result = -9; break;
                case 'j': result = -10; break;
                case 'k': result = -11; break;
                case 'l': result = -12; break;
                case 'm': result = -13; break;
                case 'n': result = -14; break;
                case 'o': result = -15; break;
                case 'p': result = -16; break;
                case 'q': result = -17; break;
                case 'r': result = -18; break;
                case 's': result = -19; break;
                case 't': result = -20; break;
                case 'u': result = -21; break;
                case 'v': result = -22; break;
                case 'w': result = -23; break;
                case 'x': result = -24; break;
                case 'y': result = -25; break;
                case 'z': result = -26; break;
            }
            return result;
        }
        public static int FindBiggestSum(int[,] arr, int rows, int cols)
        {
            int rResult = 0;
            int cResult = 0;
            int sum = 0;
            //int tempStart = 0;
            //int startIndex = 0;
            //int endIndex = 0;

            int[,] sumArr = new int[rows * 3, cols * 3];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols * 3; j++)
                {
                    sumArr[i, j] = arr[i % rows, j % cols];
                }
            }

            List<int> tempList = new List<int>();
            for (int i = 0; i < rows * 3; i++)
            {
                for (int j = 0; j < cols * 3; j++)
                {
                    tempList.Add(sumArr[i,j]);
                }
            }

            for (int i = 0; i < tempList.Count; i++)
            {
                sum += tempList[i];
                if (sum > rResult)
                {
                    rResult = sum;
                    //startIndex = tempStart;
                    //endIndex = i;
                }
                if (sum < 0)
                {
                    sum = 0;
                    //tempStart = i + 1;
                }
            }

            tempList.Clear();

            for (int i = 0; i < rows * 3; i++)
            {
                for (int j = 0; j < cols * 3; j++)
                {
                    tempList.Add(sumArr[j,i]);
                }
            }

            for (int i = 0; i < tempList.Count; i++)
            {
                sum += tempList[i];
                if (sum > cResult)
                {
                    cResult = sum;
                }
                if (sum < 0)
                {
                    sum = 0;
                }
            }

            return Math.Max(rResult,cResult);
        }


        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("sampleInput.txt");
            int t = Convert.ToInt32(lines[0]);
            for (int i = 1; i < lines.Length; i++)
            {
                int n = Convert.ToInt32(lines[i].Split(' ')[0]);
                if (n > 0)
                {
                    int m = Convert.ToInt32(lines[i].Split(' ')[1]);
                    int[,] arr = new int[n,m];
                    for (int j = 0; j < n; j++)
                    {
                        int[] line = lines[i + j + 1].Select(x => ConvertLetterToNumber(x)).ToArray();
                        for (int k = 0; k < m; k++)
                        {
                            arr[j, k] = line[k];
                        }
                    }
                    i += n;
                    Console.WriteLine(FindBiggestSum(arr,n,m));
                }
            }
        }
    }
}
