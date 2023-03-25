using Cmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// Executes static methods via reflection
/// </summary>
public class UCmd
{
    /// <summary>
    /// Represents parsed method invocation data (method to run + arguments).
    /// </summary>
    private class ArgumentData
    {
        public readonly string methodName;

        public readonly string[] methodArgs = new string[0];

        public ArgumentData(string methodName, string[] methodArgs)
        {
            this.methodName = methodName;
            this.methodArgs = methodArgs ?? new string[0];
        }
    }

    /// <summary>
    /// Represents method execution data (MethodInfo + arguments array).
    /// </summary>
    private class MethodExecutionData
    {
        public readonly MethodInfo methodInfo;
        public readonly object[] arguments;

        public MethodExecutionData(MethodInfo methodInfo, params object[] arguments)
        {
            if (methodInfo == null || !methodInfo.IsStatic)
            {
                throw new ArgumentException("MethodInfo is null, or does not represent a static method!");
            }

            this.methodInfo = methodInfo;
            this.arguments = arguments;
        }

        public void Execute()
        {
            methodInfo.Invoke(null, arguments);
        }
    }

    private const string UCMD_METHOD_NAME = "UCmd.Run";

    private static readonly Dictionary<Type, ITypeConvertor> typeConvertors = new Dictionary<Type, ITypeConvertor>
    {
        { typeof(int), new IntConvertor() },
        { typeof(bool), new BoolConvertor() },
        { typeof(char), new CharConvertor() },
        { typeof(float), new FloatConvertor() },
        { typeof(double), new DoubleConvertor() },
        { typeof(long), new LongConvertor() },
        { typeof(Enum), new EnumConvertor() },
        { typeof(string), new StringConvertor() }
    };

    /// <summary>
    /// Tries to find the array index of a given value inside the args array.
    /// </summary>
    /// <returns>The arg index (if found), or -1 if the value was not found.</returns>
    private static int GetArgIndex(string[] args, string name)
    {
        var result = -1;

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name)
            {
                result = i;

                break;
            }
        }

        return result;
    }

    /// <summary>
    /// Sets all static properties on Unity's <see cref="UnityEditor.PlayerSettings"/> class.
    /// Properties are matches from the given command line arguments (by name - case sensitive!).
    /// </summary>
    /// <example>Unity.exe bundleVersion 1.10 will set PlayerSettings.bundleVersion to 1.10</example>
    public static void SetPlayerSettings()
    {
        var args = Environment.GetCommandLineArgs();

        SetPlayerSettings(args);
    }

    /// <summary>
    /// Sets all static properties on Unity's PlayerSettings class from the given arguments.
    /// </summary>
    /// <remarks>Properties are matched by their names (CASE SENSITIVE!).</remarks>
    /// <param name="args">An array of string arguments and their values.</param>
    public static void SetPlayerSettings(string[] args)
    {
        SetStaticProperties("UnityEditor.PlayerSettings", args);
    }

    public static void SetStaticProperties(string className, string[] args)
    {
        if (string.IsNullOrEmpty(className))
        {
            throw new ArgumentException("className");
        }

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var types = assemblies.SelectMany(a => a.GetTypes().Where(t => t.FullName.StartsWith(className)));

        if (types == null)
        {
            throw new ArgumentException("Class with the given name was not found", className);
        }

        foreach (var type in types)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static).Where(p => p.CanWrite).ToArray();

            foreach (var p in properties)
            {
                var name = p.Name;
                var argIndex = GetArgIndex(args, name);

                // Make sure we have enough arguments (name + value)
                if (argIndex != -1 && args.Length >= argIndex + 2)
                {
                    var value = ConvertParameter(args[argIndex + 1], p.PropertyType);

                    Console.WriteLine("Setting property {0}.{1} to: {2}", type.FullName, p.Name, value);

                    p.SetValue(null, value, null);
                }
            }
        }
    }

    /// <summary>
    /// Executes the method specified as command line arguments with its specified arguments.
    /// This method is meant to be called from the command line via the -executeMethod argument
    /// For more information, see the Unity manual page: 
    /// </summary>
    public static void Run()
    {
        UCmd cmd = new UCmd();
        cmd.RunWithCommandLineArgs(Environment.GetCommandLineArgs());
    }

    /// <summary>
    /// Attempts to execute the method with the given command line arguments.
    /// </summary>
    /// <param name="args"></param>
    public void RunWithCommandLineArgs(string[] args)
    {
        // Validate + parse arguments
        var argumentData = ParseArguments(args);

        // Determine what method to run
        var executionData = CreateMethodExecutionData(argumentData);

        // Run the method
        executionData.Execute();
    }

    /// <summary>
    /// Validates the passed arguments
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    private ArgumentData ParseArguments(string[] args)
    {
        if (args == null)
        {
            throw new ArgumentNullException("args", "args parameter should not be null!");
        }

        if (args.Length < 3)
        {
            throw new ArgumentException("args", "args should have at least 3 elements!");
        }

        // See if -executeMethod is the last thing in args
        var index = GetArgIndex(args, "-executeMethod");

        if (index == -1)
        {
            throw new ArgumentException("-executeMethod was not found in arguments!");
        }

        // check that there are enough arguments
        if (args.Length < index + 2)
        {
            throw new ArgumentException("Not enough arguments were given! Please run with -executeMethod UCmd.Run [method] [args]");
        }

        if (args[index + 1] != UCMD_METHOD_NAME)
        {
            throw new ArgumentException("did not launch uCmd!");
        }

        return new ArgumentData(args[index + 2], args.Skip(index + 3).ToArray());
    }

    private MethodExecutionData CreateMethodExecutionData(ArgumentData argumentData)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var staticMethods = assemblies.SelectMany(asm => asm.GetTypes()).SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));

        // Filter to the methods that actually match also by arguments
        var matches = staticMethods.Where(mi => IsMatching(mi, argumentData)).ToArray();

        if (matches.Length == 0)
        {
            throw new ArgumentException(string.Format("No static method named '{0}' with {1} arguments was found!", argumentData.methodName, argumentData.methodArgs.Length));
        }
        else if (matches.Length > 1)
        {
            throw new ArgumentException("Ambiguous matches found. More than 1 method matches the passed in arguments!");
        }

        var method = matches[0];

        var types = method.GetParameters().Select(p => p.ParameterType).ToArray();
        object[] parameters = new object[types.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            parameters[i] = ConvertParameter(argumentData.methodArgs[i], types[i]);
        }

        return new MethodExecutionData(method, parameters);
    }

    /// <summary>
    /// Converts the given string value to the given Type and returns it.
    /// </summary>
    private static object ConvertParameter(string v, Type type)
    {
        if (typeConvertors.ContainsKey(type))
        {
            var convertor = typeConvertors[type];

            return convertor.Convert(v, type);
        }
        
        // Iterate of type convertors, look for a type that is assignable from the parameter 'type'
        foreach (var supportedType in typeConvertors.Keys)
        {
            if (supportedType.IsAssignableFrom(type))
            {
                var convertor = typeConvertors[supportedType];

                return convertor.Convert(v, type);
            }
        }

        // No type convertor was found, throw an exception.
        throw new InvalidOperationException("Cannot find a type convertor for: " + v);
    }

    private bool IsMatching(MethodInfo mi, ArgumentData args)
    {
        if (mi == null)
        {
            return false;
        }

        // Construct the full name (namespace.type.method) from the MethodInfo object.
        var fullName = GetFullName(mi);

        if (fullName != args.methodName)
        {
            return false;
        }

        // Check arguments (number and parameter types)
        var parameters = mi.GetParameters();

        if (parameters.Length != args.methodArgs.Length)
        {
            return false;
        }

        for (int i = 0; i < parameters.Length; i++)
        {
            if (!IsSupportedParameterType(parameters[i]))
            {
                return false;
            }

            if (!CanConvertToType(parameters[i].ParameterType))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Returns a boolean value indicating whether it's possible to convert
    /// the given (string) value to the given type.
    /// </summary>
    private bool CanConvertToType(Type type)
    {
        return typeConvertors.Keys.Any(t => t.IsAssignableFrom(type));
    }

    private bool IsSupportedParameterType(ParameterInfo parameterInfo)
    {
        if (parameterInfo == null)
        {
            return false;
        }

        var type = parameterInfo.ParameterType;

        return type == typeof(string) || type.IsPrimitive || type.IsEnum;
    }

    /// <summary>
    /// Returns a string representing the full name (including namespace, class and method names)
    /// from the given <see cref="MethodInfo"/> object.
    /// </summary>
    private string GetFullName(MethodInfo mi)
    {
        if (mi == null)
        {
            return null;
        }

        return string.Format("{0}.{1}", mi.DeclaringType.FullName, mi.Name);
    }
}