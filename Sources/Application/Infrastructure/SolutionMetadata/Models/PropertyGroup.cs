namespace VsBuddy.Infrastructure.SolutionMetadata.Models
{
    public class PropertyGroup
    {
        public bool GenerateAssemblyInfo { get; }
        public bool NullableEnabled { get; }

        public PropertyGroup(
            bool nullableEnabled,
            bool generateAssemblyInfo)
        {
            NullableEnabled = nullableEnabled;
            GenerateAssemblyInfo = generateAssemblyInfo;
        }

        public static PropertyGroup CreateEmpty()
        {
            return new PropertyGroup(false, false);
        }
    }
}