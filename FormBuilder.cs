using System;
using System.Globalization;
using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils
{
    public static class FormBuilder
    {
        public static T Build<T>() where T : new()
        {
            T obj = new T();
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                MethodInfo? setter = property.GetSetMethod();

                if (setter == null || !setter.IsPublic)
                    continue;

                string hint = BuildHint(property);
                bool valueAccepted = false;

                while (!valueAccepted)
                {
                    Console.Write(property.Name + hint + ": ");
                    string input = Console.ReadLine() ?? "";

                    try
                    {
                        object? converted = ConvertInput(
                            input,
                            property.PropertyType);

                        property.SetValue(obj, converted);
                        valueAccepted = true;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(
                            "Некоректне значення, спробуйте ще раз.");
                    }
                }
            }

            return obj;
        }

        private static string BuildHint(PropertyInfo property)
        {
            string hint = "";

            RequiredAttribute? required =
                property.GetCustomAttribute<RequiredAttribute>();

            if (required != null)
                hint += " (обов'язкове)";

            MaxLengthAttribute? maxLength =
                property.GetCustomAttribute<MaxLengthAttribute>();

            if (maxLength != null)
                hint += " (макс. " + maxLength.Length + ")";

            MinValueAttribute? minValue =
                property.GetCustomAttribute<MinValueAttribute>();

            if (minValue != null)
                hint += " (мін. " + minValue.Min + ")";

            return hint;
        }

        private static object? ConvertInput(string input, Type targetType)
        {
            if (targetType == typeof(string))
                return input;

            if (targetType.IsEnum)
                return Enum.Parse(targetType, input, true);

            return Convert.ChangeType(
                input,
                targetType,
                CultureInfo.InvariantCulture);
        }
    }
}
