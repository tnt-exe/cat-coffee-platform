using BusinessObject.Enums.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Enums;

public enum BookingStatus
{
    [StringValueAttribute("None")]
    None = 0,

    [StringValueAttribute("Waiting")]
    OnWaiting = 1,

    [StringValueAttribute("Going")]
    OnGoing = 2,

    [StringValueAttribute("Finished")]
    OnFinished = 3,

    [StringValueAttribute("Cancel")]
    Cancel = 4
}

public static class BookingStatusExtensions
{
    public static string? GetStringValue(this Enum value)
    {
        // Get the type
        Type type = value.GetType();

        // Get fieldinfo for this type
        FieldInfo? fieldInfo = type.GetField(value.ToString());

        // Get the stringvalue attributes
        StringValueAttribute[]? attribs = fieldInfo?.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

        if(attribs is null)
        {
            return null;
        }
        
        // Return the first if there was a match
        return attribs.Length > 0 ? attribs[0].StringValue : null;
    }
}
