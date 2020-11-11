using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents an extended label linkbase document.
    /// </summary>
    public class ExtendedLinkbaseDocument : LinkbaseDocument
    {
        /// <summary>
        /// A collection of <see cref="ExtendedLink"/> objects defined by the linkbase document.
        /// </summary>
        public List<ExtendedLink> LabelLinks { get; private set; }

        internal ExtendedLinkbaseDocument(string ContainingDocumentUri, string DocumentPath, XbrlFragment containingFragment)
            : base(ContainingDocumentUri, DocumentPath, containingFragment)
        {
            LabelLinks = new List<ExtendedLink>();
            foreach (INode CurrentChild in thisLinkbaseNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("link") == true)
                    this.LabelLinks.Add(new ExtendedLink(CurrentChild));
            }
        }
    }
}
