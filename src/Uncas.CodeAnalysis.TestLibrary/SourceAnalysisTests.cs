// -----------------------------------------------------------------------
// <copyright file="SourceAnalysisTests.cs" company="Microsoft">
// Copyright Ole Lynge Sørensen 2011.
// </copyright>
// -----------------------------------------------------------------------

namespace Uncas.CodeAnalysis.TestLibrary
{
    using System.IO;
    using System.Text;
    using NUnit.Framework;

    /// <summary>
    /// Test of source analysis rules.
    /// </summary>
    [TestFixture]
    public class SourceAnalysisTests : StyleCopFixtureBase
    {
        [Test]
        public void FieldNamesMustBeginWithUnderscore_ConstantWithoutUnderscore_NotViolated()
        {
            string fileName = "Test.cs";
            string contents = @"
namespace TestCode
{
    public class Test
    {
        private const int MyConstant = 3;
    }
}";
            GenerateSourceFile(fileName, contents);

            RunAnalysis();

            AssertNotViolated("FieldNamesMustBeginWithUnderscore");
        }

        [Test]
        public void FieldNamesMustBeginWithUnderscore_FieldNameWithoutUnderscore_Violated()
        {
            string fileName = "Test.cs";
            string contents = @"
namespace TestCode
{
    public class Test
    {
        private int MyConstant = 3;
    }
}";
            GenerateSourceFile(fileName, contents);

            RunAnalysis();

            AssertViolated("FieldNamesMustBeginWithUnderscore");
        }

        [Test]
        public void TooLongClass_FourHundredLines_NotViolated()
        {
            string fileName = "Test.cs";
            const int NumberOfLines = 400;
            string contents = GetCodeWithMethod(NumberOfLines);
            GenerateSourceFile(fileName, contents);

            RunAnalysis();

            AssertNotViolated("TooLongClass");
        }

        [Test]
        public void TooLongClass_FiveHundredLines_Violated()
        {
            string fileName = "Test.cs";
            const int NumberOfLines = 500;
            string contents = GetCodeWithMethod(NumberOfLines);
            GenerateSourceFile(fileName, contents);

            RunAnalysis();

            AssertViolated("TooLongClass");
        }

        [Test]
        public void TooLongMethod_FortyLines_NotViolated()
        {
            string fileName = "Test.cs";
            const int NumberOfLines = 40;
            string contents = GetCodeWithMethod(NumberOfLines);
            GenerateSourceFile(fileName, contents);

            RunAnalysis();

            AssertNotViolated("TooLongMethod");
        }

        [Test]
        public void TooLongMethod_SixtyLines_Violated()
        {
            string fileName = "Test.cs";
            const int NumberOfLines = 60;
            string contents = GetCodeWithMethod(NumberOfLines);
            GenerateSourceFile(fileName, contents);

            RunAnalysis();

            AssertViolated("TooLongMethod");
        }

        private static string GetCodeWithMethod(int methodLength)
        {
            var builder = new StringBuilder();
            builder.Append(@"
namespace TestCode
{
    public class Test
    {
        private void MyMethod()
        {
");
            for (int i = 0; i < methodLength; i++)
            {
                builder.AppendLine("System.Console.WriteLine(\"test\");");
            }

            builder.Append(@"
        }
    }
}");
            return builder.ToString();
        }

        private void GenerateSourceFile(string fileName, string contents)
        {
            string path = Path.GetFullPath(fileName);
            File.WriteAllText(path, contents);
            AddSourceFile(fileName);
        }
    }
}