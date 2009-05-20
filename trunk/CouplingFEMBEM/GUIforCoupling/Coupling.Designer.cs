namespace GUIforCoupling
{
    partial class Coupling
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Coupling));
            this.Canvas = new ChartDirector.WinChartViewer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadWorkSpaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.problemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.functionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.problemCB = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.showFigurebtn = new System.Windows.Forms.ToolStripButton();
            this.showMeshbtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.initProblembtn = new System.Windows.Forms.ToolStripButton();
            this.runProblembtn = new System.Windows.Forms.ToolStripButton();
            this.solveProblembtn = new System.Windows.Forms.ToolStripButton();
            this.doAllbtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.grOptionsCB = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.surfaceFunctionsCB = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.saveTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.toolStripContainer2.ContentPanel.SuspendLayout();
            this.toolStripContainer2.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.Canvas.ContextMenuStrip = this.contextMenu;
            this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Canvas.Location = new System.Drawing.Point(0, 0);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(643, 303);
            this.Canvas.TabIndex = 0;
            this.Canvas.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.problemsToolStripMenuItem,
            this.functionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(643, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.loadWorkSpaceToolStripMenuItem,
            this.saveWorkspaceToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.openToolStripMenuItem.Text = "Load Problem";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // loadWorkSpaceToolStripMenuItem
            // 
            this.loadWorkSpaceToolStripMenuItem.Name = "loadWorkSpaceToolStripMenuItem";
            this.loadWorkSpaceToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.loadWorkSpaceToolStripMenuItem.Text = "Load WorkSpace";
            this.loadWorkSpaceToolStripMenuItem.Click += new System.EventHandler(this.loadWorkSpaceToolStripMenuItem_Click);
            // 
            // saveWorkspaceToolStripMenuItem
            // 
            this.saveWorkspaceToolStripMenuItem.Name = "saveWorkspaceToolStripMenuItem";
            this.saveWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.saveWorkspaceToolStripMenuItem.Text = "Save Workspace";
            this.saveWorkspaceToolStripMenuItem.Click += new System.EventHandler(this.saveWorkspaceToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // problemsToolStripMenuItem
            // 
            this.problemsToolStripMenuItem.Name = "problemsToolStripMenuItem";
            this.problemsToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.problemsToolStripMenuItem.Text = "Problems";
            // 
            // functionsToolStripMenuItem
            // 
            this.functionsToolStripMenuItem.Name = "functionsToolStripMenuItem";
            this.functionsToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.functionsToolStripMenuItem.Text = "Functions";
            this.functionsToolStripMenuItem.Click += new System.EventHandler(this.functionsToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel3,
            this.problemCB,
            this.toolStripLabel1,
            this.showFigurebtn,
            this.showMeshbtn,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.initProblembtn,
            this.runProblembtn,
            this.solveProblembtn,
            this.doAllbtn,
            this.toolStripSeparator2});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(592, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(45, 22);
            this.toolStripLabel3.Text = "Problem";
            // 
            // problemCB
            // 
            this.problemCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.problemCB.Name = "problemCB";
            this.problemCB.Size = new System.Drawing.Size(120, 25);
            this.problemCB.SelectedIndexChanged += new System.EventHandler(this.problemCB_SelectedIndexChanged);
            this.problemCB.Click += new System.EventHandler(this.problemCB_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(36, 22);
            this.toolStripLabel1.Text = "Show";
            // 
            // showFigurebtn
            // 
            this.showFigurebtn.Image = ((System.Drawing.Image)(resources.GetObject("showFigurebtn.Image")));
            this.showFigurebtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showFigurebtn.Name = "showFigurebtn";
            this.showFigurebtn.Size = new System.Drawing.Size(57, 22);
            this.showFigurebtn.Text = "Figure";
            this.showFigurebtn.Click += new System.EventHandler(this.showFigurebtn_Click);
            // 
            // showMeshbtn
            // 
            this.showMeshbtn.Image = ((System.Drawing.Image)(resources.GetObject("showMeshbtn.Image")));
            this.showMeshbtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showMeshbtn.Name = "showMeshbtn";
            this.showMeshbtn.Size = new System.Drawing.Size(53, 22);
            this.showMeshbtn.Text = "Mesh";
            this.showMeshbtn.Click += new System.EventHandler(this.showMeshbtn_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(60, 22);
            this.toolStripLabel2.Text = "Operations";
            // 
            // initProblembtn
            // 
            this.initProblembtn.Image = ((System.Drawing.Image)(resources.GetObject("initProblembtn.Image")));
            this.initProblembtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.initProblembtn.Name = "initProblembtn";
            this.initProblembtn.Size = new System.Drawing.Size(40, 22);
            this.initProblembtn.Text = "Init";
            this.initProblembtn.Click += new System.EventHandler(this.initProblembtn_Click);
            // 
            // runProblembtn
            // 
            this.runProblembtn.Image = ((System.Drawing.Image)(resources.GetObject("runProblembtn.Image")));
            this.runProblembtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runProblembtn.Name = "runProblembtn";
            this.runProblembtn.Size = new System.Drawing.Size(46, 22);
            this.runProblembtn.Text = "Run";
            this.runProblembtn.Click += new System.EventHandler(this.runProblembtn_Click);
            // 
            // solveProblembtn
            // 
            this.solveProblembtn.Image = ((System.Drawing.Image)(resources.GetObject("solveProblembtn.Image")));
            this.solveProblembtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.solveProblembtn.Name = "solveProblembtn";
            this.solveProblembtn.Size = new System.Drawing.Size(54, 22);
            this.solveProblembtn.Text = "Solve";
            this.solveProblembtn.Click += new System.EventHandler(this.solveProblembtn_Click);
            // 
            // doAllbtn
            // 
            this.doAllbtn.Image = ((System.Drawing.Image)(resources.GetObject("doAllbtn.Image")));
            this.doAllbtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.doAllbtn.Name = "doAllbtn";
            this.doAllbtn.Size = new System.Drawing.Size(55, 22);
            this.doAllbtn.Text = "Do All";
            this.doAllbtn.Click += new System.EventHandler(this.doAllbtn_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 402);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(643, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveTSMI});
            this.contextMenu.Name = "contextMenuStrip1";
            this.contextMenu.Size = new System.Drawing.Size(125, 26);
            // 
            // toolStripContainer2
            // 
            // 
            // toolStripContainer2.ContentPanel
            // 
            this.toolStripContainer2.ContentPanel.Controls.Add(this.Canvas);
            this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(643, 303);
            this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer2.Location = new System.Drawing.Point(0, 24);
            this.toolStripContainer2.Name = "toolStripContainer2";
            this.toolStripContainer2.Size = new System.Drawing.Size(643, 378);
            this.toolStripContainer2.TabIndex = 6;
            this.toolStripContainer2.Text = "toolStripContainer2";
            // 
            // toolStripContainer2.TopToolStripPanel
            // 
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.toolStrip3);
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.toolStrip2);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel4,
            this.grOptionsCB,
            this.toolStripSeparator3,
            this.toolStripButton2,
            this.toolStripButton1});
            this.toolStrip2.Location = new System.Drawing.Point(3, 50);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(393, 25);
            this.toolStrip2.TabIndex = 3;
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(85, 22);
            this.toolStripLabel4.Text = "Graphic Options";
            // 
            // grOptionsCB
            // 
            this.grOptionsCB.Name = "grOptionsCB";
            this.grOptionsCB.Size = new System.Drawing.Size(121, 25);
            this.grOptionsCB.SelectedIndexChanged += new System.EventHandler(this.grOptionsCB_SelectedIndexChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(54, 22);
            this.toolStripButton2.Text = "Draw";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(113, 22);
            this.toolStripButton1.Text = "Function Manager";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStrip3
            // 
            this.toolStrip3.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel5,
            this.surfaceFunctionsCB,
            this.toolStripButton3});
            this.toolStrip3.Location = new System.Drawing.Point(3, 25);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(206, 25);
            this.toolStrip3.TabIndex = 4;
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(48, 22);
            this.toolStripLabel5.Text = "Function";
            // 
            // surfaceFunctionsCB
            // 
            this.surfaceFunctionsCB.Name = "surfaceFunctionsCB";
            this.surfaceFunctionsCB.Size = new System.Drawing.Size(121, 25);
            this.surfaceFunctionsCB.SelectedIndexChanged += new System.EventHandler(this.surfaceFunctionsCB_SelectedIndexChanged);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Draw Surface";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // saveTSMI
            // 
            this.saveTSMI.Name = "saveTSMI";
            this.saveTSMI.Size = new System.Drawing.Size(152, 22);
            this.saveTSMI.Text = "Save As";
            this.saveTSMI.Click += new System.EventHandler(this.saveTSMI_Click);
            // 
            // Coupling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 424);
            this.Controls.Add(this.toolStripContainer2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Coupling";
            this.Text = "Coupling";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.toolStripContainer2.ContentPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.PerformLayout();
            this.toolStripContainer2.ResumeLayout(false);
            this.toolStripContainer2.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChartDirector.WinChartViewer Canvas;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton showMeshbtn;
        private System.Windows.Forms.ToolStripButton showFigurebtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton initProblembtn;
        private System.Windows.Forms.ToolStripButton solveProblembtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripButton runProblembtn;
        private System.Windows.Forms.ToolStripButton doAllbtn;
        private System.Windows.Forms.ToolStripMenuItem saveWorkspaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadWorkSpaceToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripComboBox problemCB;
        private System.Windows.Forms.ToolStripMenuItem problemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem functionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer2;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripComboBox grOptionsCB;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripComboBox surfaceFunctionsCB;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripMenuItem saveTSMI;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;

    }
}

