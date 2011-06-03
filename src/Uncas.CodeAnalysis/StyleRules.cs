// -----------------------------------------------------------------------
// <copyright file="StyleRules.cs" company="Uncas">
// Copyright Ole Lynge Sørensen 2011.
// </copyright>
// -----------------------------------------------------------------------

namespace Uncas.CodeAnalysis
{
    using StyleCop;
    using StyleCop.CSharp;

    /// <summary>
    /// Source analysis rules.
    /// </summary>
    [SourceAnalyzer(typeof(CsParser))]
    public class StyleRules : SourceAnalyzer
    {
        /// <summary>
        /// Analyzes the document.
        /// </summary>
        /// <param name="document">The document.</param>
        public override void AnalyzeDocument(CodeDocument document)
        {
            CsDocument document2 = (CsDocument)document;
            if (document2.RootElement != null &&
                !document2.RootElement.Generated)
            {
                document2.WalkDocument(
                    new CodeWalkerElementVisitor<object>(this.VisitElement),
                    null,
                    null);
            }
        }

        /// <summary>
        /// Visits the element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="parentElement">The parent element.</param>
        /// <param name="context">The context.</param>
        /// <returns>True or false.</returns>
        private bool VisitElement(
            CsElement element,
            CsElement parentElement,
            object context)
        {
            this.FieldNamesMustBeginWithUnderscore(element);
            return true;
        }

        /// <summary>
        /// Checks if field names starts with underscore.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>True or false.</returns>
        private bool FieldNamesMustBeginWithUnderscore(
            CsElement element)
        {
            if (!element.Generated &&
                element.ElementType == ElementType.Field &&
                element.ActualAccess != AccessModifierType.Public &&
                element.ActualAccess != AccessModifierType.Internal &&
                element.Declaration.Name.ToCharArray()[0] != '_')
            {
                AddViolation(element, "FieldNamesMustBeginWithUnderscore");
            }

            return true;
        }
    }
}
