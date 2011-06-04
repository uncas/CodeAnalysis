// -----------------------------------------------------------------------
// <copyright file="RepositoriesShouldNotReturnIQueryable.cs" company="Uncas">
// Copyright Ole Lynge Sørensen 2011.
// </copyright>
// -----------------------------------------------------------------------

namespace Uncas.CodeAnalysis
{
    using System;
    using System.Linq;
    using Microsoft.FxCop.Sdk;
    using Microsoft.VisualStudio.CodeAnalysis.Extensibility;

    /// <summary>
    /// Rule that forbids repositories to return IQueryable.
    /// </summary>
    public class RepositoriesShouldNotReturnIQueryable : BaseRule
    {
        /// <summary>
        /// The name of the assembly containing the base entity type.
        /// </summary>
        private const string RepositoryAssemblyName = "Uncas.CodeAnalysis.TestLibrary";

        /// <summary>
        /// The name of the entity type.
        /// </summary>
        private const string RepositoryTypeName = "Uncas.CodeAnalysis.TestLibrary.Entity";

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoriesShouldNotReturnIQueryable"/> class.
        /// </summary>
        public RepositoriesShouldNotReturnIQueryable()
            : base("RepositoriesShouldNotReturnIQueryable")
        {
        }

        /// <summary>
        /// Checks the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>A list of problems.</returns>
        public override ProblemCollection Check(Member member)
        {
            var method = member as Method;
            if (method == null)
            {
                return null;
            }

            var type = method.DeclaringType;
            if (!IsRepository(type))
            {
                return null;
            }

            if (!method.IsPublic)
            {
                return null;
            }

            if (!IsQueryable(method.ReturnType))
            {
                return null;
            }

            var resolution = new Resolution(
                method.Name.Name,
                "The property {0}.{1} of entity {0} has a public setter, which is not allowed for entities.",
                type.Name.Name,
                method.Name.Name);
            var problem = new Problem(resolution, method)
            {
                Certainty = 80,
                FixCategory = FixCategories.Breaking,
                MessageLevel = MessageLevel.Warning,
            };
            Problems.Add(problem);

            return Problems;
        }

        /// <summary>
        /// Determines whether the specified type node is queryable.
        /// </summary>
        /// <param name="typeNode">The type node.</param>
        /// <returns>
        /// <c>true</c> if the specified type node is queryable; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsQueryable(TypeNode typeNode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether the specified type is an entity.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// <c>true</c> if the specified type is an entity; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsRepository(TypeNode type)
        {
            var containingAssembly =
                type.DeclaringModule.ContainingAssembly;

            AssemblyNode entityAssembly;

            if (IsRepositoryAssembly(containingAssembly))
            {
                entityAssembly = containingAssembly;
            }
            else
            {
                var entityReference =
                    containingAssembly.AssemblyReferences
                    .SingleOrDefault(
                    ar => IsRepositoryAssembly(ar.Assembly));
                if (entityReference == null)
                {
                    return false;
                }

                entityAssembly = entityReference.Assembly;
            }

            var entityType =
                entityAssembly.Types.Single(
                t => t.FullName == RepositoryTypeName);

            return type.IsDerivedFrom(entityType);
        }

        /// <summary>
        /// Determines whether the assembly is an entity assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        /// <c>true</c> if the specified assembly is an entity assembly; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsRepositoryAssembly(AssemblyNode assembly)
        {
            return assembly.Name.StartsWith(
                RepositoryAssemblyName,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
