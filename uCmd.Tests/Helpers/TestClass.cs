/// <summary>
/// A test class containing static methods to be invoked by unit tests.
/// </summary>
public class TestClass
{
    private static IMethodCallReceiver receiver;

    public static void Init(IMethodCallReceiver receiver)
    {
        TestClass.receiver = receiver;
    }

    public static void MethodNoArgs()
    {
        receiver.Call();
    }

    public static void MethodWithInt(int x)
    {
        receiver.Call(x);
    }

    public static void MethodWithLong(long x)
    {
        receiver.Call(x);
    }

    public static void MethodWithFloat(float x)
    {
        receiver.Call(x);
    }

    public static void MethodWithDouble(double x)
    {
        receiver.Call(x);
    }

    public static void MethodWithBool(bool x)
    {
        receiver.Call(x);
    }

    public static void MethodWithChar(char x)
    {
        receiver.Call(x);
    }

    public static void MethodWithString(string x)
    {
        receiver.Call(x);
    }

    public static void MethodWithEnum(TestEnum x)
    {
        receiver.Call(x);
    }
}