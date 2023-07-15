namespace VsBuddy.Areas.Temp.ClassWriting.SubAreas.TestFrameworks.Models
{
    public class TestAttribute
    {
        public bool ShouldBeApplied => !string.IsNullOrEmpty(Value);
        public string Value { get; }

        private TestAttribute(string value)
        {
            Value = value;
        }

        public static TestAttribute CreateFrom(string value)
        {
            return new TestAttribute(value);
        }

        public static TestAttribute CreateNone()
        {
            return new TestAttribute(string.Empty);
        }
    }
}