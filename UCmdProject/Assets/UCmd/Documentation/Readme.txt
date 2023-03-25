# UCmd Documentation #

UCmd provides improvements to the way you build your Unity project from the command line:

- Improve the built-in -executeMethod command line argument, by allowing you to run static methods that have arguments.
- Simplifies updating PlayerSettings (such as bundle version or application package name) from the command line.


#### Usage Instructions

1. Run Unity from the command line using the -executeMethod argument. Pass UCmd.Run as the method name.

Example usage: Unity.exe -batchmode -quit -executeMethod UCmd.Run MyClass.RunMe

This will run a static method named RunMe defined on the MyClass class.

public class MyClass
{
	public static void RunMe()
	{
	}
}

2. Any method arguments (optional) should be passed after the class & method names, and should be the last command line arguments.

Example usage: Unity.exe -batchmode -quit -executeMethod UCmd.Run MyClass.RunWithArgs 10 20

This will run a static method named RunWithArgs defined on the MyClass class.


public class MyClass
{
	public static void RunWithArgs(int x, int y)
	{
	}
}

3. Setting PlayerSettings from command line arguments:

See this documentation page for all options in PlayerSettings: https://docs.unity3d.com/ScriptReference/PlayerSettings.html

Example usage: Unity.exe -executeMethod Builder.BuildGame -batchmode -quit bundleVersion 1.10 applicationIdentifier "com.mycompany.mygame"

In your build code, call UCmd.SetPlayerSettings() to automatically apply all values from command line to PlayerSettings:

public static class Builder
{
	public static void BuildGame()
	{
		// This will automatically set any properties on the PlayerSettings class from the supplied command line arguments.
		UCmd.SetPlayerSettings();
	}
}

#### Frequently Asked Questions

- What methods can i execute using UCmd ?

UCmd can only execute static methods that accept either no arguments, or arguments from the following types:
 - bool
 - char
 - int
 - float
 - double
 - long
 - string
 - enum


#### Changelog

v0.3

* Added UCmd.SetPlayerSettings that automatically updates PlayerSettings by the passed command line arguments
* Improved Documentation


v0.2

* Added support for enum parameters