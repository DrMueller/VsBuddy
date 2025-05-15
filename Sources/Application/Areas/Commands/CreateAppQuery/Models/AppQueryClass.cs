using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Areas.Commands.Shared.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace VsBuddy.Areas.Commands.CreateAppQuery.Models
{
    internal class AppQueryClass : ClassMetadata
    {
        private readonly string _resultClassName;

        public AppQueryClass(string @namespace, string resultClassName) : base(@namespace, "Query")
        {
            _resultClassName = resultClassName;
        }

        protected override CompilationUnitSyntax CreateCompilationUnit()
        {
            return CompilationUnit()
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
                                                                Identifier("IQuery"))
                                                            .WithTypeArgumentList(
                                                                TypeArgumentList(
                                                                    SingletonSeparatedList<TypeSyntax>(
                                                                        GenericName(
                                                                                Identifier("Either"))
                                                                            .WithTypeArgumentList(
                                                                                TypeArgumentList(
                                                                                    SeparatedList<TypeSyntax>(
                                                                                        new SyntaxNodeOrToken[]
                                                                                        {
                                                                                            IdentifierName("InformationEntries"), Token(SyntaxKind.CommaToken), GenericName(
                                                                                                    Identifier("IReadOnlyCollection"))
                                                                                                .WithTypeArgumentList(
                                                                                                    TypeArgumentList(
                                                                                                        SingletonSeparatedList<TypeSyntax>(
                                                                                                            IdentifierName(_resultClassName))))
                                                                                        }))))))))))
                                        .WithSemicolonToken(
                                            Token(SyntaxKind.SemicolonToken))))))
                .NormalizeWhitespace();
        }
    }
}