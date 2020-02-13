using Mono.Cecil;

namespace SwallowExceptions.Fody
{
    public class AttributeFinder
    {
        public AttributeFinder(MethodDefinition method)
        {
            var customAttributes = method.CustomAttributes;
            if (customAttributes.ContainsAttribute("SwallowExceptions.Fody.SwallowExceptionsAttribute"))
            {
                Swallow = true;
            }
        }

        public bool Swallow;
    }
}