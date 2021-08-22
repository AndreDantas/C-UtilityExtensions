using System;
using System.Globalization;

namespace UtilityExtensions.Core.Configurations.Attributes
{
    public class DecimalConfigProperty : ConfigPropertyAttribute
    {
        public DecimalConfigProperty(string @default, string newName = null) : base(@default, newName)
        {
        }

        public override Type type => typeof(decimal);

        public override object ConvertFromString(string s)
        {
            return decimal.TryParse(s?.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator),
                               NumberStyles.Any,
                               CultureInfo.InvariantCulture,
                               out decimal result) ? result : decimal.TryParse(@default, out result) ? result : 0;
        }

        public override string ConvertToString(object o)
        {
            return o is decimal value ? value.ToString(CultureInfo.InvariantCulture) : @default;
        }
    }
}