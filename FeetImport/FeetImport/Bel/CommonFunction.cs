using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FeetImport.Bel
{
    public class CommonFunction
    {
        /// <summary>
        /// Gets the directory name path that contains currently executing file
        /// </summary>
        /// <returns> Returns the directory information for the specified path.</returns>
        public static string GetDirectoryName()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location );
            return path.Replace(@"\bin\Debug", ""); 
        }
    }
}
