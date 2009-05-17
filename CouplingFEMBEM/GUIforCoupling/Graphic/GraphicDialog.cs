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
              problemLB.Items.Add(problem);
              problemLB.SelectedIndex = problemLB.Items.IndexOf(currentStorage.Problem);
            }
            foreach (var graphic in listStarage.Graphics)
            {
                graphicsLB.Items.Add(graphic);
                graphicsLB.SelectedIndex = graphicsLB.Items.IndexOf(currentStorage.Graphic);
            }
            foreach (var gropt in listStarage.Groptions)
            {
               optionsLB.Items.Add(gropt);
               optionsLB.SelectedIndex = optionsLB.Items.IndexOf(currentStorage.Groption);
            }
            foreach (var gropt in listStarage.Functions)
            {
                functionsLB.Items.Add(gropt);
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
            Polygon poly=currentStorage.Problem.Polygon;
            GraphicOptions gro=new GraphicOptions(FunctionType.ConstX,poly);
            gro.Name = "default x=" + FunctionType.ConstX;
            listStarage.Groptions.Add(gro);
            optionsLB.Items.Add(gro);
            optionsLB.SelectedIndex = optionsLB.Items.IndexOf(gro);
        }

        private void optionsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            var opt = optionsLB.SelectedItem as GraphicOptions;
            switch (opt.FunctionType)
            {
                case FunctionType.ConstX:
                    tabControl1.SelectedIndex = 0;
                    textBox5.Text = opt.ConstValue.ToString();
                    break;
                case FunctionType.ConstY:
                    tabControl1.SelectedIndex = 1;
                    textBox4.Text = opt.ConstValue.ToString();
                    break;
            }
            optionNewName.Text = Name;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            var opt = optionsLB.SelectedItem as GraphicOptions;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    optionsLB.SelectedItem = new GraphicOptions(FunctionType.ConstX, opt.Polygon);
                    ((GraphicOptions) optionsLB.SelectedItem).ConstValue = double.Parse(optionNewName.Text);
                    ((GraphicOptions) optionsLB.SelectedItem).Name = optionNewName.Text;
                    break;
                case 1:
                    optionsLB.SelectedItem = new GraphicOptions(FunctionType.ConstY, opt.Polygon);
                    ((GraphicOptions)optionsLB.SelectedItem).ConstValue = double.Parse(textBox4.Text);
                    ((GraphicOptions)optionsLB.SelectedItem).Name = optionNewName.Text;
                    break;
            }    
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
           

 
        }

        private void problemLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            listStarage.Functions=new Dictionary<string, fxy>();
            listStarage.Functions.Add("U", currentStorage.Problem.U);
            listStarage.Functions.Add("V",currentStorage.Problem.V);

            functionsLB.Items.Add("U");
            functionsLB.Items.Add("V");
            functionsLB.SelectedIndex = 0;

        }

        private void functionsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
           currentStorage.Function = listStarage.Functions[functionsLB.SelectedItem.ToString()];
        }

 
        
    }
}
