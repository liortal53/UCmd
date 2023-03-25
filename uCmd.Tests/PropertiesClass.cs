using System;

namespace uCmd.Tests
{
    public class PropertiesClass
    {
        public static int x
        {
            get; set;
        }

        public static int y
        {
            get; private set;
        }

        public static TestEnum enumType { get; set; }

        public class Android
        {
            public static float v { get; set; }
        }
    }
}
