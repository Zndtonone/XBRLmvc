using System;
using JeffFerguson.Gepsio;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;
using System.IO;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fo");

            //var xbrlInstance = new XbrlDocument();
            //xbrlInstance.Load(Environment.CurrentDirectory + @"\20191101\2065_1_2019.xml");

            //// RTH - Layers testing
            //var listToConvert = XBRLservices.GepsioDataExtractor.GetAllValuesFromFactsList(xbrlInstance);
            //XBRLservices.JSONServices.JsonWriteToFileFromList(listToConvert);

            //XBRLservices.GepsioDataPrinter.ShowUnitsInDocument(xbrlInstance);
            //XBRLservices.GepsioDataPrinter.ShowContextsInDocument(xbrlInstance);
            //XBRLservices.GepsioDataPrinter.ShowFactsInDocument(xbrlInstance);
            //XBRLservices.GepsioDataPrinter.PrintLabelsForFacts(xbrlInstance);

            string path = @"C:/Users/RTH/ASP.NET repo/XBRLviewerMVC/wwwroot/data/20191101/2065_1_2019.xml";

            int index = path.LastIndexOf('/');
            string jsonFilePath = index == -1 ? path : path.Substring(0, index);
        }

        public static void WalkPresentableTree(XbrlDocument xbrlDoc)
        {
            var firstFragment = xbrlDoc.XbrlFragments[0];
            var tree = firstFragment.GetPresentableFactTree();
            foreach (var currentNode in tree.TopLevelNodes)
                WalkPresentableTreeNode(currentNode, 0);
        }

        private static void WalkPresentableTreeNode(PresentableFactTreeNode node, int depth)
        {

            // Indent as necessary, according to depth, so that parent child relationships
            // are shown.
            for (var indent = 0; indent < depth; indent++)
                Debug.Write("    ");

            // If the tree node has a fact, then display it.
            if (node.NodeFact != null)
            {

                // Display item details, if this fact is actually an item.
                // Tuples are not supported in this code sample.
                if (node.NodeFact is Item)
                {
                    var nodeItem = node.NodeFact as Item;
                    Debug.Write(nodeItem.Name);
                    Debug.Write(" ");
                    Debug.Write(nodeItem.Value);
                }
            }
            else
            {

                // If there is no fact for this node, then display the label.
                Debug.Write(node.NodeLabel);
            }
            Debug.WriteLine("");

            // Recursively call into this same method for each of the node's
            // child nodes, increasing the depth by one so that the indenting
            // reflects the child nodes.
            foreach (var childNode in node.ChildNodes)
                WalkPresentableTreeNode(childNode, depth + 1);
        }

        public static void PresentableFacts(XbrlDocument xbrlDoc)
        {
            foreach (var fragment in xbrlDoc.XbrlFragments)
            {
                PresentableFactTree pft = fragment.GetPresentableFactTree();

                foreach (var pfn in pft.TopLevelNodes)
                {
                    Console.WriteLine(pfn.NodeFact.ToString());
                    Console.WriteLine(pfn.NodeFact.Name.ToString());
                    Console.WriteLine(pfn.NodeLabel);
                }
            }
        }
    }
}