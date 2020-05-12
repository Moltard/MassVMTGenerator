using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMTGenerator.Tool.Utils;

namespace VMTGenerator.Tool
{
    class VMTGeneratorData
    {
        public string OutputPath { get; set; }
        public List<string> VTFsList { get; set; }
        public VMTProperties VMTData { get; set; }
        public string Logs { get; set; }

        public VMTGeneratorData(string outputPath, List<string> vtfsList, VMTProperties vmtData)
        {
            OutputPath = outputPath;
            VTFsList = vtfsList;
            VMTData = vmtData;
            Logs = string.Empty;
        }

        public void GenerateVMTs()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string vtfName in VTFsList)
            {
                sb.AppendLine(GenerateVMT(vtfName));
            }
            Logs = sb.ToString();
        }

        private string GenerateVMT(string vtfName)
        {
            StringBuilder sb = new StringBuilder();
            string vmtPath = VMTUtils.GetVMTFilePath(OutputPath, vtfName);
            try
            {
                string vmtString = VMTUtils.GetVMTString(VMTData, vtfName);
                System.IO.File.WriteAllText(vmtPath, vmtString);
                sb.AppendFormat("Generated {0}.vmt in \"{1}\"", vtfName, OutputPath);
            }
            catch
            {
                sb.AppendFormat("Unable to create {0}.vmt in \"{1}\"", vtfName, OutputPath);
            }
            sb.AppendLine();
            return sb.ToString();
        }

    }
}
