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
            //CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fo"); //

            Console.WriteLine("Loading XBRL...");

            var xbrlInstance = new XbrlDocument();
            xbrlInstance.Load(Environment.CurrentDirectory + @"\20191101\2065_1_2019.xml");

            //// RTH - Layers testing
            //var listToConvert = XBRLservices.GepsioDataExtractor.GetAllValuesFromFactsList(xbrlInstance);
            //XBRLservices.JSONServices.JsonWriteToFileFromList(listToConvert);

            XBRLservices.GepsioDataPrinter.ShowUnitsInDocument(xbrlInstance);
            XBRLservices.GepsioDataPrinter.ShowContextsInDocument(xbrlInstance);
            XBRLservices.GepsioDataPrinter.ShowFactsInDocument(xbrlInstance);
            XBRLservices.GepsioDataPrinter.PrintLabelsForFacts(xbrlInstance);

            WalkPresentableTree(xbrlInstance);

            Console.WriteLine("Loading complete...");
        }

        public static void WalkPresentableTree(XbrlDocument xbrlDocument)
        {
            //var xbrlDoc = new XbrlDocument();
            //xbrlDoc.Load(@"C:\Users\RTH\source\repos\XBRLmvc\RTHtestConsole\20191101\2065_1_2019.xml");

            var firstFragment = xbrlDocument.XbrlFragments[0];
            var tree = firstFragment.GetPresentableFactTree();
            foreach (var currentNode in tree.TopLevelNodes)
                WalkPresentableTreeNode(currentNode, 0);
        }

        private static void WalkPresentableTreeNode(PresentableFactTreeNode node, int depth)
        {

            // Indent as necessary, according to depth, so that parent child relationships
            // are shown.
            for (var indent = 0; indent < depth; indent++)
                Console.WriteLine("    ");
                Debug.Write("    ");

            // If the tree node has a fact, then display it.
            if (node.NodeFact != null)
            {

                // Display item details, if this fact is actually an item.
                // Tuples are not supported in this code sample.
                if (node.NodeFact is Item)
                {
                    var nodeItem = node.NodeFact as Item;
                    Console.WriteLine(nodeItem.Name);
                    Console.WriteLine(" ");
                    Console.WriteLine(nodeItem.Value);

                    Debug.Write(nodeItem.Name);
                    Debug.Write(" ");
                    Debug.Write(nodeItem.Value);
                }
            }
            else
            {

                // If there is no fact for this node, then display the label.
                Debug.Write(node.NodeLabel);
                Console.WriteLine(node.NodeLabel);
            }
            Debug.WriteLine("");
            Console.WriteLine("");

            // Recursively call into this same method for each of the node's
            // child nodes, increasing the depth by one so that the indenting
            // reflects the child nodes.
            foreach (var childNode in node.ChildNodes)
                WalkPresentableTreeNode(childNode, depth + 1);
        }
    }
}