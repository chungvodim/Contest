using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PikaVirus
{
    public class Node
    {
        public Node(string city, int level, int numberOfChildren, string ancestor)
        {
            this.City = city;
            this.Level = level;
            this.NumberOfChildren = numberOfChildren;
            this.Ancestor = ancestor;
        }
        public string City { get; set; }
        public int Level { get; set; }
        public int NumberOfChildren { get; set; }
        public string Ancestor { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            string testCaseResult = string.Empty;
            string[] lines = File.ReadAllLines("submitInput.txt");
            uint numberOfCities = Convert.ToUInt32(lines[0]);
            string[] srcDest;
            string srcCity;
            string destCity;
            List<Node> nodeList = new List<Node>();
            for (int i = 1; i <= numberOfCities - 1; i++)
            {
                srcDest = lines[i].Split(' ');
                srcCity = srcDest[0];
                destCity = srcDest[1];

                BuildNode(nodeList, srcCity, destCity);
            }
            //Draw(nodeList);
            List<Node> comparedNodeList = new List<Node>();
            for (uint i = numberOfCities + 1; i < lines.Length; i++)
            {
                srcDest = lines[i].Split(' ');
                srcCity = srcDest[0];
                destCity = srcDest[1];
                BuildNode(comparedNodeList, srcCity, destCity);
                if ((i - numberOfCities) % (numberOfCities - 1) == 0)
                {
                    //Draw(comparedNodeList);
                    testCaseResult = CompareViruses(nodeList, comparedNodeList);
                    sb.AppendLine(String.Format("Case #{0}:{1}", (i - numberOfCities) / (numberOfCities - 1), testCaseResult));
                    //Console.WriteLine(String.Format("Case #{0}:{1}", (i - numberOfCities) / (numberOfCities - 1), testCaseResult));
                    comparedNodeList.Clear();
                }
            }
            File.WriteAllText("submitResult.txt", sb.ToString());
        }

        private static void Draw(List<Node> nodeList)
        {
            var temp = nodeList.OrderBy(x => x.Level).ToList();
            int maxLevel = temp.Last().Level;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= maxLevel; i++)
            {
                var level = nodeList.Where(x => x.Level == i).OrderBy(x => x.City).ToList();
                foreach (var item in level)
                {
                    sb.Append(item.City + " ");
                }
                sb.Append("\n");
            }
            File.WriteAllText("Nodes.txt",sb.ToString());
        }

        private static string CompareViruses(List<Node> nodeList, List<Node> comparedNodeList)
        {
            if (nodeList.Count != comparedNodeList.Count)
            {
                return " NO";
            }
            else
            {
                nodeList = nodeList.OrderBy(x => x.City).ToList();
                comparedNodeList = comparedNodeList.OrderBy(x => x.City).ToList();
                StringBuilder sb = new StringBuilder();
                List<string> comparedCityList = new List<string>();
                for (int i = 0; i < nodeList.Count; i++)
                {
                    //if(nodeList[i].City == "aroco")
                    //{
                    //    Console.WriteLine("Debug");
                    //}
                    var comparedCitys = comparedNodeList.Where(x => x.Level == nodeList[i].Level 
                    && x.NumberOfChildren == nodeList[i].NumberOfChildren 
                    && IsSameSpreadingPath(nodeList[i], nodeList, x, comparedNodeList) 
                    && !comparedCityList.Contains(x.City)).ToList();
                    if (comparedCitys == null || comparedCitys.Count == 0)
                    {
                        return " NO";
                    }
                    else
                    {
                        comparedCityList.Add(comparedCitys.First().City);
                        sb.Append(String.Format(" {0}/{1}",nodeList[i].City, comparedCitys.First().City));
                        //Console.WriteLine(String.Format(" {0}/{1}", nodeList[i].City, comparedCitys.First().City));
                    }
                }
                return sb.ToString();
            }
        }

        private static bool IsSameSpreadingPath(Node ancestorNode, List<Node> nodeList, Node comparedAncestorNode, List<Node> comparedNodeList)
        {
            //if (comparedAncestorNode.City == "alcanices")
            //{
            //    Console.WriteLine("Debug");
            //}
            var ancestor = ancestorNode.City;
            var comparedAncestor = comparedAncestorNode.City;
            nodeList = nodeList.OrderBy(x => x.City).ToList();
            comparedNodeList = comparedNodeList.OrderBy(x => x.City).ToList();
            if (nodeList.Where(x => x.Ancestor == ancestor).ToList().Count != comparedNodeList.Where(x => x.Ancestor == comparedAncestor).ToList().Count)
            {
                return false;
            }
            else
            {
                List<string> comparedCityList = new List<string>();
                foreach (var childNode in nodeList.Where(x => x.Ancestor == ancestor).ToList())
                {
                    var comparedAncestorNodes = comparedNodeList.Where(x => x.NumberOfChildren == childNode.NumberOfChildren 
                    && x.Ancestor == comparedAncestor
                    && !comparedCityList.Contains(x.City)).ToList();
                    if(comparedAncestorNodes == null || comparedAncestorNodes.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i < comparedAncestorNodes.Count; i++)
                        {
                            //if(comparedAncestorNodes[i].City == "albaina")
                            //{
                            //    Console.WriteLine("Debug");
                            //}
                            if (IsSameSpreadingPath(childNode, nodeList, comparedAncestorNodes[i], comparedNodeList))
                            {
                                comparedAncestorNode = comparedAncestorNodes[i];
                                comparedCityList.Add(comparedAncestorNode.City);
                                break;
                            }
                            else if(i == comparedAncestorNodes.Count - 1)
                            {
                                //Console.WriteLine("{0}/{1}-{2}/{3}-{4}/{5}", childNode.City, comparedAncestorNodes[i].City
                                //    , childNode.Ancestor, comparedAncestorNodes[i].Ancestor, childNode.Level, comparedAncestorNodes[i].Level);
                                return false;
                            } 
                        }
                        IsSameSpreadingPath(childNode, nodeList, comparedAncestorNode, comparedNodeList);
                    }
                }
            }
            return true;
        }

        private static void BuildNode(List<Node> nodeList, string srcCity, string destCity)
        {
            if(nodeList.Count == 0)
            {
                nodeList.Add(new Node(srcCity,0,1,string.Empty));
                nodeList.Add(new Node(destCity, 1, 0, srcCity));
            }
            else
            {
                Node node = nodeList.FirstOrDefault(x => x.City == srcCity);
                if (node != null)
                {
                    node.NumberOfChildren++;
                    int level = node.Level + 1;
                    nodeList.Add(new Node(destCity, level, 0, srcCity));
                }
            }
        }
    }
}
