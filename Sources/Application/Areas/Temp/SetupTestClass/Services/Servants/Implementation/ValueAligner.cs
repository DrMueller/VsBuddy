namespace VsBuddy.Areas.Temp.SetupTestClass.Services.Servants.Implementation
{
    public class ValueAligner : IValueAligner
    {
        public string CreateMockFieldName(string paramName)
        {
            var fieldName = $"_{paramName}Mock";
            return fieldName;
        }
    }
}