using System.Collections.Generic;
using System.Linq;

namespace VsBuddy.Areas.CreateUnitTests.ClassInformations.Models
{
    public class ClassInformation
    {
        private readonly List<UsingEntry> _usingEntries;
        public string ClassName { get; }
        public Constructor Constructor { get; }
        public string NamespaceDecl { get; }
        public IReadOnlyCollection<UsingEntry> SortedUsingEntries => _usingEntries.OrderBy(f => f).ToList();

        public ClassInformation(
            string className,
            string namespaceDecl,
            Constructor constructor,
            List<UsingEntry> usingEntries)
        {
            ClassName = className;
            NamespaceDecl = namespaceDecl;
            Constructor = constructor;
            _usingEntries = usingEntries;
        }

        public void AppendUsing(UsingEntry usingEntry)
        {
            _usingEntries.Add(usingEntry);
        }
    }
}