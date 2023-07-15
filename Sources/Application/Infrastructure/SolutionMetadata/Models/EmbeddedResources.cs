using System.Collections.Generic;

namespace VsBuddy.Infrastructure.SolutionMetadata.Models
{
    public class EmbeddedResources
    {
        public IReadOnlyCollection<EmbeddedResource> Entries { get; }

        public EmbeddedResources(IReadOnlyCollection<EmbeddedResource> entries)
        {
            Entries = entries;
        }
    }
}