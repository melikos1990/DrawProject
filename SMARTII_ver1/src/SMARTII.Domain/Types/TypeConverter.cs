using System;
using System.Collections.Generic;
using System.Reflection;

namespace SMARTII.Domain.Types
{
    public static class TypeConverter
    {
        public static T CastTo<T>(this IDictionary<string, object> source,
         Type DistinctType,
         BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
          where T : class, new()
        {
            T targetObject = Activator.CreateInstance(DistinctType) as T;

            // Go through all bound target object type properties...
            foreach (PropertyInfo property in
                        DistinctType.GetProperties(bindingFlags))
            {
                // ...and check that both the target type property name and its type matches
                // its counterpart in the ExpandoObject

                if (source.ContainsKey(property.Name)
                    && property.PropertyType == source[property.Name]?.GetType())
                {
                    property.SetValue(targetObject, source[property.Name]);
                }
            }

            return targetObject;
        }

        public static TObject CastTo<TObject>(this IDictionary<string, object> source,
            BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
               where TObject : class, new()
        {
            TObject targetObject = new TObject();
            Type targetObjectType = typeof(TObject);

            // Go through all bound target object type properties...
            foreach (PropertyInfo property in
                        targetObjectType.GetProperties(bindingFlags))
            {
                // ...and check that both the target type property name and its type matches
                // its counterpart in the ExpandoObject

                if (source.ContainsKey(property.Name)
                    && property.PropertyType == source[property.Name]?.GetType())
                {
                    property.SetValue(targetObject, source[property.Name]);
                }
            }

            return targetObject;
        }
    }
}