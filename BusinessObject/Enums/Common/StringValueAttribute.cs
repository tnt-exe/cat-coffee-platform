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
