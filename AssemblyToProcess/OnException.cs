
using System;
using BasicFodyAddin.Fody;

namespace AssemblyToProcess
{
    public class OnException
    {
        [SwallowException]
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
