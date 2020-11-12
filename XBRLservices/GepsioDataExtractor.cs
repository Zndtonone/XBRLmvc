using JeffFerguson.Gepsio;
using System;
using System.Collections.Generic;
using System.IO;

namespace XBRLservices
{
    /// <summary>
    /// This class extracts data from Gepsio. So it does not print data, but returns it.
    /// </summary>
    public class GepsioDataExtractor
    {
        [STAThread]
        static void Main()
        {
        }

        //==================================================== PUBLIC METHODS =================================================================

        // GEPSIO - Returns a list of anonymous types containing id, label, value, namespace, contextID
        public static List<object> GetValuesFromFactsToList(XbrlDocument xbrlInstance)
        {
            List<object> factList = new List<object>();

            foreach (var currentFragment in xbrlInstance.XbrlFragments)
            {
                foreach (var currentFact in currentFragment.Facts)
                {
                    string[] arr = GetFact(currentFact); // Returns a string array of Item.Value, Item.Namespace, Item.ContextIDName
                    
                    var matchingLabel = FindLabelForFact(currentFact, currentFragment);

                    if (string.IsNullOrEmpty(matchingLabel) == false)
                    {
                        var f = new {
                            Id = currentFact.Name,
                            Label = matchingLabel,
                            Value = arr[0],
                            Namespace = arr[1],
                            ContextID = arr[2]
                        };
                        factList.Add(f);
                    }
                }
            }

            return factList;
        }

        // GEPSIO - Returns facts in document (type, namespace, value, context)
        public static void ShowFactsInDocument(XbrlDocument xbrlInstance)
        {
            foreach (var currentFragment in xbrlInstance.XbrlFragments)
            {
                ShowFactsInFragment(currentFragment);
            }
        }

        // GEPSIO - Returns contexts in document
        public static void ShowContextsInDocument(XbrlDocument doc)
        {
            foreach (var currentFragment in doc.XbrlFragments)
            {
                ShowContextsInFragment(currentFragment);
            }
        }

        // GEPSIO - Returns units in document
        public static void ShowUnitsInDocument(XbrlDocument doc)
        {
            foreach (var currentFragment in doc.XbrlFragments)
            {
                ShowUnitsInFragment(currentFragment);
            }
        }

        //==================================================== PRIVATE METHODS ================================================================

        // RTH - Get value from fact
        private static string[] GetFact(Fact fact)
        {
            if (fact is Item)
            {
                string[] arr = GetItem(fact as Item);
                return arr;
            }

            return null;
        }

        // RTH - Show item
        private static string[] GetItem(Item item)
        {
            string[] arr;

            arr = new string[3]
            {
                item.Value,
                item.Namespace,
                item.ContextRefName
            };

            return arr;

            //if (item.ContextRef != null)
            //    ShowContext(item.ContextRef);
        }

        // GEPSIO - Find label for fact
        private static string FindLabelForFact(Fact currentFact, XbrlFragment xbrlFragment)
        {
            foreach (var currentSchema in xbrlFragment.Schemas)
            {
                if (currentSchema.LabelLinkbase != null)
                {
                    var labelId = FindLabelIdForFact(currentFact, currentSchema.LabelLinkbase);

                    if (string.IsNullOrEmpty(labelId) == false)
                    {
                        return FindLabelForLabelId(labelId, currentSchema.LabelLinkbase);
                    }
                }
            }
            return string.Empty;
        }

        // GEPIO - Find label ID for fact
        private static string FindLabelIdForFact(Fact currentFact, LabelLinkbaseDocument labelLinkbase)
        {
            var factAsItem = currentFact as Item;
            if (factAsItem == null)
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(factAsItem.SchemaElement.Id) == true)
            {
                return string.Empty;
            }

            foreach (var currentLink in labelLinkbase.LabelLinks)
            {
                string strMatchingArcId = factAsItem.SchemaElement.Id.Substring(factAsItem.SchemaElement.Id.IndexOf('_') + 1);

                var matchingArc = currentLink.GetLabelArc(strMatchingArcId);

                // var matchingArc = currentLink.GetLabelArc(factAsItem.SchemaElement.Id);

                if (matchingArc != null)
                {
                    return matchingArc.ToId;
                }
            }
            return string.Empty;
        }

        // GEPSIO - Find label for label id
        private static string FindLabelForLabelId(string labelId, LabelLinkbaseDocument labelLinkbase)
        {
            foreach (var currentLabelLink in labelLinkbase.LabelLinks)
            {
                foreach (var currentLabel in currentLabelLink.Labels)
                {
                    if (currentLabel.Label.Equals(labelId) == true)
                    {
                        return currentLabel.Text;
                    }
                }
            }
            return string.Empty;
        }

        // GEPSIO - Show facts in fragment
        private static void ShowFactsInFragment(XbrlFragment currentFragment)
        {
            foreach (var currentFact in currentFragment.Facts)
            {
                ShowFact(currentFact);
            }
        }

        // GEPSIO - Show fact
        private static void ShowFact(Fact fact)
        {
            Console.WriteLine($"FACT {fact.Name}");

            if (fact is Item)
            {
                ShowItem(fact as Item);
            }
        }

        // GEPSIO - Show item
        private static void ShowItem(Item item)
        {
            Console.WriteLine("\tType      : Item");
            Console.WriteLine($"\tNamespace : {item.Namespace}");
            Console.WriteLine($"\tValue     : {item.Value}");
            Console.WriteLine($"\tContext ID: {item.ContextRefName}");

            if (item.ContextRef != null)
                ShowContext(item.ContextRef);
        }

        // GEPSIO - Show contexts in fragment
        private static void ShowContextsInFragment(XbrlFragment currentFragment)
        {
            foreach (var currentContext in currentFragment.Contexts)
            {
                ShowContext(currentContext);
            }
        }

        // GEPSIO - Show context
        private static void ShowContext(Context currentContext)
        {
            Console.WriteLine("CONTEXT");
            Console.WriteLine($"\tID          : {currentContext.Id}");
            Console.Write($"\tPeriod Type : ");
            if (currentContext.InstantPeriod)
            {
                Console.WriteLine("instant");
                Console.WriteLine($"\tInstant Date: {currentContext.InstantDate}");
            }
            else if (currentContext.DurationPeriod)
            {
                Console.WriteLine("period");
                Console.WriteLine($"\tPeriod Date : from {currentContext.PeriodStartDate} to {currentContext.PeriodEndDate}");
            }
            else if (currentContext.ForeverPeriod)
            {
                Console.WriteLine("forever");
            }
        }

        // GEPSIO - Show units in fragment
        private static void ShowUnitsInFragment(XbrlFragment currentFragment)
        {
            foreach (var currentUnit in currentFragment.Units)
            {
                ShowUnit(currentUnit);
            }
        }

        // GEPSIO - Show unit
        private static void ShowUnit(Unit currentUnit)
        {
            Console.WriteLine("UNIT");
            Console.WriteLine($"\tID  : {currentUnit.Id}");
            foreach (var currentMeasureQualifiedName in currentUnit.MeasureQualifiedNames)
            {
                Console.WriteLine($"\tName: {currentMeasureQualifiedName.Namespace}:{currentMeasureQualifiedName.LocalName}");
            }
        }
    }
}
