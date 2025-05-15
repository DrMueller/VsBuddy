using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Areas.Commands.Shared.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace VsBuddy.Areas.Commands.CreateAppCommand.Models
{
    internal class AppCommandHandlerClass : ClassMetadata
    {
        private readonly string _commandName;

        public AppCommandHandlerClass(string @namespace, string commandName) : base(@namespace, "CommandHandler")
        {
            _commandName = commandName;
        }

        protected override CompilationUnitSyntax CreateCompilationUnit()
        {
            return CompilationUnit()
                .WithUsings(
                    SingletonList(
                        UsingDirective(
                            IdentifierName("MediatR"))))
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
                                                                        new SyntaxNodeOrToken[] { IdentifierName(_commandName), Token(SyntaxKind.CommaToken), IdentifierName("InformationEntries") })))))))
                                        .WithMembers(
                                            SingletonList<MemberDeclarationSyntax>(
                                                MethodDeclaration(
                                                        GenericName(
                                                                Identifier("Task"))
                                                            .WithTypeArgumentList(
                                                                TypeArgumentList(
                                                                    SingletonSeparatedList<TypeSyntax>(
                                                                        IdentifierName("InformationEntries")))),
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
                                                                            IdentifierName(_commandName)),
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