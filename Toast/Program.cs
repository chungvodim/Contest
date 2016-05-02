using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toast
{
    class Program
    {
        public class State
        {

        }
        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            string testCaseResult = string.Empty;
            string[] lines = File.ReadAllLines("submitInput.txt");
            for (int i = 1; i < lines.Length; i++)
            {
                ulong[] arr = lines[i].Split(' ').Select(x => Convert.ToUInt64(x)).ToArray();
                testCaseResult = String.Format("Case #{0}: {1}", i, GetSeconds(arr[0], arr[1], arr[2]));
                sb.AppendLine(testCaseResult);
                Console.WriteLine(testCaseResult);
                File.WriteAllText("submitResult.txt", sb.ToString());
            }
        }

        private static string GetSeconds(ulong n, ulong m, ulong k)
        {
            if (k == n * m) return "0";
            if(k < n * m) return "IMPOSSIBLE";
            if (k % m != 0) return "IMPOSSIBLE";
            k = k / m;
            k = k - n + 1;
            int count = 0;
            while( k != 0)
            {
                if(k % 2 == 1)
                {
                    k = k - 1;
                }
                else
                {
                    k = k / 2;
                }
                count++;
            }
            return (count-1).ToString();
        }

    }
}
