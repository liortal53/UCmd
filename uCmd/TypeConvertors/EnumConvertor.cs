using System;

namespace Cmd
{
    internal class EnumConvertor : ITypeConvertor
    {
        public object Convert(string value, Type type)
        {
            // Support cases where value contains the enum type name as well.
            // In those cases, use only the part after the '.' character.
            int index = value.LastIndexOf('.');

            value = index > -1 ? value.Substring(index + 1) : value;

            return Enum.Parse(type, value, true);
        }
    }
}