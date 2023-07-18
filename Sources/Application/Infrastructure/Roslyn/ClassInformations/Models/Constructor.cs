using System.Collections.Generic;

namespace VsBuddy.Infrastructure.Roslyn.ClassInformations.Models
{
    public class Constructor
    {
        public IReadOnlyCollection<Parameter> Parameters { get; }

        public Constructor(IReadOnlyCollection<Parameter> parameters)
        {
            Parameters = parameters;
        }

        public static Constructor CreateEmpty()
        {
            return new Constructor(new List<Parameter>());
        }
    }
}