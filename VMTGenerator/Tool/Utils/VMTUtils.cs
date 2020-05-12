using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMTGenerator.Tool.Utils
{
    class VMTUtils
    {
        private const string DefaultIndent = "    ";
        private const string CurlyBracketOpen = "{";
        private const string CurlyBracketClose = "}";
        private const string KeyBaseTexture = "$basetexture";
        public const string KeySurfaceProp = "$surfaceprop";
        public const string ValueParameter = "1";

        private const string ExtensionVMT = ".vmt";
        public const string ExtensionsVTF = "*.vtf";

        /// <summary>
        /// Returns a string "key" "value"
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="indent"></param>
        /// <returns></returns>
        public static string GetKeyValueLine(string key, string value, string indent = "")
        {
            return string.Format("{0}\"{1}\" \"{2}\"", indent, key, value);
        }

        /// <summary>
        /// Returns a string "key"
        /// </summary>
        /// <param name="key"></param>
        /// <param name="indent"></param>
        /// <returns></returns>
        public static string GetKeyLine(string key, string indent = "")
        {
            return string.Format("{0}\"{1}\"", indent, key);
        }

        /// <summary>
        /// Returns a string with intendation and no quote
        /// </summary>
        /// <param name="str"></param>
        /// <param name="indent"></param>
        /// <returns></returns>
        public static string GetLine(string str, string indent = "")
        {
            return string.Format("{0}{1}", indent, str);
        }

        /// <summary>
        /// Returns the VMT string for the given VMT properties and VTF name 
        /// </summary>
        /// <param name="vmt">VMT data</param>
        /// <param name="vtfName">VTF name</param>
        /// <returns>The VMT content</returns>
        public static string GetVMTString(VMTProperties vmt, string vtfName)
        {
            string vtfPath = vmt.BaseTexture + vtfName;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(GetKeyLine(vmt.Shader));
            sb.AppendLine(GetLine(CurlyBracketOpen));   
            sb.AppendLine(GetKeyValueLine(KeyBaseTexture, vtfPath, DefaultIndent));
            foreach (KeyValuePair<string, string> item in vmt.ParametersList)
            {
                sb.AppendLine(GetKeyValueLine(item.Key, item.Value, DefaultIndent));
            }
            sb.Append(GetLine(CurlyBracketClose));
            return sb.ToString();
        }

        /// <summary>
        /// Return the path of the VMT file to create
        /// </summary>
        /// <param name="outputPath"></param>
        /// <param name="vtfName"></param>
        /// <returns></returns>
        public static string GetVMTFilePath(string outputPath, string vtfName)
        {
            return System.IO.Path.Combine(outputPath, vtfName + ExtensionVMT);
        }
    }
}
