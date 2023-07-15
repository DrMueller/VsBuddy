using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsBuddy.Infrastructure.FilreWriting.Services
{
    public interface IFileWriter
    {
        void WriteFile(string filePath, string content);

        bool CheckVerifyFilePath(string filePath);
    }
}
