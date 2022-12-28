namespace Auxquimia
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Assigns a string value to a Enum through atribute
    /// </summary>
    public class StringValueAttribute : System.Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringValueAttribute"/> class.
        /// </summary>
        /// <param name="value">The value<see cref="string"/></param>
        public StringValueAttribute(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the Value
        /// </summary>
        public string Value { get; }
    }

    /// <summary>
    /// Defines the <see cref="StringEnum" />
    /// </summary>
    public static class StringEnum
    {
        /// <summary>
        /// The GetStringValue
        /// </summary>
        /// <param name="value">The value<see cref="Enum"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();

            //Check first in our cached results...

            //Look for our 'StringValueAttribute'

            //in the field's custom attributes

            FieldInfo fi = type.GetField(value.ToString());
            StringValueAttribute[] attrs =
               fi.GetCustomAttributes(typeof(StringValueAttribute),
                                       false) as StringValueAttribute[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }
    }
}
