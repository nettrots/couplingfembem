﻿using System;
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
           GraphicOptions gr=new GraphicOptions(FunctionType.ConstX,currentStorage.Problem.Polygon);
            gr.ConstValue = 0;
            gr.Name = "on x=0";
            listStarage.Groptions.Add(gr);
            optionsLB.Items.Add(gr);
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
            listStarage.Functions=new Dictionary<string, fxy>();
            listStarage.Functions.Add("U", currentStorage.Problem.U);
            listStarage.Functions.Add("V",currentStorage.Problem.V);

            functionsLB.Items.Clear();
            functionsLB.Items.Add("U");
            functionsLB.Items.Add("V");
            functionsLB.SelectedIndex = 0;

        }

        private void functionsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
           currentStorage.Function = listStarage.Functions[functionsLB.SelectedItem.ToString()];
           if (currentStorage.Groption!=null)
            functionNameTB.Text = functionsLB.SelectedItem.ToString() + " " + currentStorage.Groption.Name + "("+currentStorage.Problem.ToString() + ")";
           //functionsLB.Items.Remove(functionsLB.SelectedItem);
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

 
        
    }
}
