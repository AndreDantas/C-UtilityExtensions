using System;
using UtilityExtensions.Extensions;

namespace UtilityExtensions.Core.Configurations.Attributes
{
    public class DateTimeConfigProperty : ConfigPropertyAttribute
    {
        private string format;

        public DateTimeConfigProperty(string @default, string newName = null, string format = "yyyy-MM-dd HH:mm:ss") : base(@default, newName)
        {
            this.format = format;
        }

        public override Type type => typeof(DateTime);

        public override object ConvertFromString(string s)
        {
            return DateTime.TryParse(s, out DateTime result) ? result : DateTime.TryParse(@default, out result) ? result : new DateTime();
        }

        public override string ConvertToString(object o)
        {
            return o is DateTime value ? value.TryToString(format) : @default;
        }
    }
}