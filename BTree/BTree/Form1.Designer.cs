namespace BTree
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.CreateNewDB = new System.Windows.Forms.ToolStripButton();
            this.OpenDB = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Logs = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.FileName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateNewDB,
            this.OpenDB});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // CreateNewDB
            // 
            this.CreateNewDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CreateNewDB.Image = ((System.Drawing.Image)(resources.GetObject("CreateNewDB.Image")));
            this.CreateNewDB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CreateNewDB.Name = "CreateNewDB";
            this.CreateNewDB.Size = new System.Drawing.Size(23, 22);
            this.CreateNewDB.Text = "Create New DataBase";
            this.CreateNewDB.Click += new System.EventHandler(this.CreateNewDB_Click);
            // 
            // OpenDB
            // 
            this.OpenDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OpenDB.Image = ((System.Drawing.Image)(resources.GetObject("OpenDB.Image")));
            this.OpenDB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenDB.Name = "OpenDB";
            this.OpenDB.Size = new System.Drawing.Size(23, 22);
            this.OpenDB.Text = "Open Existing Database";
            this.OpenDB.Click += new System.EventHandler(this.OpenDB_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(77, 142);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(615, 23);
            this.textBox1.TabIndex = 1;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // Logs
            // 
            this.Logs.AutoSize = true;
            this.Logs.Location = new System.Drawing.Point(76, 276);
            this.Logs.Name = "Logs";
            this.Logs.Size = new System.Drawing.Size(0, 15);
            this.Logs.TabIndex = 2;
            this.Logs.TextChanged += new System.EventHandler(this.Logs_TextChanged);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(468, 201);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(309, 198);
            this.treeView1.TabIndex = 3;
            // 
            // FileName
            // 
            this.FileName.AutoSize = true;
            this.FileName.Location = new System.Drawing.Point(77, 115);
            this.FileName.Name = "FileName";
            this.FileName.Size = new System.Drawing.Size(78, 15);
            this.FileName.TabIndex = 4;
            this.FileName.Text = "File Selected: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(468, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Key Output";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FileName);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.Logs);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton CreateNewDB;
        private System.Windows.Forms.ToolStripButton OpenDB;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label Logs;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label FileName;
        private System.Windows.Forms.Label label1;
    }
}
