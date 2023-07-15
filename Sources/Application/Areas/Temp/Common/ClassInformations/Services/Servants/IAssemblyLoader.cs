using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VsBuddy.Areas.Temp.Common.ClassInformations.Services.Servants
{
    public interface IAssemblyLoader
    {
        Assembly Load(string classFilePath);
    }
}
