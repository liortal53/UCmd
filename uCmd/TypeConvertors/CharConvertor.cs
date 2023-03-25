using System;

namespace Cmd
{
    internal class CharConvertor : ITypeConvertor
    {
        public object Convert(string value, Type type)
        {
            return char.Parse(value);
        }
    }
}