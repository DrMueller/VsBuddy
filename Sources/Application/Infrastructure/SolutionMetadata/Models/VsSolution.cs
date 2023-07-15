using System;
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

        public IReadOnlyCollection<CsProj> Projects { get; }

        public VsSolution(IReadOnlyCollection<CsProj> projects)
        {
            Projects = projects;
        }

        public CsProj SearchbyNamespace(
            string classNamespace,
            string extension = null)
        {
            var split = classNamespace.Split('.');

            for (var i = split.Length; i > 0; i--)
            {
                var tryPart = string.Join(".", split.Take(i));

                if (!string.IsNullOrEmpty(extension))
                {
                    tryPart = $"{tryPart}.{extension}";
                }

                var csProj = Projects.FirstOrDefault(p => p.AssemblyName == tryPart);

                if (csProj != null)
                {
                    return csProj;
                }
            }

            throw new Exception($"Project with namespace {classNamespace} not found.");
        }
    }
}