using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsBuddy.Infrastructure.SolutionMetadata.Models;

namespace VsBuddy.Infrastructure.SolutionMetadata.Services
{
    public interface IVsSolutionFactory
    {
        VsSolution Create(string sourceFilePath);
    }
}
