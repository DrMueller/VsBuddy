namespace VsBuddy.Infrastructure.SolutionMetadata.Models
{
    public class EmbeddedResource
    {
        public string DependendantUpon { get; }
        public string UpdatePath { get; }

        public EmbeddedResource(string updatePath, string dependendantUpon)
        {
            UpdatePath = updatePath;
            DependendantUpon = dependendantUpon;
        }
    }
}