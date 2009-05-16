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
        private Dictionary<string, CouplingMethod> problemsD;
        private Dictionary<string, Graphic> graphicsD;
        private Dictionary<string, GraphicOptions> groptD;



        public GraphicDialog(IEnumerable<CouplingMethod> problems, IEnumerable<Graphic> graphics, IEnumerable<GraphicOptions> gropts)
        {
            InitializeComponent();
            
            /*user defined initialize*/
            var b = true;
            foreach (var problem in problems)
            {
                problemsD.Add(problem.Name, problem);
                problemCB.Items.Add(problem);
                if (!b) continue;
                    if (problem.Polygon != null) domainsCB.Items.Add("Domain");
                    if (problem.FemPolygon != null) domainsCB.Items.Add("FEMDomain");
                    if (problem.BemPolygon != null) domainsCB.Items.Add("BEMDomain");
                    if (domainsCB.Items.Count != 0)
                    {
                        domainsCB.SelectedIndex = 0;
                        functionCB.Items.AddRange(problem.FuncList.ToArray());//may it be?
                    }
                b = false;
            }
            foreach (var graphic in graphics)
            {
                graphicsD.Add(graphic.Name, graphic);
                functionsLB.Items.Add(graphic);
            }
            foreach (var gropt in gropts)
            {
                groptD.Add(gropt.Name, gropt);
                optionsLB.Items.Add(gropt);
            }

            
        }

        private void problemCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            CouplingMethod problem = problemsD[problemCB.SelectedText];
            if (problem.Polygon != null) domainsCB.Items.Add("Domain");
            if (problem.FemPolygon != null) domainsCB.Items.Add("FEMDomain");
            if (problem.BemPolygon != null) domainsCB.Items.Add("BEMDomain");
            
        }

        private void domainsCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            CouplingMethod problem = problemsD[problemCB.SelectedText];
            var obj = sender as ComboBox;
            if (obj.SelectedText=="Domain")
                functionCB.Items.AddRange(problem.FuncList.ToArray());//?
            if (obj.SelectedText == "FEMDomain")
                functionCB.Items.AddRange(problem.FEM.FuncList.ToArray());//?
            if (obj.SelectedText == "BEMDomain")
                functionCB.Items.AddRange(problem.BEM.FuncList.ToArray());//?
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            colorDialog1.Color = textBox3.BackColor;
            if(DialogResult.OK==colorDialog1.ShowDialog())
            {
                textBox3.BackColor = colorDialog1.Color;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CouplingMethod problem = problemsD[problemCB.SelectedText];
            Polygon poly=null;
            switch (domainsCB.SelectedText)
            {
                case "Domain":
                    poly = problem.Polygon;
                    break;
                case "FEMDomain":
                    poly = problem.FEM.Polygon;
                    break;
                case "BEMDomain":
                    poly = problem.BEM.Polygon;
                    break;
            }
 
            GraphicOptions gro=new GraphicOptions(FunctionType.ConstX,poly);
            gro.Name = "default";
            optionsLB.Items.Add(gro);
            optionsLB.SelectedIndex = optionsLB.Items.Count - 1;
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
            textBox1.Text = Name;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            var opt = optionsLB.SelectedItem as GraphicOptions;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    optionsLB.SelectedItem = new GraphicOptions(FunctionType.ConstX, opt.Polygon);
                    ((GraphicOptions) optionsLB.SelectedItem).ConstValue = double.Parse(textBox5.Text);
                    ((GraphicOptions) optionsLB.SelectedItem).Name = textBox1.Text;
                    break;
                case 1:
                    optionsLB.SelectedItem = new GraphicOptions(FunctionType.ConstY, opt.Polygon);
                    ((GraphicOptions)optionsLB.SelectedItem).ConstValue = double.Parse(textBox4.Text);
                    ((GraphicOptions)optionsLB.SelectedItem).Name = textBox1.Text;
                    break;
            }    
        }

        private void button5_Click(object sender, EventArgs e)
        {
            optionsLB.Items.RemoveAt(optionsLB.SelectedIndex);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var opt = optionsLB.SelectedItem as GraphicOptions;
            CouplingMethod problem = problemsD[problemCB.SelectedText];
            Polygon poly = null;
            switch (domainsCB.SelectedText)
            {
                case "Domain":
                    poly = problem.Polygon;
                    break;
                case "FEMDomain":
                    poly = problem.FEM.Polygon;
                    break;
                case "BEMDomain":
                    poly = problem.BEM.Polygon;
                    break;
            }
 
        }

 
        
    }
}
