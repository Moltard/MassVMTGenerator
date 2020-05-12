using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMTGenerator.Tool
{
    class VMTProperties
    {
        public string Shader { get; set; }
        public string BaseTexture { get; set; }
        public List<KeyValuePair<string, string>> ParametersList { get; set; }
        
        public VMTProperties()
        {
            Shader = "";
            BaseTexture = "";
            ParametersList = new List<KeyValuePair<string, string>>();
        }
    }
}
