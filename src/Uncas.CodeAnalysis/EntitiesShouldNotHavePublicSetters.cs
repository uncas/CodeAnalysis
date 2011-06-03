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
        /// Initializes a new instance of the <see cref="EntitiesShouldNotHavePublicSetters"/> class.
        /// </summary>
        public EntitiesShouldNotHavePublicSetters()
            : base("EntitiesShouldNotHavePublicSetters")
        {
        }

        /// <summary>
        /// Checks the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A list of problems.</returns>
        public override ProblemCollection Check(TypeNode type)
        {
            var entityReference =
                type.DeclaringModule.ContainingAssembly.AssemblyReferences
                .SingleOrDefault(
                ar => ar.Assembly.Name.StartsWith(
                    "Uncas.CodeAnalysis.TestLibrary",
                    StringComparison.OrdinalIgnoreCase));

            if (entityReference == null)
            {
                return null;
            }

            var entityType =
                entityReference.Assembly.Types.Single(
                t => t.FullName == "Uncas.CodeAnalysis.TestLibrary.Entity");

            if (!type.IsDerivedFrom(entityType))
            {
                return null;
            }

            var publicProperties = type.Members.OfType<PropertyNode>()
                .Where(x => x.Setter.IsPublic);

            foreach (var publicProperty in publicProperties)
            {
                var resolution = new Resolution(type.Name.Name);

                // var resolution = new Resolution(
                //    type.Name.Name,
                //    "Property {0} has a public setter.",
                //    publicProperty.Name.Name);
                var problem = new Problem(resolution, type)
                {
                    Certainty = 100,
                    FixCategory = FixCategories.Breaking,
                    MessageLevel = MessageLevel.Warning,
                };
                Problems.Add(problem);
            }

            return Problems;
        }
    }
}
