Swallow Exceptions
=================

A C# Fody plugin that allows you to annotate a method with the [SwallowExceptions] attribute to have the method contents wrapped in a try / catch block.

[Introduction to Fody](https://github.com/Fody/Home/blob/master/pages/usage.md)

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
```

### Working alongside other Fody Plugins

In the case of other attribute based Fody plugins, i.e. Anotar, the order that the plugins get invoked becomes important.

In most cases you'd like this plugin to be the last plugin to inject code into a method, to do so ensure that it is the last plugin listed in the project's FodyWeavers.xml file like shown here:

```
<?xml version="1.0" encoding="utf-8"?>
<Weavers>
  <Anotar.Log4Net />
  <SwallowExceptions />
</Weavers>

```

In this case, you can do things like the following, where Anotar will log the exception, then SwallowExceptions will catch the rethrown exception:

```
public class MyClass
{
    [LogToFatalOnException, SwallowExceptions]
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
    void MyMethod()
    {
      try
      {
        try
        {
            DoSomethingDangerous();
        }
        catch (Exception exception)
        {
            if (logger.IsErrorEnabled)
            {
                var message = string.Format("Exception occurred in SimpleClass.MyMethod. param1 '{0}', param2 '{1}'", param1, param2);
                logger.ErrorException(message, exception);
            }
            throw;
        }
      }
      catch (Exception exception)
      {
      
      }
    }
}
```










[![NuGet Status](https://img.shields.io/nuget/v/BasicFodyAddin.Fody.svg)](https://www.nuget.org/packages/BasicFodyAddin.Fody/)

![Icon](https://raw.githubusercontent.com/Fody/Home/master/BasicFodyAddin/package_icon.png)

This is a simple solution used to illustrate how to [write a Fody addin](https://github.com/Fody/Home/blob/master/pages/addin-development.md).


## Usage

See also [Fody usage](https://github.com/Fody/Home/blob/master/pages/usage.md).


### NuGet installation

Install the [BasicFodyAddin.Fody NuGet package](https://www.nuget.org/packages/BasicFodyAddin.Fody/) and update the [Fody NuGet package](https://www.nuget.org/packages/Fody/):

```powershell
PM> Install-Package Fody
PM> Install-Package BasicFodyAddin.Fody
```

The `Install-Package Fody` is required since NuGet always defaults to the oldest, and most buggy, version of any dependency.


### Add to FodyWeavers.xml

Add `<BasicFodyAddin/>` to [FodyWeavers.xml](https://github.com/Fody/Home/blob/master/pages/configuration.md#fodyweaversxml)

```xml
<Weavers>
  <BasicFodyAddin/>
</Weavers>
```


## The moving parts

See [writing an addin](https://github.com/Fody/Home/blob/master/pages/addin-development.md)

## Updating Package Version Number

Edit the value in the Version element of Directory.Build.props

## Creating the NuGet Package

Run the following command in ./SwallowExceptions/

```powershell
dotnet pack .\SwallowExceptions.csproj -c Release
```

The NuGet package will be saved as ./nugets/SwallowExceptions.Fody.(VersionNumber).nupkg


## Icon

[Lego](https://thenounproject.com/term/lego/16919/) designed by [Timur Zima](https://thenounproject.com/timur.zima/) from [The Noun Project](https://thenounproject.com).
