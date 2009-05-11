using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChartDirector;
using SbB.Diploma;
using SbB.Diploma.Methods;

namespace GUIforCoupling
{
    public partial class Coupling : Form
    {
        private BaseChart chart;
        private List<Graphic> graphs;
        private DomainTriangulation dt;
        private DomainTriangulation dt2;
        private DomainTriangulation dt3;

        public Coupling()
        {
            graphs=new List<Graphic>();
            Polygon p =
           new Polygon(new Vertex[] {new Vertex(0, 0), new Vertex(0, 1), new Vertex(1, 1), new Vertex(1, 0),});
            GraphicOptions gropt=new GraphicOptions(FunctionType.ConstX,p) {ConstValue = 0.5};
            Graphic g=new Graphic((x,y)=>-2*y*y+1) {N = 100, Name = "Name1", Options = gropt};

            graphs.Add(g);

            Polygon p1 =
                new Polygon(new [] { new Vertex(0, 0), new Vertex(0, 1), new Vertex(1, 1), new Vertex(1, 0), });
            GraphicOptions gropt1 = new GraphicOptions(FunctionType.InSegments, p1)
                                        {
                                            X1 = new Vertex(0, 0),
                                            X2 = new Vertex(1, 1)
                                        };
            Graphic g1 = new Graphic((x, y) => x * y) {N = 100, Name = "Name2", Options = gropt1};
            graphs.Add(g1);


            LinearTriangle[] lintriangles=new LinearTriangle[3];
            lintriangles[0]=new LinearTriangle(new Vertex(0,0),new Vertex(0.5,0),new Vertex(0,1) );
            lintriangles[1] = new LinearTriangle(new Vertex(0.5, 0), new Vertex( 0.5,1), new Vertex(0.5, 0.5));
            lintriangles[2] = new LinearTriangle(new Vertex(0, 1), new Vertex(0.5, 0.5), new Vertex(0.5, 1));

            LinearBEMEdge[] segments = new LinearBEMEdge[8];
            segments[0] = new LinearBEMEdge(new Vertex(0.5, 0), new Vertex(0.5, 0.1));
            segments[1] = new LinearBEMEdge(new Vertex(0.5, 0.1), new Vertex(0.5, 0.2));
            segments[2] = new LinearBEMEdge(new Vertex(0.5, 0.2), new Vertex(0.5, 0.4));
            segments[3] = new LinearBEMEdge(new Vertex(0.5, 0.4), new Vertex(0.5, 0.6));
            segments[4] = new LinearBEMEdge(new Vertex(0.5, 0.6), new Vertex(0.5, 0.8));
            segments[5] = new LinearBEMEdge(new Vertex(0.5, 0.8), new Vertex(0.5, 1));
            segments[6] = new LinearBEMEdge(new Vertex(0.5, 1), new Vertex(1, 1));
            segments[7] = new LinearBEMEdge(new Vertex(1, 1), new Vertex(0, 1));

            dt=new DomainTriangulation{Polygon=p};
            dt2 = new DomainTriangulation { Elements = lintriangles };
            dt3 = new DomainTriangulation() {Segments = segments};

            InitializeComponent();
            //lineModeChart();
            triangulationeModeChart();
            flash();
            

        }
        private void flash()
        {
            Canvas.Image = chart.makeImage();

        }

        private void lineModeChart()
        {

            var chart1 = new XYChart(Canvas.Width, Canvas.Height);
            chart1.setSize(Canvas.Width, Canvas.Height);
            chart1.setRoundedFrame();

            chart1.setPlotArea(30, 30, Canvas.Width - 60, Canvas.Height - 100, 0xffffff, -1, -1, 0xcccccc, 0xcccccc);
            foreach (var graphic in graphs)
            {
                graphic.drawLine(chart1);
            }

            chart = chart1;

            chart.addLegend(50, 30, false, "Arial Bold", 9).setBackground(
                Chart.Transparent);


        }

        private void triangulationeModeChart()
        {

            var chart1 = new XYChart(Canvas.Width, Canvas.Height);
            chart1.setSize(Canvas.Width, Canvas.Height);
            chart1.setRoundedFrame();

            chart1.setPlotArea(40, 40, Canvas.Width - 60, Canvas.Height - 100, 0xffffff, -1, -1, 0xffffff, 0xffffff);
            dt.drawDomain(chart1);
            dt2.triangulationLayer(chart1);
            dt3.segmentsLayer(chart1);
            chart = chart1;
            //chart1.setShadingMode(Chart.S)

        }

        private void surfaceModeChart()
        {
            SurfaceChart chart1 = new SurfaceChart(Canvas.Width, Canvas.Height, 0xffffff, 0x888888);
            chart1.setSize(Canvas.Width, Canvas.Height);
            chart1.setRoundedFrame();
            chart1.setPlotRegion(40, 40, Canvas.Width - 60, Canvas.Height - 100, 150);
            chart1.setViewAngle(90);
            chart1.setWallThickness(0);
            chart1.setPerspective(0);

            //chart1.setShadingMode(Chart.S)

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            
            lineModeChart();
            flash();
            //Canvas.Invalidate();
        }
        
    }

   

}
