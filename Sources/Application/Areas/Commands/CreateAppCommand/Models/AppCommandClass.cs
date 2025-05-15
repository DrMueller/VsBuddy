using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Areas.Commands.Shared.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace VsBuddy.Areas.Commands.CreateAppCommand.Models
{
    public class AppCommandClass : ClassMetadata
    {
        public AppCommandClass(string @namespace) : base(@namespace, "Command")
        {
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
                                                                Identifier("ICommand"))
                                                            .WithTypeArgumentList(
                                                                TypeArgumentList(
                                                                    SingletonSeparatedList<TypeSyntax>(
                                                                        IdentifierName("InformationEntries"))))))))
                                        .WithMembers(
                                            SingletonList<MemberDeclarationSyntax>(
                                                ConstructorDeclaration(
                                                        Identifier(ClassName))
                                                    .WithModifiers(
                                                        TokenList(
                                                            Token(SyntaxKind.PublicKeyword)))
                                                    .WithBody(
                                                        Block())))))))
                .NormalizeWhitespace();
        }
    }
}