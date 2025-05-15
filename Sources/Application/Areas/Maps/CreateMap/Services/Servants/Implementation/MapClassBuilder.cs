using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Infrastructure.Roslyn.ClassInformations.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace VsBuddy.Areas.Maps.CreateMap.Services.Servants.Implementation
{
    public class MapClassBuilder : IMapClassBuilder
    {
        public MethodDeclarationSyntax CreateMapMethod(ClassInformation classInfo, string mapClassName)
        {
            var assignmentNodes = new List<SyntaxNodeOrToken>();

            foreach (var prop in classInfo.Properties)
            {
                var assignment = AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(prop.Identifier),
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("entry"),
                        IdentifierName(prop.Identifier)));

                assignmentNodes.Add(assignment);
                assignmentNodes.Add(Token(SyntaxKind.CommaToken));
            }

            if (assignmentNodes.Any())
            {
                assignmentNodes.RemoveAt(assignmentNodes.Count - 1);
            }

            var block = Block(
                SingletonList<StatementSyntax>(
                    ReturnStatement(
                        ObjectCreationExpression(
                                IdentifierName(mapClassName))
                            .WithInitializer(
                                InitializerExpression(SyntaxKind.ObjectInitializerExpression,
                                    SeparatedList<ExpressionSyntax>(assignmentNodes))))));

            var paramList = ParameterList(
                SingletonSeparatedList(
                    Parameter(
                            Identifier("entry"))
                        .WithType(
                            IdentifierName(classInfo.ClassName))));

            var mapMethod = MethodDeclaration(
                    IdentifierName(mapClassName),
                    Identifier("Map"))
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)))
                .WithParameterList(paramList)
                .WithBody(block);

            return mapMethod;
        }

        public SyntaxList<MemberDeclarationSyntax> CreateProperties(ClassInformation classInfo)
        {
            var props = new List<PropertyDeclarationSyntax>();

            foreach (var prop in classInfo.Properties)
            {
                var newProp = PropertyDeclaration(
                        prop.PropertyType,
                        prop.Identifier)
                    .WithModifiers(
                        TokenList(Token(SyntaxKind.PublicKeyword)))
                    .WithAccessorList(
                        AccessorList(
                            List(
                                new[]
                                {
                                    AccessorDeclaration(
                                            SyntaxKind.GetAccessorDeclaration)
                                        .WithSemicolonToken(
                                            Token(SyntaxKind.SemicolonToken)),
                                    AccessorDeclaration(
                                            SyntaxKind.InitAccessorDeclaration)
                                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                                })));

                props.Add(newProp);
            }

            return List<MemberDeclarationSyntax>(props);
        }
    }
}