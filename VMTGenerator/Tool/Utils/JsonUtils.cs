using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMTGenerator.Tool.Utils
{
    class JsonUtils
    {

        private const string DirectoryJson = "Parameters/";

        private const string ShaderJson = "Parameters/shader.json";
        private const string SurfacePropJson = "Parameters/surfaceprop.json";
        private const string ParametersJson = "Parameters/parameters.json";

        private const string ShaderBackup = "VMTGenerator.Parameters.shader_backup.txt";
        private const string SurfacePropBackup = "VMTGenerator.Parameters.surfaceprop_backup.txt";
        private const string ParametersBackup = "VMTGenerator.Parameters.parameters_backup.txt";


        /// <summary>
        /// Create the JSON files directory if it doesn't exist
        /// </summary>
        public static void CreateJsonDirectory()
        {
            if (!System.IO.Directory.Exists(DirectoryJson))
            {
                FilesUtils.CreateDirectory(DirectoryJson);
            }
        }


        /// <summary>
        /// Read the associated JSON file and create it if it's doesn't exist.
        /// </summary>
        /// <param name="jsonEnum">Enumeration associating the file to load</param>
        /// <returns>The JSON string</returns>
        public static string ReadJson(JsonFileEnum jsonEnum)
        {
            string jsonFile = null;
            string jsonBackup = null;

            switch(jsonEnum)
            {
                case JsonFileEnum.Shader:
                    jsonFile = ShaderJson;
                    jsonBackup = ShaderBackup;
                    break;
                case JsonFileEnum.SurfaceProp:
                    jsonFile = SurfacePropJson;
                    jsonBackup = SurfacePropBackup;
                    break;
                case JsonFileEnum.Parameters:
                    jsonFile = ParametersJson;
                    jsonBackup = ParametersBackup;
                    break;
            }

            string jsonText = null;
            if (System.IO.File.Exists(jsonFile))
            {
                // If the json file exists, we read it
                jsonText = FilesUtils.ReadAllText(jsonFile);
            }
            else
            {
                // Otherwise we read the embedded file and then create the json
                jsonText = FilesUtils.ReadResourceFile(jsonBackup);
                FilesUtils.WriteAllText(jsonFile, jsonText);
            }
            return jsonText;
        }
        
        /// <summary>
        /// Parse the given JSON string into a List of string
        /// </summary>
        /// <param name="jsonText">JSON string</param>
        /// <returns>The parsed List of String</returns>
        public static List<string> DeserializeArray(string jsonText)
        {
           return Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(jsonText);
        }

    }

    enum JsonFileEnum
    {
        Shader,
        SurfaceProp,
        Parameters
    }
}
