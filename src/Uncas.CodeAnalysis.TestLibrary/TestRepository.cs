// -----------------------------------------------------------------------
// <copyright file="TestRepository.cs" company="Uncas">
// Copyright Ole Lynge Sørensen 2011.
// </copyright>
// -----------------------------------------------------------------------

namespace Uncas.CodeAnalysis.TestLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TestRepository : Repository
    {
        [SuppressMessage(
            "Microsoft.Design",
            "CA1024:UsePropertiesWhereAppropriate",
            Justification = "This is a test method")]
        public static IQueryable<int> GetStuff()
        {
            throw new NotImplementedException();
        }

        [SuppressMessage(
            "Microsoft.Design",
            "CA1024:UsePropertiesWhereAppropriate",
            Justification = "This is a test method")]
        public static IEnumerable<int> GetItems()
        {
            throw new NotImplementedException();
        }
    }
}
