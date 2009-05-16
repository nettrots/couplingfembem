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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Coupling));
            this.Canvas = new ChartDirector.WinChartViewer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadWorkSpaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Canvas.Location = new System.Drawing.Point(0, 24);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(643, 400);
            this.Canvas.TabIndex = 0;
            this.Canvas.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
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
            // 
            // saveWorkspaceToolStripMenuItem
            // 
            this.saveWorkspaceToolStripMenuItem.Name = "saveWorkspaceToolStripMenuItem";
            this.saveWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.saveWorkspaceToolStripMenuItem.Text = "Save Workspace";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // toolStrip1
            // 
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
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(643, 25);
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
            this.problemCB.Name = "problemCB";
            this.problemCB.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(33, 22);
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
            this.showMeshbtn.Size = new System.Drawing.Size(52, 22);
            this.showMeshbtn.Text = "Mesh";
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
            this.initProblembtn.Size = new System.Drawing.Size(43, 22);
            this.initProblembtn.Text = "Init";
            // 
            // runProblembtn
            // 
            this.runProblembtn.Image = ((System.Drawing.Image)(resources.GetObject("runProblembtn.Image")));
            this.runProblembtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runProblembtn.Name = "runProblembtn";
            this.runProblembtn.Size = new System.Drawing.Size(46, 22);
            this.runProblembtn.Text = "Run";
            // 
            // solveProblembtn
            // 
            this.solveProblembtn.Image = ((System.Drawing.Image)(resources.GetObject("solveProblembtn.Image")));
            this.solveProblembtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.solveProblembtn.Name = "solveProblembtn";
            this.solveProblembtn.Size = new System.Drawing.Size(53, 22);
            this.solveProblembtn.Text = "Solve";
            // 
            // doAllbtn
            // 
            this.doAllbtn.Image = ((System.Drawing.Image)(resources.GetObject("doAllbtn.Image")));
            this.doAllbtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.doAllbtn.Name = "doAllbtn";
            this.doAllbtn.Size = new System.Drawing.Size(54, 22);
            this.doAllbtn.Text = "Do All";
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
            // Coupling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 424);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.Canvas);
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
        private System.Windows.Forms.ToolStripComboBox problemCB;
        private System.Windows.Forms.ToolStripButton runProblembtn;
        private System.Windows.Forms.ToolStripButton doAllbtn;
        private System.Windows.Forms.ToolStripMenuItem saveWorkspaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadWorkSpaceToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;

    }
}

