using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using UtilityExtensions.Core.Configurations.Attributes;

namespace UtilityExtensions.Core.Configurations
{
    public abstract class Configuration
    {
        public struct Data
        {
            public struct ValueInfo
            {
                public string displayName;
                public string value;
                public Type type;

                public ValueInfo(string displayName, string value, Type type)
                {
                    this.displayName = displayName;
                    this.value = value;
                    this.type = type;
                }
            }

            public string name;

            public Dictionary<string, ValueInfo> values;

            public Data(string name)
            {
                this.name = name;
                values = new Dictionary<string, ValueInfo>();
            }

            public DataTable ToDataTable()
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Key");
                dt.Columns.Add("DisplayName");
                dt.Columns.Add("Type");
                dt.Columns.Add("Value");

                if (values == null)
                {
                    return dt;
                }

                foreach (string key in values.Keys)
                {
                    DataRow dr = dt.NewRow();
                    dr["Key"] = key;
                    dr["DisplayName"] = string.IsNullOrEmpty(values[key].displayName) ? key : values[key].displayName;
                    dr["Type"] = values[key].type;
                    dr["Value"] = values[key].value;
                    dt.Rows.Add(dr);
                }

                return dt;
            }
        }

        public abstract string Name { get; }

        public virtual Data ToData()
        {
            Data configData = new Data(Name);

            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                if (!prop.CanWrite || !prop.CanRead)
                {
                    continue;
                }

                object[] attributes = prop.GetCustomAttributes(typeof(ConfigPropertyAttribute), true);
                if (attributes.Length == 1)
                {
                    ConfigPropertyAttribute attr = (ConfigPropertyAttribute)attributes[0];
                    string displayName = string.IsNullOrEmpty(attr.displayName) ? prop.Name : attr.displayName;
                    configData.values[prop.Name] = new Data.ValueInfo(displayName, attr.ConvertToString(prop.GetValue(this, null)), attr.type);
                }
            }

            return configData;
        }

        public virtual void FromData(Data data)
        {
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                if (!prop.CanWrite || !prop.CanRead)
                {
                    continue;
                }

                object[] attributes = prop.GetCustomAttributes(typeof(ConfigPropertyAttribute), true);
                if (attributes.Length == 1)
                {
                    ConfigPropertyAttribute attr = (ConfigPropertyAttribute)attributes[0];

                    if (data.values?.ContainsKey(prop.Name) ?? false)
                    {
                        prop.SetValue(this, attr.ConvertFromString(data.values[prop.Name].value));
                    }
                    else
                    {
                        prop.SetValue(this, attr.ConvertFromString(attr.@default));
                    }
                }
            }
        }

        protected static string GetValue(string key, Data config, string @default = "")
        {
            if (string.IsNullOrEmpty(key) || config.values == null)
            {
                return @default;
            }

            if (config.values.ContainsKey(key) && !string.IsNullOrEmpty(config.values[key].value))
            {
                return config.values[key].value;
            }

            return @default;
        }
    }
}