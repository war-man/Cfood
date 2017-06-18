﻿using System;
using System.ComponentModel;
using System.Linq;

namespace V308CMS.Common
{
    public static class CloneObjectHelper
    {
        public static TConvert CloneTo<TConvert>(this object entity, string[] skipProperties = null) where TConvert : new()
        {
            var convertProperties = TypeDescriptor.GetProperties(typeof(TConvert)).Cast<PropertyDescriptor>();
            var entityProperties = TypeDescriptor.GetProperties(entity).Cast<PropertyDescriptor>();

            var convert = new TConvert();

            foreach (var entityProperty in entityProperties)
            {
                var property = entityProperty;
                var convertProperty = convertProperties.FirstOrDefault(prop => prop.Name == property.Name);
                var isSkipProperty = skipProperties != null && skipProperties.Contains(property.Name) == false;
                if (convertProperty != null && isSkipProperty)
                {
                    convertProperty.SetValue(convert, Convert.ChangeType(entityProperty.GetValue(entity), convertProperty.PropertyType));
                }
            }

            return convert;
        }
    }
}