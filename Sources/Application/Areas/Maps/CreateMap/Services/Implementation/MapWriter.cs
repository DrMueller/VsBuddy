using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using VsBuddy.Areas.Maps.CreateMap.Services.Servants;
using VsBuddy.Infrastructure.Roslyn.ClassInformations.Services;
using VsBuddy.Infrastructure.SolutionMetadata.Services;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace VsBuddy.Areas.Maps.CreateMap.Services.Implementation
{
    public class MapWriter : IMapWriter
    {
        private readonly IClassInformationFactory _classInfoFactory;
        private readonly IMapClassBuilder _mapClassBuilder;
        private readonly IVsSolutionFactory _vsSolutionFactory;

        public MapWriter(IVsSolutionFactory vsSolutionFactory,
            IClassInformationFactory classInfoFactory,
            IMapClassBuilder mapClassBuilder)
        {
            _vsSolutionFactory = vsSolutionFactory;
            _classInfoFactory = classInfoFactory;
            _mapClassBuilder = mapClassBuilder;
        }

        public void CreateMap(string filePath)
        {
            var vsSolution = _vsSolutionFactory.Create(filePath);
            var csProj = vsSolution.SearchCsProjByPath(filePath);
            var classInfo = _classInfoFactory.Create(filePath, csProj);

            var mapClassName = $"{classInfo.ClassName}Map";

            var cu = CompilationUnit();
            var @namespace = NamespaceDeclaration(IdentifierName(classInfo.NamespaceDecl));
            var @class = ClassDeclaration(mapClassName).WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)));

            var props = _mapClassBuilder.CreateProperties(classInfo);
            @class = @class.WithMembers(props);

            var mapMethod = _mapClassBuilder.CreateMapMethod(classInfo, mapClassName);
            @class = @class.AddMembers(mapMethod);

            @namespace = @namespace.AddMembers(@class);
            cu = cu.AddMembers(@namespace);

            var str = cu.NormalizeWhitespace().ToFullString();

            var fileName = $"{filePath.Replace(".cs", string.Empty)}Map.cs";
            File.WriteAllText(fileName, str);
        }
    }
}