using System;

namespace VsBuddy.Areas.CreateUnitTests.ClassInformations.Models
{
    public class UsingEntry : IComparable<UsingEntry>
    {
        public bool IsSystemUsing => Value.ToLowerInvariant().StartsWith("system.");
        public string Value { get; }

        private UsingEntry(string value)
        {
            Value = value;
        }

        public static UsingEntry CreateFrom(string value)
        {
            return new UsingEntry(value);
        }

        public int CompareTo(UsingEntry other)
        {
            if (IsSystemUsing && !other.IsSystemUsing)
            {
                return -1;
            }

            if (!IsSystemUsing && other.IsSystemUsing)
            {
                return 1;
            }

            return string.Compare(Value, other.Value, StringComparison.Ordinal);
        }
    }
}