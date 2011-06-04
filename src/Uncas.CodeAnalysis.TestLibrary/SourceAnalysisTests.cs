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
        public void AvoidIfDebug_ContainsIfDebug_Violated()
        {
            string contents = @"
namespace TestCode
{
    public class Test
    {
        private const int MyConstant = 3;

#if debug
        private const int MyDebugConstant = 5;
#endif
    }
}";
            GenerateSourceFile(contents);

            RunAnalysis();

            AssertViolated("AvoidIfDebug");
        }

        [Test]
        public void AvoidIfDebug_DoesNotContainIfDebug_NotViolated()
        {
            string contents = @"
namespace TestCode
{
    public class Test
    {
        private const int MyConstant = 3;
    }
}";
            GenerateSourceFile(contents);

            RunAnalysis();

            AssertNotViolated("AvoidIfDebug");
        }

        [Test]
        public void FieldNamesMustBeginWithUnderscore_ConstantWithoutUnderscore_NotViolated()
        {
            string contents = @"
namespace TestCode
{
    public class Test
    {
        private const int MyConstant = 3;
    }
}";
            GenerateSourceFile(contents);

            RunAnalysis();

            AssertNotViolated("FieldNamesMustBeginWithUnderscore");
        }

        [Test]
        public void FieldNamesMustBeginWithUnderscore_FieldNameWithoutUnderscore_Violated()
        {
            string contents = @"
namespace TestCode
{
    public class Test
    {
        private int MyConstant = 3;
    }
}";
            GenerateSourceFile(contents);

            RunAnalysis();

            AssertViolated("FieldNamesMustBeginWithUnderscore");
        }

        [Test]
        public void TooLongClass_FourHundredLines_NotViolated()
        {
            const int NumberOfLines = 400;
            string contents = GetCodeWithMethod(NumberOfLines);
            GenerateSourceFile(contents);

            RunAnalysis();

            AssertNotViolated("TooLongClass");
        }

        [Test]
        public void TooLongClass_FiveHundredLines_Violated()
        {
            const int NumberOfLines = 500;
            string contents = GetCodeWithMethod(NumberOfLines);
            GenerateSourceFile(contents);

            RunAnalysis();

            AssertViolated("TooLongClass");
        }

        [Test]
        public void TooLongMethod_FortyLines_NotViolated()
        {
            const int NumberOfLines = 40;
            string contents = GetCodeWithMethod(NumberOfLines);
            GenerateSourceFile(contents);

            RunAnalysis();

            AssertNotViolated("TooLongMethod");
        }

        [Test]
        public void TooLongMethod_SixtyLines_Violated()
        {
            const int NumberOfLines = 60;
            string contents = GetCodeWithMethod(NumberOfLines);
            GenerateSourceFile(contents);

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

        private void GenerateSourceFile(string contents)
        {
            const string FileName = "Test.cs";
            string path = Path.GetFullPath(FileName);
            File.WriteAllText(path, contents);
            AddSourceFile(FileName);
        }
    }
}