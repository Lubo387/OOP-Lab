using System;
using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils
{
    public static class ModelValidator
    {
        public static ValidationResult Validate(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            ValidationResult result = new ValidationResult();
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object? value = property.GetValue(obj);

                RequiredAttribute? required =
                    property.GetCustomAttribute<RequiredAttribute>();

                if (required != null)
                {
                    bool isEmpty = value == null;

                    if (value is string textValue
                        && string.IsNullOrWhiteSpace(textValue))
                    {
                        isEmpty = true;
                    }

                    if (isEmpty)
                    {
                        result.AddError(
                            property.Name + ": " + required.ErrorMessage);
                    }
                }

                MaxLengthAttribute? maxLength =
                    property.GetCustomAttribute<MaxLengthAttribute>();

                if (maxLength != null && value is string text)
                {
                    if (text.Length > maxLength.Length)
                    {
                        string message = maxLength.ErrorMessage;

                        if (string.IsNullOrWhiteSpace(message))
                        {
                            message = "Maximum length is " +
                                maxLength.Length + " characters.";
                        }

                        result.AddError(property.Name + ": " + message);
                    }
                }

                MinValueAttribute? minValue =
                    property.GetCustomAttribute<MinValueAttribute>();

                if (minValue != null && value != null)
                {
                    try
                    {
                        double number = Convert.ToDouble(value);

                        if (number < minValue.Min)
                        {
                            string message = minValue.ErrorMessage;

                            if (string.IsNullOrWhiteSpace(message))
                            {
                                message = "Minimum value is " + minValue.Min + ".";
                            }

                            result.AddError(property.Name + ": " + message);
                        }
                    }
                    catch (Exception)
                    {
                        result.AddError(
                            property.Name + ": value is not a number.");
                    }
                }
            }

            return result;
        }

        public static void PrintInfo(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            Console.WriteLine(type.Name);
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                Console.Write("  " + property.Name);
                object[] attributes = property.GetCustomAttributes(false);

                foreach (object attribute in attributes)
                {
                    Type attributeType = attribute.GetType();
                    string attributeName = attributeType.Name;

                    if (attributeName.EndsWith("Attribute"))
                    {
                        attributeName = attributeName.Substring(
                            0, attributeName.Length - "Attribute".Length);
                    }

                    Console.Write(" [" + attributeName + "]");
                }

                Console.WriteLine();
            }
        }
    }
}
