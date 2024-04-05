using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Infrastructure.Roslyn.ClassInformations.Models;
using VsBuddy.Infrastructure.Types.Maybes;

namespace VsBuddy.Areas.CreateBlazorUnitTests.SubAreas.ClassContentCreation.Services.Servants.Implementation
{
    public class BlazorClassBuilder : IBlazorClassBuilder
    {
        private ClassDeclarationSyntax _classDeclaration;
        private ClassInformation _classInfo;

        public IBlazorClassBuilder AppendConstructor()
        {
            var statements = new List<StatementSyntax>();
            var sb = new StringBuilder();

            var ctor = _classInfo.Constructor.Reduce(Constructor.CreateEmpty);

            foreach (var ctorParam in ctor.Parameters)
            {
                statements.Add(
                    SyntaxFactory.ParseStatement($"_{ctorParam.ParameterName}Mock= new Mock<{ctorParam.ParameterType}>();"));
            }

            foreach (var injection in _classInfo.Injections)
            {
                statements.Add(
                    SyntaxFactory.ParseStatement($"_{injection.ParameterName}Mock= new Mock<{injection.ParameterType}>();"));
            }
            
            statements.Add(SyntaxFactory.ParseStatement(string.Empty).WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed));
            
            foreach (var injection in _classInfo.Injections)
            {
                statements.Add(
                    SyntaxFactory.ParseStatement($"Services.AddSingleton(_{injection.ParameterName}Mock.Object);"));
            }

            statements.Add(SyntaxFactory.ParseStatement(string.Empty).WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed));

            statements.Add(
                SyntaxFactory.ParseStatement("Services.AddJsServices();"));

            statements.Add(
                SyntaxFactory.ParseStatement("Services.AddBlazorBootstrap();"));

            statements.Add(
                SyntaxFactory.ParseStatement("JSInterop.Mode = JSRuntimeMode.Loose;"));

            statements.Add(SyntaxFactory.ParseStatement(string.Empty).WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed));
            
            sb.AppendLine($"_cut = RenderComponent<{_classInfo.ClassName}>();");

            var str = sb.ToString();
            statements.Add(SyntaxFactory.ParseStatement(str));

            var setupMethod = SyntaxFactory.ConstructorDeclaration(_classDeclaration.Identifier.ToString());

            setupMethod = setupMethod
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block(statements));

            _classDeclaration = _classDeclaration.AddMembers(setupMethod);

            return this;
        }

        public IBlazorClassBuilder AppendExamplaryMethod()
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

        public IBlazorClassBuilder AppendFields()
        {
            var ctor = _classInfo.Constructor.Reduce(Constructor.CreateEmpty);

            foreach (var param in ctor.Parameters)
            {
                _classDeclaration = _classDeclaration.AddMembers(
                    CreatePrivateField($"Mock<{param.ParameterType}>", $"_{param.ParameterName}Mock", true));
            }

            foreach (var inj in _classInfo.Injections)
            {
                _classDeclaration = _classDeclaration.AddMembers(
                    CreatePrivateField($"Mock<{inj.ParameterType}>", $"_{inj.ParameterName}Mock", true));
            }

            var sutField = SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.GenericName(
                                    SyntaxFactory.Identifier("IRenderedComponent"))
                                .WithTypeArgumentList(
                                    SyntaxFactory.TypeArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                            SyntaxFactory.IdentifierName(_classInfo.ClassName)))))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("_cut")))))
                .WithModifiers(
                    SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword), SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword)));

            _classDeclaration = _classDeclaration.AddMembers(sutField);

            return this;
        }

        public ClassDeclarationSyntax Build()
        {
            return _classDeclaration;
        }

        public IBlazorClassBuilder Initialize(ClassInformation classInfo)
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

            _classDeclaration = _classDeclaration.WithBaseList(
                SyntaxFactory.BaseList(
                    SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(
                        SyntaxFactory.SimpleBaseType(
                            SyntaxFactory.IdentifierName("TestContext")))));
        }
    }
}