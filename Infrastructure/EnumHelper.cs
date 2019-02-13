using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WebExtras.Core;

namespace Infrastructure
{
    public static class EnumHelper
    {
        internal static readonly string[] EnumProductColor = GetEnumDescription(typeof(EnumProductColor));

        public static Expression<Func<TSource, string>> CreateEnumToStringExpression<TSource, TMember>(Expression<Func<TSource, TMember>> memberAccess, string defaultValue = "")
        {
            var type = typeof(TMember);

            if (!type.IsEnum)
            {
                throw new InvalidOperationException("TMember must be an Enum type");
            }

            string[] enumNames;

            if (type == typeof(EnumProductColor))
            {
                enumNames = EnumProductColor;
            }
            else
            {
                enumNames = GetEnumDescription(type);
            }

            var enumValues = (TMember[])Enum.GetValues(type);

            var inner = (Expression)Expression.Constant(defaultValue);

            var parameter = memberAccess.Parameters[0];

            for (int i = 0; i < enumValues.Length; i++)
            {
                inner = Expression.Condition(
                Expression.Equal(memberAccess.Body, Expression.Constant(enumValues[i])),
                Expression.Constant(enumNames[i]),
                inner);
            }

            var expression = Expression.Lambda<Func<TSource, string>>(inner, parameter);

            return expression;
        }

        public static string[] GetEnumDescription(Type enumType)
        {
            var result = new List<string>();

            foreach (var e in Enum.GetValues(enumType))
            {
                var fi = e.GetType().GetField(e.ToString());
                var attributes = (StringValueAttribute[])fi.GetCustomAttributes(typeof(StringValueAttribute), false);

                if (attributes.Length > 0)
                {
                    result.Add(attributes[0].Value);
                }
            }

            return result.ToArray();
        }
    }

    public class EnumParser<T> where T : struct, IConvertible
    {
        public static T? GetEnumFromStringValue(string stringValue)
        {
            var type = typeof(T);

            if (!type.IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            var enums = GetEnumValues(type);

            if (enums.ContainsKey(stringValue))
                return GetEnumValues(type)[stringValue];

            return null;
        }

        public static Dictionary<string, T> GetEnumValues(Type enumType)
        {
            var result = new Dictionary<string, T>();

            foreach (var e in Enum.GetValues(enumType))
            {
                var fi = e.GetType().GetField(e.ToString());
                var attributes = (StringValueAttribute[])fi.GetCustomAttributes(typeof(StringValueAttribute), false);

                if (attributes.Length > 0)
                {
                    result.Add(attributes[0].Value, (T)e);
                }
            }

            return result;
        }
    }
}