using System.Collections.Generic;

namespace VsBuddy.Infrastructure.SolutionMetadata.Models
{
    public class CsProj
    {
        public string AssemblyName { get; }
        public string AssemblyPath { get; }
        public EmbeddedResources EmbeddedResources { get; }
        public IReadOnlyCollection<NugetPackageReference> NugetReferences { get; }
        public IReadOnlyCollection<ProjectReference> ProjectReferences { get; }
        public PropertyGroup PropertyGroup { get; }

        public CsProj(
            string assemblyPath,
            string assemblyName,
            IReadOnlyCollection<NugetPackageReference> nugetReferences,
            IReadOnlyCollection<ProjectReference> projectReferences,
            PropertyGroup propertyGroup,
            EmbeddedResources embeddedResources)
        {
            AssemblyPath = assemblyPath;
            AssemblyName = assemblyName;
            NugetReferences = nugetReferences;
            ProjectReferences = projectReferences;
            PropertyGroup = propertyGroup;
            EmbeddedResources = embeddedResources;
        }
    }
}