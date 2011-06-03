// -----------------------------------------------------------------------
// <copyright file="SourceAnalysisTests.cs" company="Microsoft">
// Copyright Ole Lynge Sørensen 2011.
// </copyright>
// -----------------------------------------------------------------------

namespace Uncas.CodeAnalysis.TestLibrary
{
    using NUnit.Framework;

    /// <summary>
    /// Test of source analysis rules.
    /// </summary>
    [TestFixture]
    public class SourceAnalysisTests : StyleCopFixtureBase
    {
        /// <summary>
        /// Xs this instance.
        /// </summary>
        [Test]
        public void X()
        {
            AddSourceFile("Test.cs");
            RunAnalysis();
            AssertViolated("TooLongMethod");
            AssertViolated("TooLongClass");
        }
    }
}
