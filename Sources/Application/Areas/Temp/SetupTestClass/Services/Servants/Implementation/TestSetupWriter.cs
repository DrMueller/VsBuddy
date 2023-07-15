using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Areas.Temp.Common.ClassInformations.Models;

namespace VsBuddy.Areas.Temp.SetupTestClass.Services.Servants.Implementation
{
    public class TestSetupWriter : ITestSetupWriter
    {
        private readonly IConstructorSyntaxFactory _ctorSyntaxFactory;
        private readonly IFieldSyntaxFactory _fieldSyntaxFactory;

        public TestSetupWriter(IConstructorSyntaxFactory ctorSyntaxFactory, IFieldSyntaxFactory fieldSyntaxFactory)
        {
            _ctorSyntaxFactory = ctorSyntaxFactory;
            _fieldSyntaxFactory = fieldSyntaxFactory;
        }

        public string WriteSetup(ClassInformation classInfo)
        {
            var ctor = _ctorSyntaxFactory.CreateConstructor(classInfo);
            var fields = _fieldSyntaxFactory.CreateFields(classInfo);

            var allMembers = new List<MemberDeclarationSyntax>();
            allMembers.AddRange(fields);
            allMembers.Add(ctor);

            var compUnit = SyntaxFactory.CompilationUnit()
                .WithMembers(
                    SyntaxFactory.List(allMembers));

            var result = compUnit
                .NormalizeWhitespace()
                .ToFullString();

            return result;
        }
    }
}