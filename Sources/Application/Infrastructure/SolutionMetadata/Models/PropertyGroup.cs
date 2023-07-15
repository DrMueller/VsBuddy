namespace VsBuddy.Infrastructure.SolutionMetadata.Models
{
    public class PropertyGroup
    {
        public PropertyGroup(
            bool nullableEnabled,
            bool generateAssemblyInfo)
        {
            NullableEnabled = nullableEnabled;
            GenerateAssemblyInfo = generateAssemblyInfo;
        }

        public bool GenerateAssemblyInfo { get; }
        public bool NullableEnabled { get; }

        public static PropertyGroup CreateEmpty()
        {
            return new PropertyGroup(false, false);
        }
    }
}