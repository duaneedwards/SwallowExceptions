using System;
using SwallowExceptions.Fody;

namespace AssemblyToProcess
{
    public class OnException
    {
        [SwallowExceptions]
        public void AnnotatedMethodShouldntThrow()
        {
            throw new Exception("Testing");
        }

        public void UnannotatedMethodShouldThrow()
        {
            throw new Exception("Testing");
        }

        public void ReferenceMethod()
        {
            try
            {
                throw new Exception("Testing");
            }
            catch (Exception)
            {
                
            }
        }
    }
}
