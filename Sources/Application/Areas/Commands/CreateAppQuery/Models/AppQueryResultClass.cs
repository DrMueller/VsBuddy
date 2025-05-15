using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Areas.Commands.Shared.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace VsBuddy.Areas.Commands.CreateAppQuery.Models
{
    public class AppQueryResultClass : ClassMetadata
    {
        public AppQueryResultClass(string @namespace) : base(@namespace, "Entry")
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
                                    RecordDeclaration(
                                            SyntaxKind.RecordDeclaration,
                                            Token(SyntaxKind.RecordKeyword),
                                            Identifier(ClassName))
                                        .WithModifiers(
                                            TokenList(
                                                Token(SyntaxKind.PublicKeyword)))
                                        .WithSemicolonToken(
                                            Token(SyntaxKind.SemicolonToken))))))
                .NormalizeWhitespace();
        }
    }
}