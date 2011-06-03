// -----------------------------------------------------------------------
// <copyright file="BaseRule.cs" company="Uncas">
// Copyright Ole Lynge Sørensen 2011.
// </copyright>
// -----------------------------------------------------------------------

namespace Uncas.CodeAnalysis
{
    using Microsoft.FxCop.Sdk;

    /// <summary>
    /// The base class for rules.
    /// </summary>
    public abstract class BaseRule : BaseIntrospectionRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRule"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        protected BaseRule(string name)
            : base(
                name,
                typeof(BaseRule).Assembly.GetName().Name + ".Rules",
                typeof(BaseRule).Assembly)
        {
        }
    }
}