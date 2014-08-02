![Icon](https://raw.githubusercontent.com/duaneedwards/SwallowExceptions/master/Icons/package_icon.png)

Swallow Exceptions
=================

A C# Fody plugin that allows you to annotate a method with the [SwallowExceptions] attribute to have the method contents wrapped in a try / catch block.

[Introduction to Fody](https://github.com/Fody/Fody/wiki/SampleUsage)

## Nuget

 * SwallowExceptions package https://www.nuget.org/packages/SwallowExceptions.Fody

    PM> Install-Package SwallowExceptions.Fody
    
### Your Code

```
public class MyClass
{
    [SwallowExceptions]
    void MyMethod()
    {
      DoSomethingDangerous();
    }
}
```

### What gets compiled

```
public class MyClass
{
    static Logger logger = LogManager.GetLogger("MyClass");

    void MyMethod()
    {
      try
      {
        DoSomethingDangerous();
      }
      catch (Exception exception)
      {
      
      }
    }
}
