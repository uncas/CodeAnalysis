// -----------------------------------------------------------------------
// <copyright file="InheritanceChecker.cs" company="Uncas">
// Copyright Ole Lynge Sørensen 2011.
// </copyright>
// -----------------------------------------------------------------------

namespace Uncas.CodeAnalysis
{
    using System;
    using System.Linq;
    using Microsoft.FxCop.Sdk;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class InheritanceChecker
    {
        /// <summary>
        /// Determines whether the specified node type inherits from the given base type name from the given base assembly.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="baseAssemblyName">Name of the base assembly.</param>
        /// <param name="baseTypeName">Name of the base type.</param>
        /// <returns>
        /// <c>true</c> if the specified type inherits from the base type; otherwise, <c>false</c>.
        /// </returns>
        public static bool InheritsType(
            TypeNode type,
            string baseAssemblyName,
            string baseTypeName)
        {
            var containingAssembly =
                type.DeclaringModule.ContainingAssembly;

            AssemblyNode entityAssembly;

            if (IsBaseAssembly(containingAssembly, baseAssemblyName))
            {
                entityAssembly = containingAssembly;
            }
            else
            {
                var entityReference =
                    containingAssembly.AssemblyReferences
                    .SingleOrDefault(
                    ar => IsBaseAssembly(ar.Assembly, baseAssemblyName));
                if (entityReference == null)
                {
                    return false;
                }

                entityAssembly = entityReference.Assembly;
            }

            var entityType =
                entityAssembly.Types.Single(
                t => t.FullName == baseTypeName);

            return type.IsDerivedFrom(entityType);
        }

        /// <summary>
        /// Determines whether the assembly is the base assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="baseAssemblyName">Name of the base assembly.</param>
        /// <returns>
        /// <c>true</c> if the specified assembly is the base assembly; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsBaseAssembly(
            AssemblyNode assembly,
            string baseAssemblyName)
        {
            return assembly.Name.StartsWith(
                baseAssemblyName,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
