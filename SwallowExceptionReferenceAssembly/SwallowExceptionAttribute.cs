using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicFodyAddin.Fody
{
    /// <summary>
    /// If an <see cref="Exception"/> occurs in the applied method then swallow it <c>Error</c>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public class SwallowExceptionAttribute : Attribute
    {
    }
}