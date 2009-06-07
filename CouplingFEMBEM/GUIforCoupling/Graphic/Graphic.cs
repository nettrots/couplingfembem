using System;
using ChartDirector;
using SbB.Diploma;


namespace GUIforCoupling
{
    public delegate double f(double t);
    public delegate double[] fxyArr(Vertex[] v);
    public delegate double fxy(double x, double y);
    public class Box
    {
        public Box(Vertex minPoint,Vertex maxPoint)
        {
            MinPoint = minPoint;
            MaxPoint = maxPoint;
        }
        public Vertex MinPoint { get; set; }

        public Vertex MaxPoint { get; set; }

        public double MinX{ get { return MinPoint.X; }}
        public double MaxX{ get { return MaxPoint.X; }}
        public double MinY{ get { return MinPoint.Y; }}
        public double MaxY { get { return MaxPoint.Y; } }

        public Vertex A { get { return MinPoint; } }
        public Vertex B { get { return MaxPoint; } }

        }
    public class Graphic
    {
        private GraphicOptions options;
        private fxy f;
        private fxyArr farr;
        private int n;


        public bool Enabled { get; set; }
        public Graphic(fxyArr f) {
            farr = f; }

        public GraphicOptions Options
        {
            get { return options; }
            set { options = value; }
        }

        public fxy Fxy
        {
            set { f = value; }
            get { return  f; }
        }
        public fxyArr FxyArr
        {
            set { farr = value; }
            get { return farr; }
        }
        public string Name
        {
            get; set;
        }

        public int N
        {
            get { return n; }
            set { n = value; }
        }

        public double F(double t)
        {
            return options.CheckPoint(options.getX(t), options.getY(t)) ? f(options.getX(t), options.getY(t)) : double.NaN;
        }
        public double[] F(double[] t)
        {
            Vertex[] vertices=new Vertex[t.Length];
            for (int i = 0; i < t.Length; i++)
            {
                vertices[i] = new Vertex(options.getX(t[i]), options.getY(t[i]));
            }
            return farr(vertices);
            //return options.CheckPoint(options.getX(x), options.getYarr(x)) ? f(options.getX(t), options.getY(t)) : double.NaN;
        }
        public override string ToString()
        {
            return Name;
        }
      
        public void drawLine(XYChart chart)
        {
            double[] x = new double[N + 1], y = new double[N + 1], z = new double[N + 1], zt = new double[N + 1];
            string[] s=new string[N+1];
            for (int i = 0; i < N+1; i++)
            {
                x[i] = Options.getX((float)i/N);
                y[i] = Options.getY((float)i / N);
                s[i] = "(" +x[i]+ ";"+y[i]+")";
                zt[i] = ((float) i/N);
            }
            z = F(zt);
            // Add a title to the y axis
           //chart.yAxis().setTitle("MBytes per hour");

            // Add a line layer to the chart
            LineLayer layer = chart.addLineLayer2();

            layer.setLineWidth(2);
           
          //  X
           
            chart.xAxis().setLabels(s);
            chart.xAxis().setLabelStep(10);

          //  Y
           layer.addDataSet(z, -1, Name);
        

            
        }
    }


    public enum FunctionType
    {
        ConstX,ConstY,Canonical,InSegments
    }
    public class GraphicOptions
    {
        private FunctionType type;
        private Box box;
        
        //diferent types of equation have diferent constants
        private double constValue;
        private double a;
        private double b;
        private Vertex x1;
        private Vertex x2;

        private Polygon polygon;

        public GraphicOptions(FunctionType type,Polygon poly)
        {

            Polygon = poly;
            FunctionType = type;
            calcBox();

            switch (type)
            {
                case FunctionType.ConstX:
                    getX = t => ConstValue;
                    getY = t => (box.MaxY - box.MinY) * t + box.MinY;
                    break;
                case FunctionType.ConstY:
                    getX = t => (box.MaxX - box.MinX)*t + box.MinX;
                    getY = t => ConstValue;
                    break;
                case FunctionType.Canonical:
                    getX = t => (box.MaxX - box.MinX) * t + box.MinX;
                    getY = t => A*getX(t)+B;
                    break;
                case FunctionType.InSegments:
                    getX = t => (X2.X-X1.X) * t + X1.X;
                    getY = t => (X2.Y-X1.Y) * t + X1.Y;
                    break;
                default:
                    throw new Exception("Error in function type");
            }
            
            
        }


        private void calcBox()
        {
            Polygon p = Polygon;
            double xmin = p[0].X;
            double ymin = p[0].Y;
            double xmax = p[0].X;
            double ymax = p[0].Y;
            for (int i = 0; i < p.Count; i++)
            {
                if (p[i].X < xmin) xmin = p[i].X;
                if (p[i].Y < ymin) ymin = p[i].Y;
                if (p[i].X > xmax) xmax = p[i].X;
                if (p[i].Y > ymax) ymax = p[i].Y;
            }
            box = new Box(new Vertex(xmin,ymin),new Vertex(xmax,ymax));
             
        }
        
        public f getX { get; private set; }

        public f getY { get; private set; }


        public double ConstValue
        {
            get { return constValue; }
            set { if(FunctionType==FunctionType.ConstX || FunctionType.ConstY==FunctionType ) constValue = value; }
        }

        public FunctionType FunctionType
        {
            get { return type; }
            set { type = value; }
        }

        public string Name
        {
            get; set;
        }

        public Polygon Polygon
        {
            get { return polygon; }
            set { polygon = value; }
        }

        public double A
        {
            get { return a; }
            set { if (FunctionType == FunctionType.Canonical || FunctionType.ConstY == FunctionType) a = value; }
        }

        public double B
        {
            get { return b; }
            set { if (FunctionType == FunctionType.Canonical || FunctionType.ConstY == FunctionType) b = value; }
        }

        public Vertex X1
        {
            get { return x1; }
            set
            {
                if (FunctionType == FunctionType.InSegments || FunctionType.ConstY == FunctionType) x1 = value;
                if (x1 != null && x2 != null) swithPoints();
            }
        }

        public Vertex X2
        {
            get { return x2; }
            set
            {
                if (FunctionType == FunctionType.InSegments || FunctionType.ConstY == FunctionType) x2 = value;
                if (x1 != null && x2 != null) swithPoints();
            }
        }
        private void swithPoints()
        {
                if(x1>x2){
                        Vertex t = x1;
                        x1 = x2;
                        x2 = t;
                    }
        }


        internal bool CheckPoint(double p, double p_2)
        {
            var v = new Vertex(p, p_2);
            return Polygon.hasVertex(v);
        }
        public override string ToString()
        {
            return Name;
        }

    }
}
