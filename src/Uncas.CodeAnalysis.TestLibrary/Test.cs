// -----------------------------------------------------------------------
// <copyright file="Test.cs" company="Uncas">
// Copyright Ole Lynge Sørensen 2011.
// </copyright>
// -----------------------------------------------------------------------

namespace Uncas.CodeAnalysis.TestLibrary
{
    /// <summary>
    /// Test entity that breaks a code analysis rule for entities.
    /// </summary>
    public class Test : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Test"/> class.
        /// </summary>
        /// <param name="myProperty">My property.</param>
        public Test(int myProperty)
        {
            this.MyProperty = myProperty;
        }

        /// <summary>
        /// Gets my property.
        /// </summary>
        /// <value>My property.</value>
        public int MyProperty { get; private set; }

        /// <summary>
        /// Gets or sets my property2.
        /// </summary>
        /// <remarks>This property breaks a code analysis rule.</remarks>
        /// <value>My property2.</value>
        public int MyProperty2 { get; set; }

        /// <summary>
        /// Gets or sets my property3.
        /// </summary>
        /// <remarks>This property breaks a code analysis rule.</remarks>
        /// <value>My property3.</value>
        public int MyProperty3 { get; set; }

#if debug

        public int MyDebugProperty{ get; set; }

#endif
    }
}
