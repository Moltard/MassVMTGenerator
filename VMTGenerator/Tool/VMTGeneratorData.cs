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
            string vmtFilePath = VMTUtils.GetVMTFilePath(OutputPath, vtfName);
            string vmtString = VMTUtils.GetVMTString(VMTData, vtfName);
            bool success = FilesUtils.TryWriteAllText(vmtFilePath, vmtString);
            if (success)
            {
                sb.Append($"Generated {vtfName}.vmt in \"{OutputPath}\"");
            }
            else
            {
                sb.Append($"Unable to create {vtfName}.vmt in \"{OutputPath}\"");
            }
            sb.AppendLine();
            return sb.ToString();
        }

    }
}
