using System;

namespace UtilityExtensions.Core.Configurations.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class ConfigPropertyAttribute : Attribute
    {
        public abstract Type type { get; }
        public string @default;
        public string displayName;

        public abstract string ConvertToString(object o);

        public abstract object ConvertFromString(string s);

        protected ConfigPropertyAttribute(string @default, string displayName)
        {
            this.@default = @default;
            this.displayName = displayName;
        }
    }
}