using System;

namespace Cmd
{
    internal class BoolConvertor : ITypeConvertor
    {
        public object Convert(string value, Type type)
        {
            return bool.Parse(value);
        }
    }
}