using System;

namespace Cmd
{
    internal class IntConvertor : ITypeConvertor
    {
        public object Convert(string value, Type type)
        {
            return int.Parse(value);
        }
    }
}