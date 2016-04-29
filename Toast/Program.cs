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
            string[] lines = File.ReadAllLines("testInput.txt");
            for (int i = 1; i < lines.Length; i++)
            {
                uint[] arr = lines[i].Split(' ').Select(x => Convert.ToUInt32(x)).ToArray();
                testCaseResult = String.Format("Case #{0}: {1}", i, GetSeconds(arr[0], arr[1], arr[2]));
                sb.AppendLine(testCaseResult);
                Console.WriteLine(testCaseResult);
                File.WriteAllText("testResult.txt", sb.ToString());
            }
        }

        private static string GetSeconds(uint n, uint m, uint k)
        {
            if (k < n * m) return "IMPOSSIBLE";
            if (k == n * m) return "0";
            if(k % m != 0) return "IMPOSSIBLE";
            k = k / m;
            Stack<Dictionary<uint, uint>> stateStack = new Stack<Dictionary<uint, uint>>();
            Dictionary<uint, uint> stateDict = new Dictionary<uint, uint>();
            stateDict.Add(0,n);
            stateStack.Push(stateDict);
            uint seconds = GetSeconds(stateStack,k,0);
            return seconds.ToString();
        }

        private static uint GetSeconds(Stack<Dictionary<uint, uint>> stateStack, uint k, uint seconds)
        {
            Dictionary<uint, uint> lastState = stateStack.Pop();
            if(CheckSum(lastState) == k)
            {
                return seconds;
            }
            else
            {
                ChangeState(stateStack);
                return GetSeconds(stateStack , k, seconds);
            }
        }

        private static void ChangeState(Stack<Dictionary<uint, uint>> stateStack)
        {
            throw new NotImplementedException();
        }

        private static uint CheckSum(Dictionary<uint, uint> stateDict)
        {
            uint sum = 0;
            foreach (var item in stateDict)
            {
                sum += (uint)Math.Pow(2, item.Key) * item.Value;
            }
            return sum;
        }
    }
}
