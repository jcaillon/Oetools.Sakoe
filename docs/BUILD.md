# BUILD

## Build tools required

You can simply install the latest visual studio version (2017 atm).

Or, if you don't want to install everything but just build with `build.bat` or a third party IDE, you can install msbuild tools :

https://visualstudio.microsoft.com/downloads/#build-tools-for-visual-studio-2017

With the following options :

- MsBuild tools
- .Net Core build tools
- Individual components :
  - .Net framework 4.6.1 SKD
  - .Net framework 4.6.1 Targeting pack

## Manual build command

```bat
build.bat
```

## Additionnal remarks

*Why are the libraries like Oetools.Packager targetting explicitly net461 AND netstandard2.0?*

This question is legit, according to this [net-implementation-support table](https://docs.microsoft.com/en-us/dotnet/standard/net-standard#net-implementation-support), we should be able to make our libraries target netstandard2.0 and that's it. Since our application is targetting v4.6.1 and since v4.6.1 implements netstandard2.0 we should be good. But nop! This is explained here :

- https://www.youtube.com/watch?v=u67Eu_IgEMs&list=PLRAdsfhKI4OWx321A_pr-7HhRNk7wOLLY&index=8
- https://stackoverflow.com/questions/47365136/why-does-my-net-standard-nuget-package-trigger-so-many-dependencies/47366401#47366401
- also for reference on pb with nuget : https://github.com/dotnet/standard/issues/481

So yeah. I have to explicitly target net461.

## Creating a releasing

- Update the Version in .csproj, the format should be X.X.X-suffix (with suffix being optional)
- Create a new tag : `git tag X.X.X-suffix`
- Push the tag `git push --tags` : this will trigger the build with the deployment on the appveyor
