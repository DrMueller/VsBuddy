﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace VsBuddy.Infrastructure.SolutionMetadata.Models
{
    public class VsSolution
    {
        public IReadOnlyCollection<CsProj> Projects { get; }

        public VsSolution(IReadOnlyCollection<CsProj> projects)
        {
            Projects = projects;
        }

        public CsProj SearchByNamespace(
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

            throw new Exception($"Project with namespace '{classNamespace}' not found.");
        }

        public CsProj SearchCsProjByPath(
            string filePath)
        {
            var split = filePath.Split('\\');

            for (var i = split.Length; i > 0; i--)
            {
                var tryPart = string.Join("\\", split.Take(i));

                var csProj = Projects.FirstOrDefault(p => p.AssemblyPath == tryPart);

                if (csProj != null)
                {
                    return csProj;
                }
            }

            throw new Exception($"Project with namespace {filePath} not found.");
        }
    }
}