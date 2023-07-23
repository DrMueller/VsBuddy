using System.IO;
using System.Linq;
using System.Xml.Linq;
using VsBuddy.Infrastructure.SolutionMetadata.Models;

namespace VsBuddy.Infrastructure.SolutionMetadata.Services.Implementation
{
    public class CsProjectFactory : ICsProjectFactory
    {
        public CsProj Create(string filePath)
        {
            var xDoc = XDocument.Load(filePath);
            var packageReferences = xDoc.Descendants().Where(f => f.Name == "PackageReference")
                .Select(f => new NugetPackageReference(f.Attribute("Include").Value))
                .ToList();

            var projectReferences = xDoc.Descendants().Where(f => f.Name == "ProjectReference")
                .Select(f => new ProjectReference(f.Attribute("Include").Value))
                .ToList();

            var assemblyName = GetAssemblyName(filePath, xDoc);

            return new CsProj(
                Path.GetDirectoryName(filePath),
                assemblyName,
                packageReferences,
                projectReferences,
                CreatePropertyGroup(xDoc),
                CreateEmbeddedResources(xDoc));
        }

        private static EmbeddedResources CreateEmbeddedResources(XDocument xDoc)
        {
            var resources = xDoc
                .Descendants()
                .Where(f => f.Name == "EmbeddedResource")
                .Select(x => new EmbeddedResource(x.Attribute("Update")?.Value ?? string.Empty, x.Descendants("DependentUpon").SingleOrDefault()?.Value ?? string.Empty))
                .ToList();

            return new EmbeddedResources(resources);
        }

        private static PropertyGroup CreatePropertyGroup(XContainer xDoc)
        {
            var propGroupDoc = xDoc.Descendants().SingleOrDefault(f => f.Name == "PropertyGroup");

            if (propGroupDoc == null)
            {
                return PropertyGroup.CreateEmpty();
            }

            var nullableEnabled = propGroupDoc.Descendants().SingleOrDefault(f => f.Name == "Nullable")?.Value == "enable";
            var generateAssemblyInfoConfig = propGroupDoc.Descendants().SingleOrDefault(f => f.Name == "GenerateAssemblyInfo")?.Value;
            var generateAssemblyInfo = string.IsNullOrEmpty(generateAssemblyInfoConfig) || bool.Parse(generateAssemblyInfoConfig);

            return new PropertyGroup(nullableEnabled, generateAssemblyInfo);
        }

        private static string GetAssemblyName(string filePath, XDocument xDoc)
        {
            var assemblyName = xDoc.Descendants().SingleOrDefault(f => f.Name == "PropertyGroup")?
                .Descendants().SingleOrDefault(f => f.Name == "AssemblyName")?.Value;

            if (string.IsNullOrEmpty(assemblyName))
            {
                assemblyName = Path.GetFileName(filePath).Replace(".csproj", string.Empty);
            }

            return assemblyName;
        }
    }
}