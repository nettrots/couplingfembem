using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using QiHe.Yaml.Grammar;
using SbB.Diploma.Yaml.Custom;

namespace QiHe.Yaml.YamlUtility.UI
{
    class YamlEmittor
    {
        public static HashValue CreateNode(DataItem item)
        {
            HashValue t=new HashValue();
            if (item is Scalar)
            {
                //arr.Add(new DictionaryEntry(,CreateNodeForScalar(item as Scalar)));
                t.eString = CreateNodeForScalar(item as Scalar);
                return  t;
            }
            else if (item is Sequence)
            {
                t.eHash = CreateNodeForSequence(item as Sequence);
                return  t;
            }
            else if (item is Mapping)
            {
                t.eHash = CreateNodeForMapping(item as Mapping);
                return t;
            }
            else
            {
                return t;
            }
        }

        private static string CreateNodeForScalar(Scalar scalar)
        {
            return scalar.Text;
        }
        static int counter=1;
        private static Dictionary<string, HashValue> CreateNodeForSequence(Sequence sequence)
        {
            Dictionary<string, HashValue> node = new Dictionary<string, HashValue>();
            int i = 0;
            foreach (DataItem item in sequence.Enties)
            {

                node.Add((i++).ToString(), CreateNode(item));
            }
            return node;
        }

        private static Dictionary<string, HashValue> CreateNodeForMapping(Mapping mapping)
        {
            Dictionary<string, HashValue> a1 = new Dictionary<string, HashValue>();
            foreach (MappingEntry entry in mapping.Enties)
            {
                a1.Add(entry.Key.ToString(),CreateNode( entry.Value));
            }
            return a1;
        }
    }
}
