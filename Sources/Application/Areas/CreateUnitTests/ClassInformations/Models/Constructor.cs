using System.Collections.Generic;

namespace VsBuddy.Areas.CreateUnitTests.ClassInformations.Models
{
    public class Constructor
    {
        public IReadOnlyCollection<Parameter> Parameters { get; }

        public Constructor(IReadOnlyCollection<Parameter> parameters)
        {
            Parameters = parameters;
        }
    }
}