// -----------------------------------------------------------------------
// <copyright file="StyleRules.cs" company="Uncas">
// Copyright Ole Lynge Sørensen 2011.
// </copyright>
// -----------------------------------------------------------------------

namespace Uncas.SourceAnalysis
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
        /// The maximum method length.
        /// </summary>
        private const int MaxMethodLength = 50;

        /// <summary>
        /// Analyzes the document.
        /// </summary>
        /// <param name="document">The document.</param>
        public override void AnalyzeDocument(CodeDocument document)
        {
            CsDocument csharpDocument = document as CsDocument;
            if (csharpDocument != null &&
                csharpDocument.RootElement != null &&
                !csharpDocument.RootElement.Generated)
            {
                csharpDocument.WalkDocument(
                   new CodeWalkerElementVisitor<object>(this.VisitElement),
                   new CodeWalkerStatementVisitor<object>(this.VisitStatement),
                   new CodeWalkerExpressionVisitor<object>(this.VisitExpression),
                   this);
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
            this.TooLongMethod(element);
            return true;
        }

        /// <summary>
        /// Visits the statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <param name="parentExpression">The parent expression.</param>
        /// <param name="parentStatement">The parent statement.</param>
        /// <param name="parentElement">The parent element.</param>
        /// <param name="context">The context.</param>
        /// <returns>True or false.</returns>
        private bool VisitStatement(
            Statement statement,
            Expression parentExpression,
            Statement parentStatement,
            CsElement parentElement,
            object context)
        {
            // Add your code here.
            return true;
        }

        /// <summary>
        /// Visits the expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="parentExpression">The parent expression.</param>
        /// <param name="parentStatement">The parent statement.</param>
        /// <param name="parentElement">The parent element.</param>
        /// <param name="context">The context.</param>
        /// <returns>True or false.</returns>
        private bool VisitExpression(
            Expression expression,
            Expression parentExpression,
            Statement parentStatement,
            CsElement parentElement,
            object context)
        {
            // Add your code here.
            return true;
        }

        /// <summary>
        /// Checks if field names start with underscore.
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
                AddViolation(
                    element,
                    "FieldNamesMustBeginWithUnderscore",
                    element.Declaration.Name);
            }

            return true;
        }

        /// <summary>
        /// Tooes the long method.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>True or false.</returns>
        private bool TooLongMethod(
            CsElement element)
        {
            if (element.ElementType != ElementType.Method)
            {
                return true;
            }

            int firstLineNumber = element.LineNumber;
            int lastLineNumber = firstLineNumber;

            foreach (var statement in element.ChildStatements)
            {
                lastLineNumber = statement.LineNumber;
            }

            int numberOfLinesInMethod = lastLineNumber - firstLineNumber + 1;
            if (numberOfLinesInMethod > MaxMethodLength)
            {
                AddViolation(
                    element,
                    "TooLongMethod",
                    element.Declaration.Name,
                    numberOfLinesInMethod,
                    MaxMethodLength);
            }

            return true;
        }
    }
}
