using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMTGenerator.Tool.Utils
{
    class FilesUtils
    {

        private const string FolderSelection = "[Folder Selection]";


        /// <summary>
        /// Read contents of an embedded resource file
        /// </summary>
        public static string ReadResourceFile(string filename)
        {
            var thisAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            using (var stream = thisAssembly.GetManifestResourceStream(filename))
            {
                using (var reader = new System.IO.StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        
        /// <summary>
        /// Get the list of all files in a given directory that match the file pattern
        /// </summary>
        /// <param name="directory">Path to the directory</param>
        /// <param name="pattern">Pattern of the files</param>
        /// <returns>Returns the list of all files in that directory, null if error</returns>
        public static string[] GetFiles(string directory, string pattern)
        {
            try
            {
                return System.IO.Directory.GetFiles(directory, pattern);
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Create a file at the given path with the given text
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <param name="text">Content of the file</param>
        public static void WriteAllText(string path, string text)
        {
            try
            {
                System.IO.File.WriteAllText(path, text);
            } catch {}
        }

        /// <summary>
        /// Read the file at the given path
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns>Returns the content of the file or null if there is an error</returns>
        public static string ReadAllText(string path)
        {
            try
            {
                return System.IO.File.ReadAllText(path);
            } catch {}
            return null;
            
        }

        /// <summary>
        /// Get the path to the directory for a given file
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Returns the path to the directory</returns>
        public static string GetDirectoryName(string path)
        {
            try
            {
                return System.IO.Path.GetDirectoryName(path);
            }
            catch { }
            return null;
            
        }

        /// <summary>
        /// Create a directory at the given path
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(string path)
        {
            try
            {
                System.IO.Directory.CreateDirectory(path);
            }
            catch { }
            
        }

        /// <summary>
        /// Open the File Browser Dialog and return the path of the selected folder, null if none
        /// </summary>
        /// <returns>Returns the selected folder</returns>
        public static string OpenFolderDialog()
        {
            Microsoft.Win32.FileDialog folderDialog = new Microsoft.Win32.OpenFileDialog
            {
                CheckFileExists = false,
                FileName = FolderSelection // Default name
            };
            bool? result = folderDialog.ShowDialog();
            if (result == true)
                return System.IO.Path.GetDirectoryName(folderDialog.FileName);

            return null;
        }
        

    }
}
