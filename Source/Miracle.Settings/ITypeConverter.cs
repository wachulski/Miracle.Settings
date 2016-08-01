﻿using System;

namespace Miracle.Settings
{
    public interface ITypeConverter
    {
        /// <summary>
        /// Check if <param name="values"/> can be converted to type <param name="conversionType"/>
        /// </summary>
        /// <param name="values">Values to convert</param>
        /// <param name="conversionType">Destination type to convert to</param>
        /// <returns>True if type converter is able to convert values to desired type, otherwise false</returns>
        bool CanConvert(object[] values, Type conversionType);

        /// <summary>
        /// Convert <param name="values"/> into instance of type <param name="conversionType"/>
        /// </summary>
        /// <param name="values">Values to convert</param>
        /// <param name="conversionType">Destination type to convert to</param>
        /// <returns>Instance of type <param name="conversionType"/></returns>
        object ChangeType(object[] values, Type conversionType);
    }
}