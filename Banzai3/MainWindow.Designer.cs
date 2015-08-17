namespace Banzai3
{
    partial class MainWindow
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
            this.panelScroll = new System.Windows.Forms.Panel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnSelect = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
            this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
            this.btnUndo = new System.Windows.Forms.ToolStripButton();
            this.btnRedo = new System.Windows.Forms.ToolStripButton();
            this.btnCrop = new System.Windows.Forms.ToolStripButton();
            this.btnAddLeft = new System.Windows.Forms.ToolStripButton();
            this.btnAddRight = new System.Windows.Forms.ToolStripButton();
            this.btnAddTop = new System.Windows.Forms.ToolStripButton();
            this.btnAddDown = new System.Windows.Forms.ToolStripButton();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.panelCross = new Banzai3.PanelCross();
            this.panelScroll.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelScroll
            // 
            this.panelScroll.AutoScroll = true;
            this.panelScroll.Controls.Add(this.panelCross);
            this.panelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScroll.Location = new System.Drawing.Point(0, 53);
            this.panelScroll.Name = "panelScroll";
            this.panelScroll.Size = new System.Drawing.Size(803, 446);
            this.panelScroll.TabIndex = 1;
            this.panelScroll.Resize += new System.EventHandler(this.crossScroll_Resize);
            // 
            // toolStrip
            // 
            this.toolStrip.AutoSize = false;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSelect,
            this.btnSave,
            this.btnZoomIn,
            this.btnZoomOut,
            this.btnUndo,
            this.btnRedo,
            this.btnCrop,
            this.btnAddLeft,
            this.btnAddRight,
            this.btnAddTop,
            this.btnAddDown,
            this.btnClear});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(803, 53);
            this.toolStrip.TabIndex = 0;
            // 
            // btnSelect
            // 
            this.btnSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSelect.Image = global::Banzai3.Properties.Resources.icoFolder;
            this.btnSelect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(52, 50);
            this.btnSelect.Text = "Open";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = false;
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = global::Banzai3.Properties.Resources.icoSave;
            this.btnSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(50, 50);
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.AutoSize = false;
            this.btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomIn.Image = global::Banzai3.Properties.Resources.icoZoomIn;
            this.btnZoomIn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(50, 50);
            this.btnZoomIn.Text = "Clear";
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.AutoSize = false;
            this.btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomOut.Image = global::Banzai3.Properties.Resources.icoZoomOut;
            this.btnZoomOut.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(50, 50);
            this.btnZoomOut.Text = "Clear";
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUndo.Image = global::Banzai3.Properties.Resources.icoUndo;
            this.btnUndo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(52, 50);
            this.btnUndo.Text = "Undo";
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnRedo
            // 
            this.btnRedo.AutoSize = false;
            this.btnRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRedo.Image = global::Banzai3.Properties.Resources.icoRedo;
            this.btnRedo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(50, 50);
            this.btnRedo.Text = "Redo";
            this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
            // 
            // btnCrop
            // 
            this.btnCrop.AutoSize = false;
            this.btnCrop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCrop.Image = global::Banzai3.Properties.Resources.icoCut;
            this.btnCrop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnCrop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCrop.Name = "btnCrop";
            this.btnCrop.Size = new System.Drawing.Size(50, 50);
            this.btnCrop.Text = "Crop";
            this.btnCrop.Click += new System.EventHandler(this.btnCrop_Click);
            // 
            // btnAddLeft
            // 
            this.btnAddLeft.AutoSize = false;
            this.btnAddLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddLeft.Image = global::Banzai3.Properties.Resources.icoAddLeft;
            this.btnAddLeft.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAddLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddLeft.Name = "btnAddLeft";
            this.btnAddLeft.Size = new System.Drawing.Size(50, 50);
            this.btnAddLeft.Text = "Add Left";
            this.btnAddLeft.Click += new System.EventHandler(this.btnAddLeft_Click);
            // 
            // btnAddRight
            // 
            this.btnAddRight.AutoSize = false;
            this.btnAddRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddRight.Image = global::Banzai3.Properties.Resources.icoAddRight;
            this.btnAddRight.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAddRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddRight.Name = "btnAddRight";
            this.btnAddRight.Size = new System.Drawing.Size(50, 50);
            this.btnAddRight.Text = "Add Right";
            this.btnAddRight.Click += new System.EventHandler(this.btnAddRight_Click);
            // 
            // btnAddTop
            // 
            this.btnAddTop.AutoSize = false;
            this.btnAddTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddTop.Image = global::Banzai3.Properties.Resources.icoAddTop;
            this.btnAddTop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAddTop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddTop.Name = "btnAddTop";
            this.btnAddTop.Size = new System.Drawing.Size(50, 50);
            this.btnAddTop.Text = "Add Top";
            this.btnAddTop.Click += new System.EventHandler(this.btnAddTop_Click);
            // 
            // btnAddDown
            // 
            this.btnAddDown.AutoSize = false;
            this.btnAddDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddDown.Image = global::Banzai3.Properties.Resources.icoAddDown;
            this.btnAddDown.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAddDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddDown.Name = "btnAddDown";
            this.btnAddDown.Size = new System.Drawing.Size(50, 50);
            this.btnAddDown.Text = "Add Down";
            this.btnAddDown.Click += new System.EventHandler(this.btnAddDown_Click);
            // 
            // btnClear
            // 
            this.btnClear.AutoSize = false;
            this.btnClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClear.Image = global::Banzai3.Properties.Resources.icoClear;
            this.btnClear.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(50, 50);
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // panelCross
            // 
            this.panelCross.Location = new System.Drawing.Point(101, 111);
            this.panelCross.Margin = new System.Windows.Forms.Padding(0);
            this.panelCross.Name = "panelCross";
            this.panelCross.Size = new System.Drawing.Size(150, 150);
            this.panelCross.TabIndex = 0;
            this.panelCross.Paint += new System.Windows.Forms.PaintEventHandler(this.panelCross_Paint);
            this.panelCross.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelCross_MouseDown);
            this.panelCross.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelCross_MouseMove);
            this.panelCross.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelCross_MouseUp);
            // 
            // MainWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(803, 499);
            this.Controls.Add(this.panelScroll);
            this.Controls.Add(this.toolStrip);
            this.DoubleBuffered = true;
            this.Name = "MainWindow";
            this.Text = "Banzai3";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Banzai3_FormClosed);
            this.Load += new System.EventHandler(this.Banzai3_Load);
            this.panelScroll.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelScroll;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnUndo;
        private System.Windows.Forms.ToolStripButton btnRedo;
        private Banzai3.PanelCross panelCross;
        private System.Windows.Forms.ToolStripButton btnSelect;
        private System.Windows.Forms.ToolStripButton btnAddDown;
        private System.Windows.Forms.ToolStripButton btnCrop;
        private System.Windows.Forms.ToolStripButton btnAddLeft;
        private System.Windows.Forms.ToolStripButton btnAddRight;
        private System.Windows.Forms.ToolStripButton btnAddTop;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnClear;
        private System.Windows.Forms.ToolStripButton btnZoomIn;
        private System.Windows.Forms.ToolStripButton btnZoomOut;
    }
}

