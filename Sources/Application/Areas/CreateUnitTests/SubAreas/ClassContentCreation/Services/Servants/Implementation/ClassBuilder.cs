using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Infrastructure.Roslyn.ClassInformations.Models;
using VsBuddy.Infrastructure.Types.Maybes;

namespace VsBuddy.Areas.CreateUnitTests.SubAreas.ClassContentCreation.Services.Servants.Implementation
{
    public class ClassBuilder : IClassBuilder
    {
        private ClassDeclarationSyntax _classDeclaration;
        private ClassInformation _classInfo;

        public IClassBuilder AppendConstructor()
        {
            var statements = new List<StatementSyntax>();
            var sb = new StringBuilder();

            var ctor = _classInfo.Constructor.Reduce(Constructor.CreateEmpty);

            foreach (var ctorParam in ctor.Parameters)
            {
                statements.Add(
                    SyntaxFactory.ParseStatement($"_{ctorParam.ParameterName}= new Mock<{ctorParam.ParameterType}>();"));
            }

            sb.AppendLine($"_sut = new {_classInfo.ClassName}(");

            for (var i = 0; i < ctor.Parameters.Count; i++)
            {
                var ctorParam = ctor.Parameters.ElementAt(i);
                sb.Append($"_{ctorParam.ParameterName}.Object");

                if (i < ctor.Parameters.Count - 1)
                {
                    sb.AppendLine(",");
                }
            }

            sb.AppendLine(");");

            var str = sb.ToString();
            statements.Add(SyntaxFactory.ParseStatement(str));

            var setupMethod = SyntaxFactory.ConstructorDeclaration(_classDeclaration.Identifier.ToString());

            setupMethod = setupMethod
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block(statements));

            _classDeclaration = _classDeclaration.AddMembers(setupMethod);

            return this;
        }

        public IClassBuilder AppendExamplaryMethod()
        {
            var statements = new List<StatementSyntax>
            {
                SyntaxFactory.ParseStatement(string.Empty).WithTrailingTrivia(SyntaxFactory.Comment("// Arrange")),
                SyntaxFactory.ParseStatement(string.Empty).WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement(string.Empty).WithTrailingTrivia(SyntaxFactory.Comment("// Act")),
                SyntaxFactory.ParseStatement(string.Empty).WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxFactory.ParseStatement(string.Empty).WithTrailingTrivia(SyntaxFactory.Comment("// Assert")),
                SyntaxFactory.ParseStatement(string.Empty).WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed)
            };

            var method = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "UnitOfWork_InitialCondition_ExpectedOutcome")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block(statements));

            var attributes = SyntaxFactory.AttributeList(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("Fact"))
                )
            );

            method = method.AddAttributeLists(attributes);

            _classDeclaration = _classDeclaration.AddMembers(method);

            return this;
        }

        public IClassBuilder AppendFields()
        {
            var ctor = _classInfo.Constructor.Reduce(Constructor.CreateEmpty);

            foreach (var param in ctor.Parameters)
            {
                _classDeclaration = _classDeclaration.AddMembers(
                    CreatePrivateField($"Mock<{param.ParameterType}>", "_" + param.ParameterName, true));
            }

            var sutField = CreatePrivateField(
                _classInfo.ClassName,
                "_sut",
                true);

            _classDeclaration = _classDeclaration.AddMembers(sutField);

            return this;
        }

        public ClassDeclarationSyntax Build()
        {
            return _classDeclaration;
        }

        public IClassBuilder Initialize(ClassInformation classInfo)
        {
            _classInfo = classInfo;
            InitializeClassDeclaration();

            return this;
        }

        private static FieldDeclarationSyntax CreatePrivateField(string className, string variableName, bool applyReadOnlyModiefier)
        {
            var variableDeclaration = SyntaxFactory
                .VariableDeclaration(SyntaxFactory.ParseTypeName(className))
                .AddVariables(SyntaxFactory.VariableDeclarator(variableName));

            var field = SyntaxFactory.FieldDeclaration(variableDeclaration)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));

            if (applyReadOnlyModiefier)
            {
                field = field.AddModifiers(SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword));
            }

            return field;
        }

        private void InitializeClassDeclaration()
        {
            _classDeclaration = SyntaxFactory.ClassDeclaration(_classInfo.ClassName + "UnitTests");
            _classDeclaration = _classDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
        }
    }
}