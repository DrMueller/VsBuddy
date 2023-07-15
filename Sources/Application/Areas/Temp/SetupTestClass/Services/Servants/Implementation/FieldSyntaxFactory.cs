using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VsBuddy.Areas.Temp.Common.ClassInformations.Models;

namespace VsBuddy.Areas.Temp.SetupTestClass.Services.Servants.Implementation
{
    public class FieldSyntaxFactory : IFieldSyntaxFactory
    {
        private readonly IValueAligner _valueAligner;

        public FieldSyntaxFactory(IValueAligner valueAligner)
        {
            _valueAligner = valueAligner;
        }

        public IReadOnlyCollection<MemberDeclarationSyntax> CreateFields(ClassInformation classInfo)
        {
            var fieldDeclarations = new List<FieldDeclarationSyntax>
            {
                CreateField(classInfo.ClassName, "_sut")
            };

            var mockFields = classInfo.Constructor.Parameters.Select(param => CreateMockField(param.ParameterType, param.ParameterName));
            fieldDeclarations.AddRange(mockFields);

            return fieldDeclarations;
        }

        private static FieldDeclarationSyntax CreateField(string typeIdenfifier, string variableName)
        {
            return SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName(typeIdenfifier))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier(variableName)))))
                .WithModifiers(
                    SyntaxFactory.TokenList(
                        SyntaxFactory.Token(SyntaxKind.PrivateKeyword),
                        SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword)));
        }

        private FieldDeclarationSyntax CreateMockField(string typeName, string parameterName)
        {
            return SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.GenericName(
                                    SyntaxFactory.Identifier("Mock"))
                                .WithTypeArgumentList(
                                    SyntaxFactory.TypeArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                            SyntaxFactory.IdentifierName(typeName)))))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier(_valueAligner.CreateMockFieldName(parameterName))))))
                .WithModifiers(
                    SyntaxFactory.TokenList(
                        SyntaxFactory.Token(SyntaxKind.PrivateKeyword),
                        SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword)));
        }
    }
}