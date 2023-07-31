
namespace Paint
{
    partial class PaintForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaintForm));
            this.PaintArea = new System.Windows.Forms.PictureBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripVisibleButton = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripVisibleButton = new System.Windows.Forms.ToolStripMenuItem();
            this.groupsVisibleButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.ToolStripOpenButton = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSaveButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.UndoButton = new System.Windows.Forms.ToolStripButton();
            this.RedoButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ClearButton = new System.Windows.Forms.ToolStripButton();
            this.DeleteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolsLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolSelectButton = new System.Windows.Forms.ToolStripButton();
            this.toolDrawButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.shapesLabel = new System.Windows.Forms.ToolStripLabel();
            this.shapeRectangleButton = new System.Windows.Forms.ToolStripButton();
            this.shapeEllipseButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.SelectedToolLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PaintOperationLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.PaintArea)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PaintArea
            // 
            this.PaintArea.BackColor = System.Drawing.Color.White;
            this.PaintArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PaintArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PaintArea.Location = new System.Drawing.Point(0, 0);
            this.PaintArea.Name = "PaintArea";
            this.PaintArea.Size = new System.Drawing.Size(704, 370);
            this.PaintArea.TabIndex = 0;
            this.PaintArea.TabStop = false;
            this.PaintArea.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintArea_Paint);
            this.PaintArea.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PaintArea_MouseDown);
            this.PaintArea.MouseLeave += new System.EventHandler(this.PaintArea_MouseLeave);
            this.PaintArea.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PaintArea_MouseMove);
            this.PaintArea.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PaintArea_MouseUp);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(704, 24);
            this.menuStrip.TabIndex = 12;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenToolStripMenuItem,
            this.SaveToolStripMenuItem,
            this.ExportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.Name = "openToolStripMenuItem";
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.OpenToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.SaveToolStripMenuItem.Text = "Save";
            // 
            // exportToolStripMenuItem
            // 
            this.ExportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.ExportToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.ExportToolStripMenuItem.Text = "Export as image";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripVisibleButton,
            this.statusStripVisibleButton,
            this.groupsVisibleButton});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // toolStripVisibleButton
            // 
            this.toolStripVisibleButton.Checked = true;
            this.toolStripVisibleButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripVisibleButton.Name = "toolStripVisibleButton";
            this.toolStripVisibleButton.Size = new System.Drawing.Size(123, 22);
            this.toolStripVisibleButton.Text = "Toolstrip";
            // 
            // statusStripVisibleButton
            // 
            this.statusStripVisibleButton.Checked = true;
            this.statusStripVisibleButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.statusStripVisibleButton.Name = "statusStripVisibleButton";
            this.statusStripVisibleButton.Size = new System.Drawing.Size(123, 22);
            this.statusStripVisibleButton.Text = "Statusbar";
            // 
            // groupsVisibleButton
            // 
            this.groupsVisibleButton.Checked = true;
            this.groupsVisibleButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.groupsVisibleButton.Name = "groupsVisibleButton";
            this.groupsVisibleButton.Size = new System.Drawing.Size(123, 22);
            this.groupsVisibleButton.Text = "Groups";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripOpenButton,
            this.ToolStripSaveButton,
            this.toolStripSeparator1,
            this.UndoButton,
            this.RedoButton,
            this.toolStripSeparator2,
            this.ClearButton,
            this.DeleteButton,
            this.toolStripSeparator3,
            this.toolsLabel,
            this.toolSelectButton,
            this.toolDrawButton,
            this.toolStripSeparator4,
            this.shapesLabel,
            this.shapeRectangleButton,
            this.shapeEllipseButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(704, 25);
            this.toolStrip.TabIndex = 13;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripOpenButton
            // 
            this.ToolStripOpenButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripOpenButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripOpenButton.Image")));
            this.ToolStripOpenButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripOpenButton.Name = "toolStripOpenButton";
            this.ToolStripOpenButton.Size = new System.Drawing.Size(23, 22);
            this.ToolStripOpenButton.Text = "Open";
            // 
            // toolStripSaveButton
            // 
            this.ToolStripSaveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripSaveButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSaveButton.Image")));
            this.ToolStripSaveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripSaveButton.Name = "toolStripSaveButton";
            this.ToolStripSaveButton.Size = new System.Drawing.Size(23, 22);
            this.ToolStripSaveButton.Text = "Save";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // UndoButton
            // 
            this.UndoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.UndoButton.Image = ((System.Drawing.Image)(resources.GetObject("UndoButton.Image")));
            this.UndoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.UndoButton.Name = "UndoButton";
            this.UndoButton.Size = new System.Drawing.Size(23, 22);
            this.UndoButton.Text = "Undo";
            // 
            // RedoButton
            // 
            this.RedoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RedoButton.Image = ((System.Drawing.Image)(resources.GetObject("RedoButton.Image")));
            this.RedoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RedoButton.Name = "RedoButton";
            this.RedoButton.Size = new System.Drawing.Size(23, 22);
            this.RedoButton.Text = "Redo";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // ClearButton
            // 
            this.ClearButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ClearButton.Image = ((System.Drawing.Image)(resources.GetObject("ClearButton.Image")));
            this.ClearButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(23, 22);
            this.ClearButton.Text = "Clear";
            // 
            // DeleteButton
            // 
            this.DeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteButton.Image")));
            this.DeleteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(23, 22);
            this.DeleteButton.Text = "Delete";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolsLabel
            // 
            this.toolsLabel.Name = "toolsLabel";
            this.toolsLabel.Size = new System.Drawing.Size(37, 22);
            this.toolsLabel.Text = "Tools:";
            // 
            // toolSelectButton
            // 
            this.toolSelectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolSelectButton.Image = ((System.Drawing.Image)(resources.GetObject("toolSelectButton.Image")));
            this.toolSelectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSelectButton.Name = "toolSelectButton";
            this.toolSelectButton.Size = new System.Drawing.Size(23, 22);
            this.toolSelectButton.Text = "toolStripButton1";
            this.toolSelectButton.ToolTipText = "Select";
            this.toolSelectButton.Click += new System.EventHandler(this.ToolSelectButton_Click);
            // 
            // toolDrawButton
            // 
            this.toolDrawButton.Checked = true;
            this.toolDrawButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolDrawButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolDrawButton.Image = ((System.Drawing.Image)(resources.GetObject("toolDrawButton.Image")));
            this.toolDrawButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolDrawButton.Name = "toolDrawButton";
            this.toolDrawButton.Size = new System.Drawing.Size(23, 22);
            this.toolDrawButton.Text = "toolStripButton2";
            this.toolDrawButton.ToolTipText = "Draw";
            this.toolDrawButton.Click += new System.EventHandler(this.ToolDrawButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // shapesLabel
            // 
            this.shapesLabel.Name = "shapesLabel";
            this.shapesLabel.Size = new System.Drawing.Size(47, 22);
            this.shapesLabel.Text = "Shapes:";
            // 
            // shapeRectangleButton
            // 
            this.shapeRectangleButton.Checked = true;
            this.shapeRectangleButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shapeRectangleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.shapeRectangleButton.Image = ((System.Drawing.Image)(resources.GetObject("shapeRectangleButton.Image")));
            this.shapeRectangleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.shapeRectangleButton.Name = "shapeRectangleButton";
            this.shapeRectangleButton.Size = new System.Drawing.Size(23, 22);
            this.shapeRectangleButton.Text = "toolStripButton1";
            this.shapeRectangleButton.ToolTipText = "Rectangle";
            this.shapeRectangleButton.Click += new System.EventHandler(this.ShapeRectangleButton_Click);
            // 
            // shapeEllipseButton
            // 
            this.shapeEllipseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.shapeEllipseButton.Image = ((System.Drawing.Image)(resources.GetObject("shapeEllipseButton.Image")));
            this.shapeEllipseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.shapeEllipseButton.Name = "shapeEllipseButton";
            this.shapeEllipseButton.Size = new System.Drawing.Size(23, 22);
            this.shapeEllipseButton.Text = "toolStripButton2";
            this.shapeEllipseButton.ToolTipText = "Ellipse";
            this.shapeEllipseButton.Click += new System.EventHandler(this.ShapeEllipseButton_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectedToolLabel,
            this.PaintOperationLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 419);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(704, 22);
            this.statusStrip.TabIndex = 14;
            this.statusStrip.Text = "statusStrip1";
            // 
            // SelectedToolLabel
            // 
            this.SelectedToolLabel.Name = "SelectedToolLabel";
            this.SelectedToolLabel.Size = new System.Drawing.Size(34, 17);
            this.SelectedToolLabel.Text = "Draw";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.PaintArea);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(704, 370);
            this.panel1.TabIndex = 15;
            // 
            // PaintOperationLabel
            // 
            this.PaintOperationLabel.Name = "PaintOperationLabel";
            this.PaintOperationLabel.Size = new System.Drawing.Size(26, 17);
            this.PaintOperationLabel.Text = "Idle";
            // 
            // PaintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 441);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(720, 480);
            this.Name = "PaintForm";
            this.Text = "Simple Paint";
            this.Load += new System.EventHandler(this.PaintForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PaintForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.PaintArea)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PaintArea;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton UndoButton;
        private System.Windows.Forms.ToolStripButton RedoButton;
        private System.Windows.Forms.ToolStripButton ToolStripOpenButton;
        private System.Windows.Forms.ToolStripButton ToolStripSaveButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel SelectedToolLabel;
        private System.Windows.Forms.ToolStripButton ClearButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolSelectButton;
        private System.Windows.Forms.ToolStripButton toolDrawButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton shapeRectangleButton;
        private System.Windows.Forms.ToolStripButton shapeEllipseButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripLabel shapesLabel;
        private System.Windows.Forms.ToolStripLabel toolsLabel;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statusStripVisibleButton;
        private System.Windows.Forms.ToolStripMenuItem toolStripVisibleButton;
        private System.Windows.Forms.ToolStripMenuItem groupsVisibleButton;
        private System.Windows.Forms.ToolStripButton DeleteButton;
        private System.Windows.Forms.ToolStripStatusLabel PaintOperationLabel;
    }
}

