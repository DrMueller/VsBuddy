using System;

namespace VsBuddy.Infrastructure.SolutionMetadata.Models
{
    public class NugetPackageReference : IEquatable<NugetPackageReference>
    {
        public string PackageName { get; }

        public NugetPackageReference(
            string packageName)
        {
            PackageName = packageName;
        }

        public bool Equals(NugetPackageReference other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return PackageName == other.PackageName;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((NugetPackageReference)obj);
        }

        public override int GetHashCode()
        {
            return PackageName.GetHashCode();
        }
    }
}