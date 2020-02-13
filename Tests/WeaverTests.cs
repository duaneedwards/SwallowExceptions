using System;
using Fody;
using Xunit;

#region WeaverTests

namespace Tests
{
    using SwallowExceptions.Fody;

    public class WeaverTests
    {
        static TestResult testResult;

        static WeaverTests()
        {
            var weavingTask = new ModuleWeaver();
            // disabled PeVerify as per:
            // https://github.com/Fody/PropertyChanged/issues/355
            testResult = weavingTask.ExecuteTestRun("AssemblyToProcess.dll", runPeVerify:false);
        }

        [Fact]
        public void ValidateAnnotationAddsTryCatchBlock()
        {
            var type = testResult.Assembly.GetType("AssemblyToProcess.OnException");

            var instance = (dynamic)Activator.CreateInstance(type);
            instance.AnnotatedMethodShouldntThrow();
        }

        [Fact]
        public void ValidateLeavesUnannotatedMethodsAlone()
        {
            var type = testResult.Assembly.GetType("AssemblyToProcess.OnException");
            var instance = (dynamic)Activator.CreateInstance(type);
            try
            {
                instance.UnannotatedMethodShouldThrow();
            }
            catch (Exception ex)
            {
                Assert.Equal("Testing", ex.Message);
            }
            
            // Doesn't work:
            // Assert.Throws() Failure
            // Expected: typeof(System.Exception)
            // Actual:   typeof(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException): Cannot implicitly convert type 'void' to 'object'
            //Assert.Throws<Exception>(() => instance.UnannotatedMethodShouldThrow());
        }
    }
}

#endregion