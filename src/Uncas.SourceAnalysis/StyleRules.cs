// -----------------------------------------------------------------------
// <copyright file="StyleRules.cs" company="Uncas">
// Copyright Ole Lynge Sørensen 2011.
// </copyright>
// -----------------------------------------------------------------------

namespace Uncas.SourceAnalysis
{
    using System;
    using System.IO;
    using System.Linq;
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
        /// The maximum class length.
        /// </summary>
        private const int MaxClassLength = 500;

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
                   new CodeWalkerElementVisitor<object>(VisitElement),
                   new CodeWalkerStatementVisitor<object>(VisitStatement),
                   new CodeWalkerExpressionVisitor<object>(VisitExpression),
                   this);
            }
        }

        /// <summary>
        /// Lowers the case first letter.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>The word with the lowercased first letter.</returns>
        private static string LowerCaseFirstLetter(string word)
        {
            string first = word.Substring(0, 1);
            string last = word.Substring(1);
            return string.Format("{0}{1}", first.ToLowerInvariant(), last);
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
            FieldNamesMustBeginWithUnderscore(element);
            TooLongMethod(element);
            TooLongClass(element);
            AvoidIfDebug(element);
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
        private void FieldNamesMustBeginWithUnderscore(
            CsElement element)
        {
            if (element.Generated ||
                element.ElementType != ElementType.Field ||
                element.ActualAccess == AccessModifierType.Public ||
                element.ActualAccess == AccessModifierType.Internal ||
                element.Declaration.Name.ToCharArray()[0] == '_' ||
                element.Declaration.Tokens.Any(x => x.CsTokenType == CsTokenType.Const))
            {
                return;
            }

            AddViolation(
                element,
                "FieldNamesMustBeginWithUnderscore",
                element.Declaration.Name,
                LowerCaseFirstLetter(element.Declaration.Name));
        }

        /// <summary>
        /// Tooes the long method.
        /// </summary>
        /// <param name="element">The element.</param>
        private void TooLongMethod(
            CsElement element)
        {
            var elementType = ElementType.Method;
            string ruleName = "TooLongMethod";
            var maxLength = MaxMethodLength;
            TooLongElement(
                element,
                elementType,
                ruleName,
                maxLength);
        }

        /// <summary>
        /// Tooes the long class.
        /// </summary>
        /// <param name="element">The element.</param>
        private void TooLongClass(CsElement element)
        {
            var elementType = ElementType.Class;
            string ruleName = "TooLongClass";
            var maxLength = MaxClassLength;
            TooLongElement(
                element,
                elementType,
                ruleName,
                maxLength);
        }

        /// <summary>
        /// Avoids if debug.
        /// </summary>
        /// <param name="element">The element.</param>
        private void AvoidIfDebug(CsElement element)
        {
            if (element.ElementType != ElementType.Root)
            {
                return;
            }

            string filePath = element.Document.SourceCode.Path;
            string contents = File.ReadAllText(filePath);

            if (!contents.Contains("#if debug"))
            {
                return;
            }

            AddViolation(
                element,
                "AvoidIfDebug",
                element.Declaration.Name);
        }

        /// <summary>
        /// Tooes the long element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="elementType">Type of the element.</param>
        /// <param name="ruleName">Name of the rule.</param>
        /// <param name="maxLength">Length of the max.</param>
        private void TooLongElement(
            CsElement element,
            ElementType elementType,
            string ruleName,
            int maxLength)
        {
            if (element.ElementType != elementType)
            {
                return;
            }

            int firstLineNumber = element.LineNumber;
            int lastLineNumber = firstLineNumber;

            foreach (var statement in element.ChildStatements)
            {
                lastLineNumber = Math.Max(lastLineNumber, statement.LineNumber);
            }

            foreach (var childElement in element.ChildElements)
            {
                foreach (var statement in childElement.ChildStatements)
                {
                    lastLineNumber = Math.Max(lastLineNumber, statement.LineNumber);
                }
            }

            int numberOfLines = lastLineNumber - firstLineNumber + 1;
            if (numberOfLines > maxLength)
            {
                AddViolation(
                    element,
                    ruleName,
                    element.Declaration.Name,
                    numberOfLines,
                    maxLength);
            }
        }
    }
}
