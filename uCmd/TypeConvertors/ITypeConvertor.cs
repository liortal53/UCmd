using System;

namespace Cmd
{
    interface ITypeConvertor
    {
        /// <summary>
        /// Converts a string value into another type (which is returned as System.object reference).
        /// </summary>
        object Convert(string value, Type type);
    }
}