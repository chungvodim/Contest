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
        public static bool UpdateMax(int[] arr, ref int currentMax, ref int currentUp, ref int currentDown)
        {
            int max = 0;
            int currentSum = 0;
            int up = 0;
            int down = 0;
            int tempIndex = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                currentSum += arr[i];
                if (currentSum > max)
                {
                    max = currentSum;
                    up = tempIndex;
                    down = i;
                }
                if (currentSum < 0)
                {
                    currentSum = 0;
                    tempIndex = i + 1;
                }
            }
            if(max > currentMax)
            {
                currentUp = up;
                currentDown = down;
                currentMax = max;
                return true;
            }
            return false;
        }
        private static bool CheckInfinity(int[,] sumArr, int left, int right, int up, int down)
        {
            int sum = 0;
            int rows = sumArr.GetLength(0);
            int cols = sumArr.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = left; j <= right; j++)
                {
                    sum += sumArr[i, j];
                }
            }
            if (sum > 0)
            {
                return true;
            }
            sum = 0;
            for (int i = 0; i < cols; i++)
            {
                for (int j = up; j <= down; j++)
                {
                    sum += sumArr[j, i];
                }
            }
            if (sum > 0)
            {
                return true;
            }
            return false;
        }
        public static string FindBiggestSum(int[,] arr, int rows, int cols)
        {
            int max = 0;
            int left = 0;
            int right = 0;
            int up = 0;
            int down = 0;
            int[] column;
            bool isInfinity = false;

            int[,] sumArr = new int[rows * 2, cols * 2];
            for (int i = 0; i < rows * 2; i++)
            {
                for (int j = 0; j < cols * 2; j++)
                {
                    sumArr[i, j] = arr[i % rows, j % cols];
                }
            }
            for (int l = 0; l < cols * 2; l++)
            {
                column = new int[rows * 2];
                for (int r = l; r < cols * 2; r++)
                {
                    for (int j = 0; j < rows * 2; j++)
                    {
                        column[j] += sumArr[j, r];
                    }
                    
                    bool isNewMax = UpdateMax(column, ref max, ref up, ref down);
                    if (isNewMax)
                    {
                        left = l;
                        right = r;
                    }
                }
            }
            isInfinity = CheckInfinity(sumArr, left, right, up, down);
            if (isInfinity)
            {
                return "INFINITY";
            }
            return max.ToString();
        }

        

        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            string result = string.Empty;
            string[] lines = File.ReadAllLines("submitInput.txt");
            //int t = Convert.ToInt32(lines[0]);
            int t = 1;
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
                    result = String.Format("Case #{0}: {1}", t, FindBiggestSum(arr, n, m));
                    Console.WriteLine(result);
                    sb.AppendLine(result);
                    t++;
                }
            }
            File.WriteAllText("submitTilesResult.txt",sb.ToString());
        }
    }
}
