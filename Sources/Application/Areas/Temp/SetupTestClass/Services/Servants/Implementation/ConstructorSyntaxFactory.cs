using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Areas.Temp.Common.ClassInformations.Models;

namespace VsBuddy.Areas.Temp.SetupTestClass.Services.Servants.Implementation
{
    public class ConstructorSyntaxFactory : IConstructorSyntaxFactory
    {
        private readonly IValueAligner _valueAligner;

        public ConstructorSyntaxFactory(IValueAligner valueAligner)
        {
            _valueAligner = valueAligner;
        }

        public ConstructorDeclarationSyntax CreateConstructor(ClassInformation classInfo)
        {
            var statements = CreateConstructorStatements(classInfo);

            var ctor = SyntaxFactory
                .ConstructorDeclaration($"{classInfo.ClassName}UnitTests")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block(statements));

            return ctor;
        }

        private IReadOnlyCollection<StatementSyntax> CreateConstructorStatements(ClassInformation classInfo)
        {
            var result = new List<StatementSyntax>();
            foreach (var param in classInfo.Constructor.Parameters)
            {
                var statement = SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.IdentifierName(_valueAligner.CreateMockFieldName(param.ParameterName)),
                        SyntaxFactory.ObjectCreationExpression(
                                SyntaxFactory.GenericName(
                                        SyntaxFactory.Identifier("Mock"))
                                    .WithTypeArgumentList(
                                        SyntaxFactory.TypeArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                SyntaxFactory.IdentifierName(param.ParameterType)))))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList())));

                result.Add(statement);
            }

            var emptyLine = SyntaxFactory.ParseStatement(string.Empty).WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
            result.Add(emptyLine);

            var sutStatement = CreateSutStatement(classInfo);
            result.Add(sutStatement);

            return result;
        }

        private ExpressionStatementSyntax CreateSutStatement(ClassInformation classInfo)
        {
            var argumentList = new List<ArgumentSyntax>();

            foreach (var param in classInfo.Constructor.Parameters)
            {
                var argument = SyntaxFactory.Argument(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName(_valueAligner.CreateMockFieldName(param.ParameterName)),
                        SyntaxFactory.IdentifierName("Object")));

                argumentList.Add(argument);
            }

            var sutStatement = SyntaxFactory.ExpressionStatement(
                SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.IdentifierName("_sut"),
                    SyntaxFactory.ObjectCreationExpression(
                            SyntaxFactory.IdentifierName(classInfo.ClassName))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList(argumentList)))));

            return sutStatement;
        }
    }
}