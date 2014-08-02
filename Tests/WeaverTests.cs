using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using NUnit.Framework;

[TestFixture]
public class WeaverTests
{
    Assembly assembly;
    string newAssemblyPath;
    string assemblyPath;

    [TestFixtureSetUp]
    public void Setup()
    {
        var projectPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\AssemblyToProcess\AssemblyToProcess.csproj"));
        assemblyPath = Path.Combine(Path.GetDirectoryName(projectPath), @"bin\Debug\AssemblyToProcess.dll");
#if (!DEBUG)
        assemblyPath = assemblyPath.Replace("Debug", "Release");
#endif

        newAssemblyPath = WeaverHelper.Weave(assemblyPath);

        assembly = Assembly.LoadFile(newAssemblyPath);
    }

    [Test]
    public void ValidateAnnotationAddsTryCatchBlock()
    {
        var type = assembly.GetType("AssemblyToProcess.OnException");

        var instance = (dynamic)Activator.CreateInstance(type);
        Assert.DoesNotThrow(() => instance.AnnotatedMethodShouldntThrow());
    }

    [Test]
    public void ValidateLeavesUnannotatedMethodsAlone()
    {
        var type = assembly.GetType("AssemblyToProcess.OnException");
        var instance = (dynamic)Activator.CreateInstance(type);
        Assert.Throws<Exception>(() => instance.UnannotatedMethodShouldThrow());
    }


#if(DEBUG)
    [Test]
    public void PeVerify()
    {
        Verifier.Verify(assemblyPath,newAssemblyPath);
    }
#endif
}