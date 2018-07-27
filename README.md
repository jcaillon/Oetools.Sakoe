# Oetools.Sakoe

[![Build status](https://ci.appveyor.com/api/projects/status/2oyd83vvvlj1tvin?svg=true)](https://ci.appveyor.com/project/jcaillon/oetools-runner)

## About

todo

## External libraries

<https://github.com/natemcmaster/CommandLineUtils>
<https://github.com/Mpdreamz/shellprogressbar>


```
dotnet build
dotnet Oetools.Sakoe\bin\Debug\netcoreapp2.0\Oetools.Sakoe.dll
dotnet run
dotnet test
```

```
mkdir SolutionWithSrcAndTest
cd SolutionWithSrcAndTest
dotnet new sln
dotnet new classlib -o MyProject
dotnet new xunit -o MyProject.Test
dotnet sln add MyProject/MyProject.csproj
dotnet sln add MyProject.Test/MyProject.Test.csproj
dotnet add reference ../MyProject/MyProject.csproj

dotnet sln todo.sln add **/*.csproj
dotnet sln remove **/*.csproj
```