using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VsBuddy.Areas.Commands.Shared.Models
{
    public abstract class ClassMetadata
    {
        public string ClassName { get; }
        public string Content => CreateCompilationUnit().ToFullString();

        public string CsFileName => $"{ClassName}.cs";
        protected string Namespace { get; }

        protected ClassMetadata(string @namespace, string classNameExtension)
        {
            Namespace = @namespace;
            ClassName = $"{@namespace.Split('.').Last()}{classNameExtension}";
        }

        protected abstract CompilationUnitSyntax CreateCompilationUnit();
    }
}