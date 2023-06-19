﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Zero.Extension
{
    public static class DataTableExtension
    {
        public static List<T> ToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                T tempT = new T();
                var tType = tempT.GetType();
                List<T> list = new List<T>();
                foreach (var row in table.Rows.Cast<DataRow>())
                {
                    T obj = new T();
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        var propertyInfo = tType.GetProperty(prop.Name);
                        var rowValue = row[prop.Name];
                        var t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                        object safeValue = (rowValue == null || DBNull.Value.Equals(rowValue)) ? null : Convert.ChangeType(rowValue, t);
                        propertyInfo.SetValue(obj, safeValue, null);
                    }
                    list.Add(obj);
                }
                return list;
            }
            catch
            {
                return new List<T>();
            }
        }
    }
}