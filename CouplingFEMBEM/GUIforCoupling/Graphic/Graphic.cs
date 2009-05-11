using System;
using ChartDirector;
using SbB.Diploma;


namespace GUIforCoupling
{
    public delegate double f(double t);
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
        private int n;

        public Graphic(fxy f) {
            this.f = f; }

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

        public string Name
        {
            get; set;
        }

        public double F(double t)
        {
            return options.CheckPoint(options.getX(t), options.getY(t)) ? f(options.getX(t), options.getY(t)) : double.NaN;
        }
        public override string ToString()
        {
            return Name;
        }
        public void draw(XYChart chart)
        {
            double[] x=new double[n+1], y=new double[n+1];
            for (int i = 0; i < n+1; i++)
            {
                x[i] = this.Options.getX((float)i/n);
                y[i] = F((float) i/n);
            }
            // Create a SurfaceChart object of size 720 x 600 pixels

            XYChart c = new XYChart(600, 300);
            c.setRoundedFrame();

            // Set the plotarea at (55, 58) and of size 520 x 195 pixels, with white
            // background. Turn on both horizontal and vertical grid lines with light
            // grey color (0xcccccc)
            c.setPlotArea(55, 58, 520, 195, 0xffffff, -1, -1, 0xcccccc, 0xcccccc);

            // Add a legend box at (50, 30) (top of the chart) with horizontal
            // layout. Use 9 pts Arial Bold font. Set the background and border color
            // to Transparent.
            c.addLegend(50, 30, false, "Arial Bold", 9).setBackground(
                Chart.Transparent);

            // Add a title box to the chart using 15 pts Times Bold Italic font, on a
            // light blue (CCCCFF) background with glass effect. white (0xffffff) on
            // a dark red (0x800000) background, with a 1 pixel 3D border.
           

            // Add a title to the y axis
            c.yAxis().setTitle("MBytes per hour");
);

            // Add a line layer to the chart
            LineLayer layer = c.addLineLayer2();

            // Set the default line width to 2 pixels
            layer.setLineWidth(2);

            // Add the three data sets to the line layer. For demo purpose, we use a
            // dash line color for the last line
            layer.addDataSet(data0, 0xff0000, "Server #1");
            layer.addDataSet(data1, 0x008800, "Server #2");
        

            
        }
    }


    public enum FunctionType
    {
        ConstX,ConstY
    }
    public class GraphicOptions
    {
        private FunctionType type;
        private Box box;
        private double constValue;
        private Polygon polygon;

        public GraphicOptions(FunctionType type,Polygon poly)
        {

            this.Polygon = poly;
            this.FunctionType = type;
            this.calcBox();

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
