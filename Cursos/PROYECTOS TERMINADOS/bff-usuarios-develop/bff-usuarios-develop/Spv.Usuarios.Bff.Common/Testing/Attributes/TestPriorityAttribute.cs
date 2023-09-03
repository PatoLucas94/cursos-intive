using System;
using System.Diagnostics.CodeAnalysis;

namespace Spv.Usuarios.Bff.Common.Testing.Attributes
{
    [ExcludeFromCodeCoverage]
    public class PriorityAttribute
    {
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
        public class TestPriorityAttribute : Attribute
        {
            public int Priority { get; private set; }

            public TestPriorityAttribute(int priority) => Priority = priority;
        }
    }
}
