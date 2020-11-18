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
        // GEPSIO - Get contexts in fragment in a list
        public static List<object> GetValuesFromContextToList(XbrlDocument xbrlInstance)
        {
            List<object> contextList = new List<object>();

            foreach (var currentFragment in xbrlInstance.XbrlFragments)
            {
                foreach (var currentContext in currentFragment.Contexts)
                {
                    string[] arr = GetContext(currentContext);

                    {
                        var c = new
                        {
                            DurationPeriod = arr[0],
                            ForeverPeriod = arr[1],
                            Fragment = arr[2],
                            Id = arr[3],
                            Identifier = arr[4],
                            IdentifierScheme = arr[5],
                            InstantDate = arr[6],
                            InstantPeriod = arr[7],
                            PeriodEndDate = arr[8],
                            PeriodStartDate = arr[9],
                        };
                        contextList.Add(c);
                    }
                }
            }

            return contextList;
        }

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
                            Name = currentFact.Name,
                            Label = matchingLabel,
                            Value = arr[0],
                            Namespace = arr[1],
                            ContextRefName = arr[2],
                            Id = arr[3],
                            UnitRefName = arr[4]
                        };
                        factList.Add(f);
                    }
                }
            }

            return factList;
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

        // RTH - Get values from fact
        private static string[] GetFact(Fact fact)
        {
            if (fact is Item)
            {
                string[] arr = GetItem(fact as Item);
                return arr;
            }

            return null;
        }

        // GEPSIO - Get values from context
        private static string[] GetContext(Context currentContext)
        {
            string[] arr;

            arr = new string[]
            {
                currentContext.DurationPeriod.ToString(),
                currentContext.ForeverPeriod.ToString(),
                currentContext.Fragment.ToString(),
                currentContext.Id,
                currentContext.Identifier,
                currentContext.IdentifierScheme,
                currentContext.InstantDate.ToString(),
                currentContext.InstantPeriod.ToString(),
                currentContext.PeriodEndDate.ToString(),
                currentContext.PeriodStartDate.ToString()
            };

            return arr;
        }

        // RTH - Show item
        private static string[] GetItem(Item item)
        {
            string[] arr;

            arr = new string[5]
            {
                item.Value,
                item.Namespace,
                item.ContextRefName,
                item.Id,
                item.UnitRefName
            };

            return arr;
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
