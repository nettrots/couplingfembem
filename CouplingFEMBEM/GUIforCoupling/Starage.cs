using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using SbB.Diploma;

namespace GUIforCoupling
{
    public delegate void Void();
    [Serializable()]
    public class Workspace : ISerializable
    {
        public ListStarage ListStarage { get; set; }
        public CurrentStarage CurrentStarage { get; set; }

        public static void Save(string fileName, Workspace wsp)
        {
            Stream stream = File.Open(fileName, FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();
            bformatter.Serialize(stream, wsp);
            stream.Close();
        }

        public static void Load(string fileName, Workspace wsp)
        {
            Stream stream = File.Open(fileName, FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();
            wsp = (Workspace)bformatter.Deserialize(stream);
            stream.Close();
        }

        public Workspace()
        {
            ListStarage = new ListStarage();
            CurrentStarage = new CurrentStarage();
        }
        public Workspace(SerializationInfo info, StreamingContext ctxt)
        {
            ListStarage = (ListStarage)info.GetValue("listStarage", typeof(ListStarage));
            CurrentStarage = (CurrentStarage)info.GetValue("currentStarage", typeof(CurrentStarage));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("listStarage", typeof(ListStarage));
            info.AddValue("currentStarage", typeof(CurrentStarage));
        }
    }

    public class ListStarage
    {
        public List<Graphic> Graphics { get; set; }
        public List<DomainTriangulation> DomainTriangulation { get; set; }
        public List<GraphicOptions> Groptions { get; set; }
        public Dictionary<string, BPMethod> Problems { get; set; }

        public Dictionary<string,Void>ChartRedraw { get; set; }

        public ListStarage()
        {
            Graphics = new List<Graphic>();
            DomainTriangulation = new List<DomainTriangulation>();
            Groptions = new List<GraphicOptions>();
            Problems = new Dictionary<string, BPMethod>();
            ChartRedraw = new Dictionary<string, Void>();
        }

    }
    public class CurrentStarage
    {
        public Graphic Graphic { get; set; }
        public GraphicOptions Groption { get; set; }
        public BPMethod Problem { get; set; }
        public DomainTriangulation DomainTriangulation { get; set; }
        public Void ChartRedraw { get; set; }

        public CurrentStarage()
        {
  
        }
    }

 

}
