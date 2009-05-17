﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChartDirector;
using QiHe.Yaml.Grammar;
using SbB.Diploma;
using SbB.Diploma.Methods;
using SbB.Diploma.Yaml.Custom;
using QiHe.Yaml.YamlUtility.UI;

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
        public static ListStorage listStarage;
        public static CurrentStorage currentStarage;
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

            switch (currentStarage.DomainTriangulation.State)
            {
                case 0:
                    currentStarage.DomainTriangulation.drawDomain(chart1);
                    break;
                case 1:

                    currentStarage.DomainTriangulation.drawTringulation(chart1);
                    break;
            }
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
        Dictionary<string, HashValue> ReadYaml(string filename)
        {
            //Create methods and fill with data

            //load yaml file
            //traverse tree to arraylist
            //ArrayList data=new ArrayList();
            Dictionary<string, HashValue> data = new Dictionary<string, HashValue>();
            if (!File.Exists(filename))
            {
                MessageBox.Show(filename + " does not exist.");
                return null;
            }
            YamlParser parser = new YamlParser();
            TextInput input = new TextInput(File.ReadAllText(filename));
            bool success;
            YamlStream yamlStream = parser.ParseYamlStream(input, out success);
            if (success)
            {
                data.Clear();
                foreach (YamlDocument doc in yamlStream.Documents)
                {
                    Dictionary<string, HashValue> a1 = YamlEmittor.CreateNode(doc.Root).eHash;
                    foreach (KeyValuePair<string, HashValue> pair in a1)
                    {
                        data.Add(pair.Key, pair.Value);
                    }
                }
            }
            else
            {
                //   MessageBox.Show(parser.GetEorrorMessages());
            }
            return data;
        }
        void LoadProblem(string filename)
        {
            
            Dictionary<string, HashValue> data=ReadYaml(filename);
            string[] ss= filename.Split('.');
            string mType = ss[ss.Length-1];
            BPMethod meth;
            switch (mType)
            {
                case "fem":
                     meth = new FEMMethod(data["FEM"].eHash);
                    listStarage.Problems.Add(meth.ToString(), meth);
                    break;
                case "bem":
                    meth = new MakarBEMMethod(data["BEM"].eHash);
                    listStarage.Problems.Add(meth.ToString(), meth);
                    break;
                case "coupl":
                    meth = new CouplingMethod(data);
                    listStarage.Problems.Add(meth.ToString(), meth);
                    break;
                default:
                    meth = null;
                    break;
            }
            currentStarage.Problem = meth;
        }
        #endregion

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if( openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                LoadProblem(openFileDialog1.FileName);
                problemCB.Items.Add(currentStarage.Problem.ToString());
                problemCB.SelectedIndex = problemCB.Items.Count - 1;
            }
        }

        private void showFigurebtn_Click(object sender, EventArgs e)
        {
            //currentStarage.DomainTriangulation = listStarage.DomainTriangulation["problemModeChart"];
            currentStarage.ChartRedraw = listStarage.ChartRedraw["problemModeChart"];
            currentStarage.DomainTriangulation = new DomainTriangulation
                                                     {
                                                         Polygon =  currentStarage.Problem.Polygon,
                                                         State = 0
                                                     };
            flash();
        }

        private void showMeshbtn_Click(object sender, EventArgs e)
        {
            currentStarage.ChartRedraw = listStarage.ChartRedraw["problemModeChart"];

            FEMElement[] elements=null;
            Edge[] segments=null;
            List<Edge> segmentsTemp = null;
            if (currentStarage.Problem is CouplingMethod)
            {

                elements = (currentStarage.Problem as CouplingMethod).FEM.Elements.ToArray();
                segmentsTemp = new List<Edge>();
                foreach (var edges in (currentStarage.Problem as CouplingMethod).BEM.Boundaries)
                {
                    foreach (var list in edges)
                    {
                        segmentsTemp.Add(list);
                    }
                }
                segments = segmentsTemp.ToArray();
            }
            if (currentStarage.Problem is FEMMethod)
                elements = (currentStarage.Problem as FEMMethod).Elements.ToArray();
            if (currentStarage.Problem is MakarBEMMethod)
            {
                segmentsTemp = new List<Edge>();
                foreach (var edges in (currentStarage.Problem as MakarBEMMethod).Boundaries)
                {
                    foreach (var list in edges)
                    {
                        segmentsTemp.Add(list);
                    }
                }
                segments = segmentsTemp.ToArray();
            }

            currentStarage.DomainTriangulation = new DomainTriangulation
                                                     {
                                                         Elements = elements,
                                                         Segments = segments,
                                                         State = 1
                                                     };
            flash();
        }

        private void initProblembtn_Click(object sender, EventArgs e)
        {
            currentStarage.Problem.Initialize();

        }

        private void functionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GraphicDialog grDialog = new GraphicDialog();
            if(grDialog.ShowDialog()==DialogResult.OK)
            {

                grOptionsCB.Items.AddRange(listStarage.Groptions.ToArray());
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            functionsToolStripMenuItem_Click( sender,  e);
        }

        private void runProblembtn_Click(object sender, EventArgs e)
        {
            currentStarage.Problem.Run();
        }

        private void solveProblembtn_Click(object sender, EventArgs e)
        {
             currentStarage.Problem.Solve();
        }

        private void doAllbtn_Click(object sender, EventArgs e)
        {
             currentStarage.Problem.Initialize();
             currentStarage.Problem.Run();
             currentStarage.Problem.Solve();
        }

        private void grOptionsCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var item in listStarage.Groptions)
            {
                if (item.Name == grOptionsCB.SelectedText)
                {
                    currentStarage.Groption = item;
                    break;
                }
            }
            foreach (var item in listStarage.Graphics)
            {
                if (item.Options == currentStarage.Groption)
                    item.Enabled = true;
                else
                    item.Enabled = false;
            }
            //currentStarage.Groption=
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            currentStarage.ChartRedraw = listStarage.ChartRedraw["lineModeChart"];
            flash();
        }
    }

   

}
