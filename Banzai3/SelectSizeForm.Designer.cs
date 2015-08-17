namespace Banzai3
{
    partial class SelectSizeForm
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
            System.Windows.Forms.Label label1;
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.controlWidth = new System.Windows.Forms.NumericUpDown();
            this.controlHeight = new System.Windows.Forms.NumericUpDown();
            this.labelWidth = new System.Windows.Forms.Label();
            this.labelHeight = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.controlWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.controlHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(277, 74);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(32, 37);
            label1.TabIndex = 3;
            label1.Text = "x";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(87, 171);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(180, 50);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(320, 171);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(180, 50);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // controlWidth
            // 
            this.controlWidth.Location = new System.Drawing.Point(151, 70);
            this.controlWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.controlWidth.Name = "controlWidth";
            this.controlWidth.Size = new System.Drawing.Size(120, 44);
            this.controlWidth.TabIndex = 2;
            this.controlWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // controlHeight
            // 
            this.controlHeight.Location = new System.Drawing.Point(315, 70);
            this.controlHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.controlHeight.Name = "controlHeight";
            this.controlHeight.Size = new System.Drawing.Size(120, 44);
            this.controlHeight.TabIndex = 4;
            this.controlHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Location = new System.Drawing.Point(144, 30);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(92, 37);
            this.labelWidth.TabIndex = 0;
            this.labelWidth.Text = "width";
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(313, 30);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(104, 37);
            this.labelHeight.TabIndex = 1;
            this.labelHeight.Text = "height";
            // 
            // SelectSizeForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(587, 233);
            this.Controls.Add(this.labelHeight);
            this.Controls.Add(this.labelWidth);
            this.Controls.Add(this.controlHeight);
            this.Controls.Add(this.controlWidth);
            this.Controls.Add(label1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SelectSizeForm";
            this.Text = "Select size";
            this.Load += new System.EventHandler(this.SelectSize_Load);
            ((System.ComponentModel.ISupportInitialize)(this.controlWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.controlHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown controlWidth;
        private System.Windows.Forms.NumericUpDown controlHeight;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.Label labelHeight;
    }
}