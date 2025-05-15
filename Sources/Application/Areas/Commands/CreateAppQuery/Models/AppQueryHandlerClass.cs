using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Areas.Commands.Shared.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace VsBuddy.Areas.Commands.CreateAppQuery.Models
{
    public class AppQueryHandlerClass : ClassMetadata
    {
        private readonly string _queryClassName;
        private readonly string _resultClassName;

        public AppQueryHandlerClass(
            string @namespace,
            string resultClassName,
            string queryClassName) : base(@namespace, "QueryHandler")
        {
            _resultClassName = resultClassName;
            _queryClassName = queryClassName;
        }

        protected override CompilationUnitSyntax CreateCompilationUnit()
        {
            return CompilationUnit()
                .WithUsings(
                    SingletonList(UsingDirective(IdentifierName("MediatR"))))
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        NamespaceDeclaration(IdentifierName(Namespace))
                            .WithMembers(
                                SingletonList<MemberDeclarationSyntax>(
                                    ClassDeclaration(ClassName)
                                        .WithModifiers(
                                            TokenList(
                                                Token(SyntaxKind.PublicKeyword)))
                                        .WithBaseList(
                                            BaseList(
                                                SingletonSeparatedList<BaseTypeSyntax>(
                                                    SimpleBaseType(
                                                        GenericName(
                                                                Identifier("IRequestHandler"))
                                                            .WithTypeArgumentList(
                                                                TypeArgumentList(
                                                                    SeparatedList<TypeSyntax>(
                                                                        new SyntaxNodeOrToken[]
                                                                        {
                                                                            IdentifierName(_queryClassName), Token(SyntaxKind.CommaToken), GenericName(
                                                                                    Identifier("Either"))
                                                                                .WithTypeArgumentList(
                                                                                    TypeArgumentList(
                                                                                        SeparatedList<TypeSyntax>(
                                                                                            new SyntaxNodeOrToken[] { IdentifierName("InformationEntries"), Token(SyntaxKind.CommaToken), IdentifierName(_resultClassName) })))
                                                                        })))))))
                                        .WithMembers(
                                            SingletonList<MemberDeclarationSyntax>(
                                                MethodDeclaration(
                                                        GenericName(
                                                                Identifier("Task"))
                                                            .WithTypeArgumentList(
                                                                TypeArgumentList(
                                                                    SingletonSeparatedList<TypeSyntax>(
                                                                        GenericName(
                                                                                Identifier("Either"))
                                                                            .WithTypeArgumentList(
                                                                                TypeArgumentList(
                                                                                    SeparatedList<TypeSyntax>(
                                                                                        new SyntaxNodeOrToken[] { IdentifierName("InformationEntries"), Token(SyntaxKind.CommaToken), IdentifierName(_resultClassName) })))))),
                                                        Identifier("Handle"))
                                                    .WithModifiers(
                                                        TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.AsyncKeyword)))
                                                    .WithParameterList(
                                                        ParameterList(
                                                            SeparatedList<ParameterSyntax>(
                                                                new SyntaxNodeOrToken[]
                                                                {
                                                                    Parameter(
                                                                            Identifier("request"))
                                                                        .WithType(
                                                                            IdentifierName(_queryClassName)),
                                                                    Token(SyntaxKind.CommaToken), Parameter(
                                                                            Identifier("cancellationToken"))
                                                                        .WithType(
                                                                            IdentifierName("CancellationToken"))
                                                                })))
                                                    .WithBody(
                                                        Block(
                                                            SingletonList<StatementSyntax>(
                                                                ReturnStatement(
                                                                    MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        IdentifierName("InformationEntries"),
                                                                        IdentifierName("Empty"))))))))))))
                .NormalizeWhitespace();
        }
    }
}