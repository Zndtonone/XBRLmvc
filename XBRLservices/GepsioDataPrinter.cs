using JeffFerguson.Gepsio;
using System;
using System.Collections.Generic;
using System.Text;

namespace XBRLservices
{
    /// <summary>
    /// This class is a test class that prints data from Gepsio.
    /// </summary>
    public class GepsioDataPrinter
    {
        //==================================================== PUBLIC METHODS (PRINTING METHODS) =================================================================
        
        // GEPSIO - Print labels for facts to console
        public static void PrintLabelsForFacts(XbrlDocument xbrlInstance)
        {
            foreach (var currentFragment in xbrlInstance.XbrlFragments)
            {
                foreach (var currentFact in currentFragment.Facts)
                {
                    var matchingLabel = FindLabelForFact(currentFact, currentFragment);

                    if (string.IsNullOrEmpty(matchingLabel) == false)
                    {
                        Console.WriteLine($"The label for fact {currentFact.Name} is '{matchingLabel}'.");
                    }
                }
            }
        }

        // GEPSIO - Prints out facts in document
        public static void ShowFactsInDocument(XbrlDocument doc)
        {
            Console.WriteLine("\n");
            Console.WriteLine("FACTS IN DOCUMENT");
            Console.WriteLine("====================================================================================================");
            foreach (var currentFragment in doc.XbrlFragments)
            {
                ShowFactsInFragment(currentFragment);
            }
        }

        // GEPSIO - Returns contexts in document
        public static void ShowContextsInDocument(XbrlDocument doc)
        {
            Console.WriteLine("\n");
            Console.WriteLine("CONTEXTS IN DOCUMENT");
            Console.WriteLine("====================================================================================================");
            foreach (var currentFragment in doc.XbrlFragments)
            {
                ShowContextsInFragment(currentFragment);
            }
        }

        // GEPSIO - Show units in document
        public static void ShowUnitsInDocument(XbrlDocument doc)
        {
            Console.WriteLine("\n");
            Console.WriteLine("UNITS IN DOCUMENT");
            Console.WriteLine("====================================================================================================");
            foreach (var currentFragment in doc.XbrlFragments)
            {
                ShowUnitsInFragment(currentFragment);
            }
        }

        //==================================================== PRIVATE METHODS (USED IN PRINTING METHODS) ========================================================

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

            //if (item.ContextRef != null)
            //    ShowContext(item.ContextRef);
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
