// -----------------------------------------------------------------------
// <copyright file="EntitiesShouldNotHavePublicSetters.cs" company="Uncas">
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
    /// Rule that forbids public setters on entities.
    /// </summary>
    public class EntitiesShouldNotHavePublicSetters : BaseRule
    {
        /// <summary>
        /// The name of the assembly containing the base entity type.
        /// </summary>
        private const string EntityAssemblyName = "Uncas.CodeAnalysis.TestLibrary";

        /// <summary>
        /// The name of the entity type.
        /// </summary>
        private const string EntityTypeName = "Uncas.CodeAnalysis.TestLibrary.Entity";

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitiesShouldNotHavePublicSetters"/> class.
        /// </summary>
        public EntitiesShouldNotHavePublicSetters()
            : base("EntitiesShouldNotHavePublicSetters")
        {
        }

        /// <summary>
        /// Checks the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>A list of problems.</returns>
        public override ProblemCollection Check(Member member)
        {
            var property = member as PropertyNode;
            if (property == null)
            {
                return null;
            }

            var type = property.DeclaringType;
            if (!IsEntity(type))
            {
                return null;
            }

            if (!property.Setter.IsPublic)
            {
                return null;
            }

            var resolution = new Resolution(
                property.Name.Name,
                "The property {0}.{1} of entity {0} has a public setter, which is not allowed for entities.",
                type.Name.Name,
                property.Name.Name);
            var problem = new Problem(resolution, property)
            {
                Certainty = 80,
                FixCategory = FixCategories.Breaking,
                MessageLevel = MessageLevel.Warning,
            };
            Problems.Add(problem);

            return Problems;
        }

        /// <summary>
        /// Determines whether the specified type is an entity.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// <c>true</c> if the specified type is an entity; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsEntity(TypeNode type)
        {
            var containingAssembly =
                type.DeclaringModule.ContainingAssembly;

            AssemblyNode entityAssembly;

            if (IsEntityAssembly(containingAssembly))
            {
                entityAssembly = containingAssembly;
            }
            else
            {
                var entityReference =
                    containingAssembly.AssemblyReferences
                    .SingleOrDefault(
                    ar => IsEntityAssembly(ar.Assembly));
                if (entityReference == null)
                {
                    return false;
                }

                entityAssembly = entityReference.Assembly;
            }

            var entityType =
                entityAssembly.Types.Single(
                t => t.FullName == EntityTypeName);

            return type.IsDerivedFrom(entityType);
        }

        /// <summary>
        /// Determines whether the assembly is an entity assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        /// <c>true</c> if the specified assembly is an entity assembly; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsEntityAssembly(AssemblyNode assembly)
        {
            return assembly.Name.StartsWith(
                EntityAssemblyName,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
