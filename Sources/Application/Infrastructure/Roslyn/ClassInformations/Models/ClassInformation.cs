using System.Collections.Generic;
using System.Linq;
using VsBuddy.Infrastructure.Types.Maybes;

namespace VsBuddy.Infrastructure.Roslyn.ClassInformations.Models
{
    public class ClassInformation
    {
        private readonly List<UsingEntry> _usingEntries;
        public string ClassName { get; }
        public Maybe<Constructor> Constructor { get; }
        public string NamespaceDecl { get; }
        public IReadOnlyCollection<UsingEntry> SortedUsingEntries => _usingEntries.OrderBy(f => f).ToList();

        public IReadOnlyCollection<Parameter> Injections { get; }
        public IReadOnlyCollection<Property> Properties { get; }

        public ClassInformation(
            string className,
            string namespaceDecl,
            Maybe<Constructor> constructor,
            List<UsingEntry> usingEntries,
            IReadOnlyCollection<Parameter> injections,
            IReadOnlyCollection<Property> properties)
        {
            ClassName = className;
            NamespaceDecl = namespaceDecl;
            Constructor = constructor;
            Injections = injections;
            Properties = properties;
            _usingEntries = usingEntries;
        }

        public void AppendUsing(UsingEntry usingEntry)
        {
            _usingEntries.Add(usingEntry);
        }
    }
}