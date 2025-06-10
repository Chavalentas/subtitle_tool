using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_lib
{
    public interface IFileReader
    {
        string[] ReadFile(string filePath);
    }
}
