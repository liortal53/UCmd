using System;

namespace Cmd
{
    internal class StringConvertor : ITypeConvertor
    {
        public object Convert(string value, Type type)
        {
            return value;
        }
    }
}