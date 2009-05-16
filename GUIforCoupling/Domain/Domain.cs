using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ChartDirector;
using SbB.Diploma;

namespace GUIforCoupling
{
    public class DomainTriangulation
    {
        public FEMElement[] Elements{ get; set;}
        public Edge[] Segments{ get; set; }
        public Polygon Polygon { get; set; }
        public DomainTriangulation()
        {
        }


        private void drawline(Edge e,XYChart chart)
        {
//            LineLayer layer = chart.addLineLayer2();
//
//            layer.setLineWidth(2);
//            
//            //  Y
//            layer.addDataSet(y, -1);
        }

        private void polygonLayer(Polygon polygon, int symbol, int symbSize, XYChart chart)
        {
            polygonLayer(polygon, -1, symbol, symbSize, chart);
        }
        private void polygonLayer(Polygon polygon,int color, XYChart chart)
        {
            polygonLayer(polygon, color, 1, 3, chart);
        }

        private void polygonLayer(Polygon polygon, XYChart chart)
        {
            polygonLayer(polygon, -1, 1, 3, chart);
        }

        private void polygonLayer(Polygon polygon,int color,int symbol,int symbSize,XYChart chart)
        {
            int N = polygon.Count;
            double[] x = new double[N + 1], y = new double[N + 1];
            for (int i = 0; i < polygon.Count+1; i++)
            {
                x[i] = polygon[i].X;
                y[i] = polygon[i].Y;
            }
            // Add a line layer to the chart
            LineLayer layer = chart.addLineLayer2();
            layer.setLineWidth(1);
            //  X
            layer.setXData(x);
            //  Y
            layer.addDataSet(y, color);
            //Chart.CrossShape
            //Chart.SquareSymbol
            layer.getDataSet(0).setDataSymbol(symbol, symbSize);
        }

    
        public void triangulationLayer(XYChart chart)
        {
            foreach (FEMElement element in Elements)
            {
                polygonLayer(element, 0xff00ff, Chart.SquareSymbol,3, chart);
            }

        }

        public void segmentsLayer(XYChart chart)
        {
            var v=new List<Vertex>();
            foreach (var edge in Segments)
            {
                for (var i = 0; i < 2; i++)
                {
                if(!v.Contains(edge[i]))
                    v.Add(edge[i]);                    
                }
            }
            Polygon poly=new Polygon(v.ToArray());
            polygonLayer(poly, Chart.CrossShape(), 4, chart);
        }

        public void drawDomain(XYChart chart)
        {
            polygonLayer(Polygon, chart);
            //Boundary conditioans using Vector charts
        }

    }
  

}
