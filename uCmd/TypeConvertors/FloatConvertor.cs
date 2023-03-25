using System;

namespace Cmd
{
    internal class FloatConvertor : ITypeConvertor
    {
        public object Convert(string value, Type type)
        {
            return float.Parse(value);
        }
    }
}