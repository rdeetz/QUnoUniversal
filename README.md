# QUno

An Uno-like card game.

## Requirements

* Windows 11 or Windows 10
* [Windows SDK](https://developer.microsoft.com/en-US/windows/downloads/windows-sdk/)
* [Visual Studio 2022](https://visualstudio.microsoft.com/) (I use the Community edition, v17)
* Your favorite editor (my favorite editor is [Visual Studio Code](https://code.visualstudio.com/))

## How To Play

`QUnoUniversal` is a Universal Windows Platform application. Follow along in the user interface. 
You can save games to a file and load them later to resume. The file format of saved games is the 
same as used by the applications in the [QUnoDesktop](https://github.com/rdeetz/QUnoDesktop) repository.

## Developer Notes

This repository includes a straightforward Universal Windows Platform application 
implemented in C#. Use Visual Studio 2022 for best results.

* `QUnoObjects` contains the game engine. This is a Windows Runtime Component built from 
the same code as `QUnoLibrary` in the [QUnoEngine](https://github.com/rdeetz/QUnoEngine) repository. 
(Universal Windows Platform applications cannot currently consume .NET assemblies.)
* `QUnoUniversal` contains a Universal Windows Platform application. It aspires 
to follow the Model-View-ViewModel pattern but I found it challenging when targeting the 
Windows Runtime.

For example, `StandardGameSerializer` cannot be used in a Universal Windows Application 
because it creates files from a string file name. Thus this application defines a new 
interface `IUniversalGameSerializer` that allows for using `Windows.Storage` APIs.
