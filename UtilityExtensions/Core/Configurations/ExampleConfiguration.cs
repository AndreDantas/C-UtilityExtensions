using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityExtensions.Core.Configurations.Attributes;

namespace UtilityExtensions.Core.Configurations
{
    public class ExampleConfiguration : Configuration
    {
        public override string Name => "Example";

        [StringConfigProperty("DefaultValue", "A String value")]
        public string MyStringValue { get; set; }
    }
}