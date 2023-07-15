using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsBuddy.Areas.CreateResx.SubAreas.ResxWriting
{
    public interface IResxWriter
    {
        void WriteEmptyResx(string targetFilePath);
    }
}
