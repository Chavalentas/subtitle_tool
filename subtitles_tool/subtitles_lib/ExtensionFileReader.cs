using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace subtitles_lib
{
    public class ExtensionFileReader : IFileReader
    {
        private string _extension;

        public ExtensionFileReader(string extension)
        {
            Extension = extension;
        }

        public string Extension
        {
            get
            {
                return _extension;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(Extension), "Cannot be null!");
                }

                _extension = value;
            }
        }

        public string[] ReadFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "Cannot be null!");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file \"{filePath}\" could not be found!", filePath);
            }

            string currentExtension = Extension.ToLower();

            if (Path.GetExtension(filePath) != currentExtension)
            {
                throw new ArgumentException($"Wrong extension of the file \"{filePath}\" detected (has to equal {currentExtension})!", nameof(filePath));
            }

            string[] fileLines = File.ReadAllLines(filePath, Encoding.UTF8);
            return fileLines;
        }
    }
}
