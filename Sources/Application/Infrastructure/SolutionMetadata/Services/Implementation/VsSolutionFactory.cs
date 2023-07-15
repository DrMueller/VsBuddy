using System.Linq;
using VsBuddy.Infrastructure.SolutionMetadata.Models;

namespace VsBuddy.Infrastructure.SolutionMetadata.Services.Implementation
{
    public class VsSolutionFactory : IVsSolutionFactory
    {
        private readonly ICsProjectFilesLocator _csProjLocator;
        private readonly ICsProjectFactory _csProjFactory;

        public VsSolutionFactory(
            ICsProjectFilesLocator csProjLocator,
            ICsProjectFactory csProjFactory)
        {
            _csProjLocator = csProjLocator;
            _csProjFactory = csProjFactory;
        }
        public VsSolution Create(string sourceFilePath)
        {
            var allProjFiles = _csProjLocator.GetAllCsProjFiles(sourceFilePath);

            var projects = allProjFiles
                .Select(f => _csProjFactory.Create(f))
                .ToList();

            return new VsSolution(projects);
        }
    }
}