# ModToolsDecomp

Decompilation of the various executables provided in the Dead Cells ModTools directory

## Compiling

The ModTools directory in this repo is a full VS2019 solution that can be compiled with your run-of-the-mill Visual Studio installation. You can also compile them with a .NET SDK installation with:

```bat
dotnet restore
dotnet build
```

## Disclaimer

This decompilation is provided for educational purposes only. I do not claim ownership of any of the files provided here, nor will I provide any support for the files.

## How?

Dead Cells contains several .NET executables that can help to create mods. Due to the nature of .NET and IL, it's pretty easy to decompile! For this project, DotPeek and dnSpy was used - DotPeek for most of the project exporting and the initial decompilation, and dnSpy for anything that refused to play nice.
