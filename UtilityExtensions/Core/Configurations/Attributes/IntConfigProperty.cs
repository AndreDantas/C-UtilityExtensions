using System;

namespace UtilityExtensions.Core.Configurations.Attributes
{
    public class IntConfigProperty : ConfigPropertyAttribute
    {
        public override Type type => typeof(int);

        public IntConfigProperty(string @default, string newName = null) : base(@default, newName)
        {
        }

        public override object ConvertFromString(string s)
        {
            return int.TryParse(s, out int result) ? result : int.TryParse(@default, out result) ? result : 0;
        }

        public override string ConvertToString(object o)
        {
            return o is int value ? value.ToString() : @default;
        }
    }
}