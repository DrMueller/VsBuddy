using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell.Interop;
using VsBuddy.Areas.CreateResx.SubAreas.ResxWriting;
using VsBuddy.Infrastructure.Roslyn.ClassInformations.Services;
using VsBuddy.Infrastructure.SolutionMetadata.Services;

namespace VsBuddy.Areas.CreateResx.Orchestration.Implementation
{
    public class CreateResxOrchestrator : ICreateResxOrchestrator
    {
        private readonly IVsSolutionFactory _vsSolutionFactory;
        private readonly IResxWriter _resxWriter;
        private readonly IClassInformationFactory _classInfoFactory;
        private readonly IFileSystem _fileSystem;

        public CreateResxOrchestrator(
            IVsSolutionFactory vsSolutionFactory,
            IResxWriter resxWriter,
            IClassInformationFactory classInfoFactory,
            IFileSystem fileSystem)
        {
            _vsSolutionFactory = vsSolutionFactory;
            _resxWriter = resxWriter;
            _classInfoFactory = classInfoFactory;
            _fileSystem = fileSystem;
        }

        public void Execute(string filePath)
        {
            var solution = _vsSolutionFactory.Create(filePath);
            var classInfo = _classInfoFactory.Create(filePath);
            var assetsProj = solution.Projects.Single(f => f.AssemblyName.EndsWith("Assets"));
            var rootNamespace = assetsProj.AssemblyName.Replace("Assets", string.Empty);
            var relativeNamespace = classInfo.NamespaceDecl.Replace(rootNamespace, string.Empty);
            var path = relativeNamespace.Replace(".", "\\");
     
            var resxFileName = $"{classInfo.ClassName}Res.resx";
            var fullpath = _fileSystem.Path.Combine(
                assetsProj.AssemblyPath,
                path,
                resxFileName);

            _resxWriter.WriteEmptyResx(fullpath);
        }
    }
}
