using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VsBuddy.Infrastructure.Roslyn.ClassInformations.Models
{
    public class Property
    {
        public SyntaxToken Identifier { get; }
        public SyntaxToken Modifier { get; }
        public TypeSyntax PropertyType { get; }

        public Property(SyntaxToken modifier, SyntaxToken identifier, TypeSyntax propertyType)
        {
            Modifier = modifier;
            Identifier = identifier;
            PropertyType = propertyType;
        }
    }
}