namespace Paint
{
    partial class ComponentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComponentForm));
            this.ComponentView = new Control.BufferedTreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.CreateGroupButton = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComponentView
            // 
            this.ComponentView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComponentView.HideSelection = false;
            this.ComponentView.Location = new System.Drawing.Point(0, 0);
            this.ComponentView.Name = "ComponentView";
            this.ComponentView.ShowNodeToolTips = true;
            this.ComponentView.Size = new System.Drawing.Size(220, 447);
            this.ComponentView.TabIndex = 0;
            this.ComponentView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ComponentView_NodeMouseClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateGroupButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(220, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // CreateGroupButton
            // 
            this.CreateGroupButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CreateGroupButton.Image = ((System.Drawing.Image)(resources.GetObject("CreateGroupButton.Image")));
            this.CreateGroupButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CreateGroupButton.Name = "CreateGroupButton";
            this.CreateGroupButton.Size = new System.Drawing.Size(23, 22);
            this.CreateGroupButton.Text = "toolStripButton1";
            this.CreateGroupButton.Click += new System.EventHandler(this.CreateGroupButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ComponentView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(220, 447);
            this.panel1.TabIndex = 3;
            // 
            // ComponentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 472);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ComponentForm";
            this.ShowInTaskbar = false;
            this.Text = "ComponentForm";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Paint.Control.BufferedTreeView ComponentView;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton CreateGroupButton;
    }
}