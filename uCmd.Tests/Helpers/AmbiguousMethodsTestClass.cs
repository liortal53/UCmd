
/// <summary>
/// Helper class for testing ambiguous matching of methods. This class has 2 methods that are:
/// 1. Named the same
/// 2. Have 1 parameter that can be ambiguous when converting from a string.
/// </summary>
public class AmbiguousMethodsTestClass
{
    public static void WithNumber(int x)
    {
    }

    public static void WithNumber(float x)
    {
    }
}