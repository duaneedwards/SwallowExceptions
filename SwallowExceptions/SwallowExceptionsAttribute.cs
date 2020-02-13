using System;

namespace SwallowExceptions.Fody
{
    /// <summary>
    /// If an <see cref="Exception"/> occurs in the applied method then swallow it <c>Error</c>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public class SwallowExceptionsAttribute : Attribute
    {
    }
}