using System;

namespace UtilityExtensions.Core.Configurations.Attributes
{
    public class BooleanConfigProperty : ConfigPropertyAttribute
    {
        private string trueValue = "1";
        public override Type type => typeof(bool);

        public BooleanConfigProperty(string @default, string newName = null, string trueValue = "1") : base(@default, newName)
        {
            if (string.IsNullOrEmpty(trueValue))
            {
                throw new ArgumentNullException(nameof(trueValue));
            }

            this.trueValue = trueValue;
        }

        public override object ConvertFromString(string s)
        {
            return (s?.ToLower() == trueValue?.ToLower() || s?.ToLower() == "true");
        }

        public override string ConvertToString(object o)
        {
            return o is bool value ? value ? "1" : "0" : @default;
        }
    }
}