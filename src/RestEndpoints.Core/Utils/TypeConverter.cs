using System;
using System.ComponentModel;

namespace RestEndpoints.Core.Utils
{
    internal static class TypeConverter
    {
        public static bool TryConvert(this string text, Type conversionType, out object value)
        {
            value = null;
            try
            {
                value = TypeDescriptor.GetConverter(conversionType).ConvertFromInvariantString(text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static object Convert(this string text, Type conversionType)
        {
            return TypeDescriptor.GetConverter(conversionType).ConvertFromInvariantString(text);
        }
    }
}