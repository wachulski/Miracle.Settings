﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Miracle.Settings.Properties;

namespace Miracle.Settings
{
    public partial class SettingsLoader
    {
        private delegate bool TypeHandlerDelegate(PropertyInfo propertyInfo, string prefix, string key, out object value);

        private readonly List<TypeHandlerDelegate> _typeHandlers;

        private List<TypeHandlerDelegate> GetTypeHandlers()
        {
            return new List<TypeHandlerDelegate>
            {
                ArrayHandler,
                ListHandler,
                DictionaryHandler,
                DirectGet,
                NestedClassHandler,
            };
        }

        private bool DirectGet(PropertyInfo propertyInfo, string prefix, string key, out object value)
        {
            object propertyValue;
            var list = GetReferencesList(propertyInfo, prefix);
            if (TryGetPropertyValue(propertyInfo, key, out propertyValue))
            {
                list.Add(propertyValue);

                try
                {
                    if (TryConstructPropertyValue(propertyInfo, list.ToArray(), out value))
                        return true;
                }
                catch (SettingsException ex)
                {
                    throw new SettingsException(string.Format(Resources.ConversionErrorSuffix, ex.Message, key));
                }
            }
            value = null;
            return false;
        }

        private bool ArrayHandler(PropertyInfo propertyInfo, string prefix, string key, out object value)
        {
            var propertyType = propertyInfo.PropertyType;
            if (propertyType.IsArray)
            {
                SettingAttribute attribute = propertyInfo.GetCustomAttributes(typeof(SettingAttribute), false).FirstOrDefault() as SettingAttribute;
	            var arguments = attribute?.Separators != null
		            ? new object[] {key, attribute.Separators, attribute.StringSplitOptions}
		            : new object[] {key};

                value = GetType()
                    .GetMethod(nameof(CreateArray), arguments.Select(x=>x.GetType()).ToArray())
                    .MakeGenericMethod(propertyType.GetElementType())
                    .Invoke(this, arguments);

                if (value != null)
                    return true;

                if (TryGetPropertyValue(propertyInfo, key, out value))
                    return true;

                value = null;
                return true;
            }
            value = null;
            return false;
        }

        private bool ListHandler(PropertyInfo propertyInfo, string prefix, string key, out object value)
        {
            var propertyType = propertyInfo.PropertyType;
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().IsGenericTypeDefinitionAssignableFrom(typeof(List<>)))
            {
                SettingAttribute attribute = propertyInfo.GetCustomAttributes(typeof(SettingAttribute), false).FirstOrDefault() as SettingAttribute;
	            var arguments = attribute?.Separators != null
		            ? new object[] {key, attribute.Separators, attribute.StringSplitOptions}
		            : new object[] {key};

                value =
                    GetType()
                        .GetMethod(nameof(CreateList), arguments.Select(x => x.GetType()).ToArray())
                        .MakeGenericMethod(propertyType.GetGenericArguments())
                        .Invoke(this, arguments);

                if (value != null)
                    return true;

                if (TryGetPropertyValue(propertyInfo, key, out value))
                    return true;

                value = null;
                return true;
            }
            value = null;
            return false;
        }

        private bool DictionaryHandler(PropertyInfo propertyInfo, string prefix, string key, out object value)
        {
            var propertyType = propertyInfo.PropertyType;
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().IsGenericTypeDefinitionAssignableFrom(typeof(Dictionary<,>)))
            {
                value =
                    GetType()
                        .GetMethod(nameof(CreateDictionary))
                        .MakeGenericMethod(propertyType.GetGenericArguments())
                        .Invoke(this, new object[] { key, null });

                if (value != null)
                    return true;

                if (TryGetPropertyValue(propertyInfo, key, out value))
                    return true;

                value = null;
                return true;
            }
            value = null;
            return false;
        }

        private bool NestedClassHandler(PropertyInfo propertyInfo, string prefix, string key, out object value)
        {
            SettingAttribute settingAttribute = propertyInfo.GetCustomAttributes(typeof(SettingAttribute), false).FirstOrDefault() as SettingAttribute;
            var propertyType = settingAttribute?.ConcreteType ?? propertyInfo.PropertyType;
            var inline = settingAttribute?.Inline ?? false;
            if (propertyType.IsClass && propertyType != typeof(string))
            {
                var nestedKey = inline ? prefix : key;
                if (IsPropertyOptional(propertyInfo))
                {
                    var hasSettings = (bool)
                        GetType()
                            .GetMethod(nameof(HasSettings))
                            .MakeGenericMethod(propertyType)
                            .Invoke(this, new object[] { nestedKey });

                    if (!hasSettings)
                    {
                        value = null;
                        return false;
                    }
                }

                try
                {
                    value =
                        GetType()
                            .GetMethod(nameof(Create))
                            .MakeGenericMethod(propertyType)
                            .Invoke(this, new object[] {nestedKey});
                    return true;
                }
                // Remove the awfull TargetInvocationException
                catch (System.Reflection.TargetInvocationException ex)
                {
                    if (ex.InnerException != null)
                    {
#if !NET40 // ExceptionDispatchInfo not available before .NET 4.5
                        ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
#else
                        // In .NET 4.0 just throw innerexception (but loose stack trace)
                        throw ex.InnerException;
#endif
                    }
                    throw;
                }
            }
            value = null;
            return false;
        }

	    private List<object> GetReferencesList(PropertyInfo propertyInfo, string prefix)
        {
            var list = new List<object>();

            SettingAttribute attribute = propertyInfo.GetCustomAttributes(typeof (SettingAttribute), false).FirstOrDefault() as SettingAttribute;
            if (attribute?.References != null)
            {
                foreach (var reference in attribute.References)
                {
                    var referenceKey = GetSettingKey(prefix, reference);
                    string referenceValue;
                    if (TryGetValue(referenceKey, out referenceValue))
                    {
                        list.Add(referenceValue);
                    }
                    else
                    {
                        throw new SettingsException(string.Format(Resources.MissingReferenceValueFormat, referenceKey));
                    }
                }
            }
            return list;
        }

        private bool TryGetPropertyValue(PropertyInfo propertyInfo, string key, out object value)
        {
            string stringValue;
            if (TryGetValue(key, out stringValue))
            {
                value = stringValue;
                return true;
            }

            // Get default value if DefaultValueAttribute is present
            DefaultValueAttribute attr = propertyInfo.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault() as DefaultValueAttribute;
            if (attr != null)
            {
                value = attr.Value;
                return true;
            }

            value = null;
            return false;
        }

        private bool TryConstructPropertyValue(PropertyInfo propertyInfo, object[] values, out object value)
        {
            SettingAttribute attribute = propertyInfo.GetCustomAttributes(typeof(SettingAttribute), false).FirstOrDefault() as SettingAttribute;
	        if (attribute != null)
	        {
		        if (attribute.IgnoreValues != null && attribute.IgnoreValues.Any())
		        {
			        var lastValue = values.Last() as string;
					if(attribute.IgnoreValues.Any(x=>x.Equals(lastValue)))
					{
						value = null;
						return true;
					}
		        }
		        if (attribute.TypeConverter != null)
		        {
			        ITypeConverter typeConverter;
			        try
			        {
				        typeConverter = Activator.CreateInstance(attribute.TypeConverter) as ITypeConverter;
			        }
			        catch (Exception ex)
			        {
				        throw new SettingsException(string.Format(Resources.CreateTypeConverterErrorFormat, attribute.TypeConverter), ex);
			        }

			        if (typeConverter == null)
				        throw new SettingsException(string.Format(Resources.BadExplicitTypeConverterTypeFormat, typeof(ITypeConverter)));

			        if (typeConverter.CanConvert(values, propertyInfo.PropertyType))
			        {
				        value = ChangeType(values, propertyInfo.PropertyType, typeConverter);
				        return true;
			        }
			        throw new SettingsException(
				        string.Format(
					        Resources.ExplicitTypeConverterErrorFormat,
					        string.Join(",", values.Select(x => x.ToString())),
					        propertyInfo.PropertyType));
		        }
	        }

	        value = ChangeType(values, propertyInfo.PropertyType);
            return true;
        }
    }
}
