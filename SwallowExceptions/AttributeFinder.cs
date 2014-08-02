using Mono.Cecil;

namespace BasicFodyAddin.Fody
{
    public class AttributeFinder
    {
        public AttributeFinder(MethodDefinition method)
        {
            var customAttributes = method.CustomAttributes;
            if (customAttributes.ContainsAttribute("BasicFodyAddin.Fody.SwallowExceptionAttribute"))
            {
                Swallow = true;
            }
        }

        public bool Swallow;
    }
}


