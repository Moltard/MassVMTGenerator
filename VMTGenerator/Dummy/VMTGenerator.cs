using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassVMTGenerator
{
    class VMTGenerator
    {
        private String folderPath;
        private String shader;
        private String basetexture;
        private List<String> vtfsList;
        private List<KeyValuePair<String,String>> mainParam;
        private String logCreation;

        // Constructor
        public VMTGenerator(String folderPath, String shader, String basetexture, List<String> vtfsList, List<KeyValuePair<String, String>> mainParam)
        {
            this.folderPath = folderPath;
            this.shader = shader;
            this.basetexture = basetexture;
            this.vtfsList = vtfsList;
            this.mainParam = mainParam;
            this.logCreation = "";
        }

        public string getLogCreation()
        {
            return this.logCreation;
        }
        
        public String getKeyValueLine(string key, string value, string indent = "\t")
        {
            return String.Format("{0}\"{1}\" \"{2}\"", indent, key, value);
        }

        public String getKeyLine(string key, string indent = "")
        {
            return String.Format("{0}\"{1}\"", indent, key);
        }

        public String getBracketLine(string bracket, string indent = "")
        {
            return String.Format("{0}{1}", indent, bracket);
        }

        public string getVMTString(String vtfName)
        {
            String vmtStr = "";
            vmtStr += getKeyLine(shader) + "\n";
            vmtStr += getBracketLine("{") + "\n";
            vmtStr += getKeyValueLine("$basetexture", String.Format("{0}{1}", basetexture, vtfName)) + "\n";

            foreach (KeyValuePair<string, string> item in mainParam)
            {
                vmtStr += getKeyValueLine(item.Key, item.Value) + "\n";
            }
            vmtStr += getBracketLine("}");

            return vmtStr;
        }
        

        public void CreateVMTs()
        {
            foreach(String vtfName in vtfsList){

                String fullVtfName = String.Format("{0}{1}.vmt",folderPath, vtfName);
                try
                {
                    if (File.Exists(fullVtfName)) // Check if file already exists. If yes, delete it.
                    {
                        File.Delete(fullVtfName);
                    }
                }
                catch
                {
                    logCreation += String.Format("Unable to delete the existing {0}.vmt in path \"{1}\"\n", vtfName, folderPath);
                }

                try
                {
                    using (StreamWriter sw = File.CreateText(fullVtfName)) // Create a new file  
                    {
                        sw.Write(getVMTString(vtfName));
                    }
                    logCreation += String.Format("Generated {0}.vmt to path \"{1}\"\n", vtfName, folderPath);
                }
                catch
                {
                    logCreation += String.Format("Unable to create {0}.vmt in path \"{1}\"\n", vtfName, folderPath);
                }
            }
        }
    }
}
