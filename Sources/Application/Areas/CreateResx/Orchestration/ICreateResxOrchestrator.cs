using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsBuddy.Areas.CreateResx.Orchestration
{
    public interface ICreateResxOrchestrator
    {
        void Execute(string filePath);
    }
}
