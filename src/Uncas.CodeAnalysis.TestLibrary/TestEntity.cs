// -----------------------------------------------------------------------
// <copyright file="TestEntity.cs" company="Uncas">
// Copyright Ole Lynge Sørensen 2011.
// </copyright>
// -----------------------------------------------------------------------

namespace Uncas.CodeAnalysis.TestLibrary
{
    using System;

    /// <summary>
    /// Test entity that breaks a code analysis rule for entities.
    /// </summary>
    public class TestEntity : Entity
    {
        /// <summary>
        /// My constant.
        /// </summary>
        private const int MyConstant = 3;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestEntity"/> class.
        /// </summary>
        /// <param name="myProperty">My property.</param>
        public TestEntity(int myProperty)
        {
            if (myProperty >= int.MaxValue - MyConstant)
            {
                throw new ArgumentOutOfRangeException("myProperty");
            }

            MyProperty = myProperty + MyConstant;
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
