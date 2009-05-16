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
        
     

        public Coupling()
        {

            workspace = new Workspace();
            listStarage = workspace.ListStarage;
            currentStarage = workspace.CurrentStarage;

            listStarage.ChartRedraw.Add("empty", empty);
            listStarage.ChartRedraw.Add("lineModeChart", lineModeChart);
            listStarage.ChartRedraw.Add("problemModeChart", problemModeChart);
            listStarage.ChartRedraw.Add("surfaceModeChart", surfaceModeChart);
            currentStarage.ChartRedraw = listStarage.ChartRedraw["empty"];

            InitializeComponent();
            //lineModeChart();
           // triangulationeModeChart();
           // flash();


        }
        #region Workspace region
        public static Workspace workspace;
        public static ListStarage listStarage;
        public static CurrentStarage currentStarage;
        #endregion

        #region Chart region

        private BaseChart chart;
        
        private void flash()
        {
            currentStarage.ChartRedraw();
            Canvas.Image = chart.makeImage();
        }

        private void empty()
        {
            chart = new XYChart(Canvas.Width, Canvas.Height);
            chart.setSize(Canvas.Width, Canvas.Height);
            chart.setRoundedFrame();
        }

        private void lineModeChart()
        {

            var chart1 = new XYChart(Canvas.Width, Canvas.Height);
            chart1.setSize(Canvas.Width, Canvas.Height);
            chart1.setRoundedFrame();

            chart1.setPlotArea(30, 30, Canvas.Width - 60, Canvas.Height - 100, 0xffffff, -1, -1, 0xcccccc, 0xcccccc);
            foreach (var graphic in listStarage.Graphics)
            {
                graphic.drawLine(chart1);
            }

            chart = chart1;

            chart.addLegend(50, 30, false, "Arial Bold", 9).setBackground(
                Chart.Transparent);


        }

        private void problemModeChart()
        {

            var chart1 = new XYChart(Canvas.Width, Canvas.Height);
            chart1.setSize(Canvas.Width, Canvas.Height);
            chart1.setRoundedFrame();

            chart1.setPlotArea(40, 40, Canvas.Width - 60, Canvas.Height - 100, 0xffffff, -1, -1, 0xffffff, 0xffffff);
           
            
            /*dt.drawDomain(chart1);
            dt2.triangulationLayer(chart1);
            dt3.segmentsLayer(chart1);*/
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

            flash();
            //Canvas.Invalidate();
        }
        #endregion

        #region Work with Problems
        void LoadProblem()
        {
         
            IProblem problem;
        }
        #endregion
    }

   

}
