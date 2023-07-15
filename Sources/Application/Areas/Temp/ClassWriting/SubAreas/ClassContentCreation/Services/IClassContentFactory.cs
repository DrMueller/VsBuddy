using VsBuddy.Areas.Temp.Common.ClassInformations.Models;

namespace VsBuddy.Areas.Temp.ClassWriting.SubAreas.ClassContentCreation.Services
{
    public interface IClassContentFactory
    {
        string CreateContent(ClassInformation classInfo);
    }
}