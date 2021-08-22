using System;

namespace UtilityExtensions.Core.Configurations.Attributes
{
    public class StringConfigProperty : ConfigPropertyAttribute
    {
        public override Type type => typeof(string);

        public StringConfigProperty(string @default, string newName = null) : base(@default, newName)
        {
        }

        public override object ConvertFromString(string s)
        {
            return s ?? @default;
        }

        public override string ConvertToString(object o)
        {
            return o?.ToString() ?? @default;
        }
    }
}