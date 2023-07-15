using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VsBuddy.Areas.Temp.Common.ClassInformations.Models
{
    public class ClassInformation
    {
        private readonly List<UsingEntry> _usingEntries;
        public string ClassName { get; }
        public Constructor Constructor { get; }
        public Assembly ContainingAssembly { get; }
        public string NamespaceDecl { get; }
        public IReadOnlyCollection<UsingEntry> SortedUsingEntries => _usingEntries.OrderBy(f => f).ToList();

        public ClassInformation(
            string className,
            string namespaceDecl,
            Constructor constructor,
            List<UsingEntry> usingEntries,
            Assembly containingAssembly)
        {
            ClassName = className;
            NamespaceDecl = namespaceDecl;
            Constructor = constructor;
            ContainingAssembly = containingAssembly;
            _usingEntries = usingEntries;
        }

        public void AppendUsing(UsingEntry usingEntry)
        {
            _usingEntries.Add(usingEntry);
        }
    }
}