using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SbB.Diploma;


namespace GUIforCoupling
{
    public partial class GraphicDialog : Form
    {
        private ListStorage listStarage = Coupling.listStarage;
        private CurrentStorage currentStorage = Coupling.currentStarage;

        public GraphicDialog()
        {
            InitializeComponent();
            
            /*user defined initialize*/
            var b = true;
            foreach (var problem in listStarage.Problems)
            {
              problemLB.Items.Add(problem.Key);
              problemLB.SelectedIndex = problemLB.Items.IndexOf(currentStorage.Problem);
            }
            foreach (var graphic in listStarage.Graphics)
            {
                graphicsLB.Items.Add(graphic);
                if (currentStorage.Graphic!=null)
                graphicsLB.SelectedIndex = graphicsLB.Items.IndexOf(currentStorage.Graphic);
            }
            foreach (var gropt in listStarage.Groptions)
            {
               optionsLB.Items.Add(gropt);
               if (currentStorage.Groption != null)
               optionsLB.SelectedIndex = optionsLB.Items.IndexOf(currentStorage.Groption);
            }
            foreach (var gropt in listStarage.Functions)
            {
                functionsLB.Items.Add(gropt);
                if (currentStorage.Function != null)
                functionsLB.SelectedIndex = functionsLB.Items.IndexOf(currentStorage.Function);
            }
            
        }

        
       void reTie()
       {
           currentStorage.Graphic = graphicsLB.SelectedItem as Graphic;
           currentStorage.Problem = problemLB.SelectedItem as BPMethod;
           currentStorage.Groption = optionsLB.SelectedItem as GraphicOptions;
       }





        private void button6_Click(object sender, EventArgs e)
        {
            GraphicOptions go;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    go =new GraphicOptions(FunctionType.ConstX, currentStorage.Problem.Polygon);
                    go.ConstValue = double.Parse(xConstValueTB.Text);
                    go.Name = optionNewName.Text;
                    optionsLB.Items.Add(go);
                    listStarage.Groptions.Add(go);
                    break;
                case 1:
                    go = new GraphicOptions(FunctionType.ConstY, currentStorage.Problem.Polygon);
                    go.ConstValue = double.Parse(yConstValueTB.Text);
                    go.Name = optionNewName.Text;
                    optionsLB.Items.Add(go);
                    listStarage.Groptions.Add(go);
                    break;
            }

            optionsLB.SelectedIndex = optionsLB.Items.Count - 1;
        }

        private void optionsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            var opt = optionsLB.SelectedItem as GraphicOptions;
            switch (opt.FunctionType)
            {
                case FunctionType.ConstX:
                    tabControl1.SelectedIndex = 0;
                    xConstValueTB.Text = opt.ConstValue.ToString();
                    break;
                case FunctionType.ConstY:
                    tabControl1.SelectedIndex = 1;
                    yConstValueTB.Text = opt.ConstValue.ToString();
                    break;
            }
            optionNewName.Text = opt.Name;
            currentStorage.Groption = opt;
           
        }

        private void button7_Click(object sender, EventArgs e)
        {

            try
            {

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    optionsLB.SelectedItem = new GraphicOptions(FunctionType.ConstX, currentStorage.Problem.Polygon);
                    ((GraphicOptions)optionsLB.SelectedItem).ConstValue = double.Parse(xConstValueTB.Text);
                    ((GraphicOptions) optionsLB.SelectedItem).Name = optionNewName.Text;
                    break;
                case 1:
                    optionsLB.SelectedItem = new GraphicOptions(FunctionType.ConstY,currentStorage.Problem.Polygon);
                    ((GraphicOptions)optionsLB.SelectedItem).ConstValue = double.Parse(yConstValueTB.Text);
                    ((GraphicOptions)optionsLB.SelectedItem).Name = optionNewName.Text;
                    break;
            }
                }
            catch (Exception)
            {
                
               
            }
            listStarage.Groptions.Clear();
            foreach (GraphicOptions s in optionsLB.Items)
            {
                listStarage.Groptions.Add(s);
            }
            
            int i = optionsLB.SelectedIndex;
            optionsLB.Items.Clear();
            foreach (var s in listStarage.Groptions)
            {
                optionsLB.Items.Add(s);
            }
            optionsLB.SelectedIndex = i;
            //optionsLB.ResetText();
       
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listStarage.Groptions.RemoveAt(optionsLB.SelectedIndex);
            optionsLB.Items.RemoveAt(optionsLB.SelectedIndex);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var opt = optionsLB.SelectedItem as GraphicOptions;
            Polygon poly = currentStorage.Problem.Polygon;

            Graphic gr = new Graphic(currentStorage.Function);
            gr.N = int.Parse(pointsNumberTB.Text);
            gr.Name = functionNameTB.Text;
            gr.Options = opt;

            listStarage.Graphics.Add(gr);
            graphicsLB.Items.Add(gr);
 
        }

        private void problemLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentStorage.Problem = listStarage.Problems[problemLB.SelectedItem.ToString()];
            listStarage.Functions=new Dictionary<string, fxyArr>();
            listStarage.Functions.Add("U", currentStorage.Problem.U);
            listStarage.Functions.Add("V", currentStorage.Problem.V);
            listStarage.Functions.Add("Sxx", currentStorage.Problem.Sxx);
            listStarage.Functions.Add("Syy", currentStorage.Problem.Syy);
            listStarage.Functions.Add("Sxy", currentStorage.Problem.Sxy);

            functionsLB.Items.Clear();
            functionsLB.Items.Add("U");
            functionsLB.Items.Add("V");
            functionsLB.Items.Add("Sxx");
            functionsLB.Items.Add("Syy");
            functionsLB.Items.Add("Sxy");
            functionsLB.SelectedIndex = 0;

        }

        private void functionsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (functionsLB.Items.Count == 0) return;
           currentStorage.Function = listStarage.Functions[functionsLB.SelectedItem.ToString()];
           if (currentStorage.Groption!=null)
            functionNameTB.Text = functionsLB.SelectedItem.ToString() + " " + currentStorage.Groption.Name + "("+currentStorage.Problem.ToString() + ")";
           //functionsLB.Items.Remove(functionsLB.SelectedItem);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i = graphicsLB.SelectedIndex;
            listStarage.Graphics.RemoveAt(i);
            graphicsLB.Items.Clear();
            foreach (var s in listStarage.Graphics)
            {
                graphicsLB.Items.Add(s);
            }
            graphicsLB.SelectedIndex = -1;
        }

 
        
    }
}
