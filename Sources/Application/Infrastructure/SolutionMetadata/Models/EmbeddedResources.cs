using System.Collections.Generic;

namespace VsBuddy.Infrastructure.SolutionMetadata.Models
{
    public class EmbeddedResources
    {
        public EmbeddedResources(IReadOnlyCollection<EmbeddedResource> entries)
        {
            Entries = entries;
        }

        public IReadOnlyCollection<EmbeddedResource> Entries { get; }
    }
}