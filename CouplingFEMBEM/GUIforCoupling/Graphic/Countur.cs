using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChartDirector;
using SbB.Diploma;

namespace GUIforCoupling
{
    public class Countur
    {
        public List<Vertex> NewVertexes{ get; set;}
        public fxy Fxy { get; set; }

        public Countur(){}

        public void drawCounturs(XYChart chart)
        {
            double[] X = new double[NewVertexes.Count], Y = new double[NewVertexes.Count], Z = new double[NewVertexes.Count];
            double min = double.MaxValue, max = double.MinValue;
            for (int i = 0; i < NewVertexes.Count; i++)
            {
                X[i] = NewVertexes[i].X;
                Y[i] = NewVertexes[i].Y;
                Z[i] = Fxy(NewVertexes[i].X, NewVertexes[i].Y);
                if (Z[i] > max) max = Z[i];
                if (Z[i] < min) min = Z[i];
            }

            // Add a scatter layer to the chart to show the position of the data
            // points
            chart.addScatterLayer(X, Y, "", Chart.Cross2Shape(0.2), 4, 0x000000);

            // Add a contour layer using the given data
            ContourLayer layer = chart.addContourLayer(X, Y, Z);

            // Move the grid lines in front of the contour layer
            chart.getPlotArea().moveGridBefore(layer);

            // Add a color axis (the legend) in which the top center is anchored at
            // (245, 455). Set the length to 330 pixels and the labels on the top
            // side.
            int w = chart.getWidth();
            int h = chart.getHeight();
            ColorAxis cAxis = layer.setColorAxis(0, w-40, Chart.TopCenter, h-w-40,
                Chart.Top);

            // Add a bounding box to the color axis using the default line color as
            // border.
            cAxis.setBoundingBox(Chart.Transparent, Chart.LineColor);


            // Set the color axis range as 0 to 20, with a step every 2 units
            cAxis.setLinearScale(min, max, (max - min)/100);


        }

    }

  
}
