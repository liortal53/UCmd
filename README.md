# UCmd
Supercharge Unity's command line execution, allowing execution of methods with arguments.

![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)

## Features

[Command Line Execution](#command-line-execution) - Run Unity from the command line, execute methods with arguments.

## Quick Start Guide

### Installation

#### Unity Asset Store
Install directly from the [Unity Asset Store](https://assetstore.unity.com/packages/tools/utilities/ucmd-125946) (FREE). 

#### Installing from GitHub Repo

1. Visit the [Releases page](https://github.com/liortal53/UCmd/releases) and download the latest `UCmd.dll`.
2. Open Unity and place this file in an Editor folder.

### Usage

#### Command Line Execution
Unity can be launched from the [command line](https://docs.unity3d.com/Manual/EditorCommandLineArguments.html). This is useful for CI/CD cases (automatically building your project on a build server).

It works by supplying the name of a static method using the *-executeMethod* argument:

    Unity -executeMethod MyClass.MyStaticMethod

*MyStaticMethod* must be defined without any arguments, so if the build requires any additional data, it must be extract from the command line arguments inside this method:

```csharp
private static void MyStaticMethod()
{
   // Extract any additional arguments from the command line
   var platform = GetOptionFromCommandLine("platform");
   var data = GetOptionFromCommandLine("data");
}
```

With UCmd, the same syntax is used, but the method executed is UCmd.Run:

    Unity -executeMethod UCmd.Run MyClass.MyStaticMethod

Now it's possible to execute methods that take additional arguments.

UCmd acts as a "middle man", using reflection to invoke the required method and passing it the arguments from the command line:

    Unity -executeMethod UCmd.Run MyClass.MyStaticMethod "Android" "1.0.0" "data"
And the method definition:

```csharp
private static void MyStaticMethod(string platform, string version, string data)
{
  // Build code here
}
```

## Contact
For support or any additional feature requests, please report an issue, or conact me directly: liortal53@gmail.com
