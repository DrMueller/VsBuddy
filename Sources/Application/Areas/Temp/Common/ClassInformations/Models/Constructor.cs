using System.Collections.Generic;

namespace VsBuddy.Areas.Temp.Common.ClassInformations.Models
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