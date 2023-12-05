using System.IO.Abstractions;
using System.Linq;
using VsBuddy.Areas.CreateResx.SubAreas.ResxWriting.Services;
using VsBuddy.Infrastructure.Roslyn.ClassInformations.Services;
using VsBuddy.Infrastructure.SolutionMetadata.Services;

namespace VsBuddy.Areas.CreateResx.Orchestration.Implementation
{
    public class CreateResxOrchestrator : ICreateResxOrchestrator
    {
        private readonly IClassInformationFactory _classInfoFactory;
        private readonly IFileSystem _fileSystem;
        private readonly IResxWriter _resxWriter;
        private readonly IVsSolutionFactory _vsSolutionFactory;

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
            var assetsProj = solution.Projects.Single(f => f.AssemblyName.EndsWith("Assets"));
            var csProj = solution.SearchCsProjByPath(filePath);
            var rootNamespace = assetsProj.AssemblyName.Replace("Assets", string.Empty);

            var classInfo = _classInfoFactory.Create(filePath, csProj);

            var relativeNamespace = classInfo
                .NamespaceDecl
                .Replace(rootNamespace, string.Empty)
                .Replace(".Implementation", string.Empty);

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