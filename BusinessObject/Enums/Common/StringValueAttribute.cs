using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Enums.Common
{
    public class StringValueAttribute : Attribute
    {
        public string? StringValue { get; set; }
        public StringValueAttribute(string stringValue)
        {
            StringValue = stringValue;
        }
    }
}
