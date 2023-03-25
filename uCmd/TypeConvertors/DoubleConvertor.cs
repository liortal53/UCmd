using System;

namespace Cmd
{
    internal class DoubleConvertor : ITypeConvertor
    {
        public object Convert(string value, Type type)
        {
            return double.Parse(value);
        }
    }
}