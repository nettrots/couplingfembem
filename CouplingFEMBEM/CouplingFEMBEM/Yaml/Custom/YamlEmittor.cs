using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using QiHe.Yaml.Grammar;
using SbB.Diploma.Yaml.Custom;

namespace QiHe.Yaml.YamlUtility.UI
{
    public class YamlEmittor
    {
        public static HashValue CreateNode(DataItem item)
        {
            HashValue t=new HashValue();
            if (item is Scalar)
            {
                object obj = CreateNodeForScalar(item as Scalar);
                if (obj is int) { t.eInt = (int)obj; t.eDouble = t.eInt; }
                if (obj is double) t.eDouble = (double)obj;
                if (obj is string) t.eString = (string)obj;
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

        private static object CreateNodeForScalar(Scalar scalar)
        {
            string currentCulture = CultureInfo.CurrentCulture.Name;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            int rezint = 0;
            if (int.TryParse(scalar.Text, out rezint)) return rezint;
            double rezdouble = 0.0;
            if (double.TryParse(scalar.Text, out rezdouble)) return rezdouble;
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
