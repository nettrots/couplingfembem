using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SbB.Diploma.Yaml.Custom
{

   
    public struct HashValue
    {
        public double eDouble;
        public int eInt;
        public string eString;
        public Dictionary<string, HashValue> eHash;

    }

}
