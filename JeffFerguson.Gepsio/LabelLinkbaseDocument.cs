﻿using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents a label linkbase document.
    /// </summary>
    public class LabelLinkbaseDocument : LinkbaseDocument
    {
        /// <summary>
        /// A collection of <see cref="LabelLink"/> objects defined by the linkbase document.
        /// </summary>
        public List<LabelLink> LabelLinks { get; private set; }

        internal LabelLinkbaseDocument(string ContainingDocumentUri, string DocumentPath, XbrlFragment containingFragment)
            : base(ContainingDocumentUri, DocumentPath, containingFragment)
        {
            LabelLinks = new List<LabelLink>();
        }

        private LabelLink lLabelLink = null;

        public void addLabelDocuments(string ContainingDocumentUri, string DocumentPath, XbrlFragment containingFragment)
        {
            foreach (INode CurrentChild in thisLinkbaseNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("labelLink") == true)
                {
                    if (lLabelLink == null)
                    {
                        lLabelLink = new LabelLink(CurrentChild);
                        this.LabelLinks.Add(lLabelLink);
                    }

                    lLabelLink.ReadChildLabels(CurrentChild);
                    lLabelLink.ReadChildLabelArcs(CurrentChild);
                    lLabelLink.ReadChildLocators(CurrentChild);
                }
            }
        }
    }
}
