using System.Collections.Generic;
using System.Linq;

namespace VsBuddy.Infrastructure.SolutionMetadata.Models
{
    public class VsSolution
    {
        private static readonly IReadOnlyCollection<string> _namespacesToIgnore = new
            List<string>
            {
                "Analyzers",
                "CodeAnalysis",
                "Microsoft.NET.Test.Sdk"
            };

        public VsSolution(IReadOnlyCollection<CsProj> projects)
        {
            Projects = projects;
        }

        public IReadOnlyCollection<CsProj> Projects { get; }

        public CsProj SearchUnitTestsByClassNamespace(string classNamespace)
        {
            var split = classNamespace.Split('.');

            for (var i = split.Length; i > 0; i--)
            {
                var tryPart = string.Join(".", split.Take(i));
                var searchName = $"{tryPart}.UnitTests";

                var csProj = Projects.FirstOrDefault(p => p.AssemblyName == searchName);

                if (csProj != null)
                {
                    return csProj;
                }
            }

            throw new System.Exception("Unit tests project not found.");

        }
    }
}