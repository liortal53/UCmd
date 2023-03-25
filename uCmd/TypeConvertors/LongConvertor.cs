using System;

namespace Cmd
{
    internal class LongConvertor : ITypeConvertor
    {
        public object Convert(string value, Type type)
        {
            return long.Parse(value);
        }
    }
}