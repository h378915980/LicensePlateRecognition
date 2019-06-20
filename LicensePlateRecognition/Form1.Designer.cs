namespace LicensePlateRecognition
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.listInputImage = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.butInputImage = new System.Windows.Forms.Button();
            this.tabShowDiffImage = new System.Windows.Forms.TabControl();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.inputImageFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.imgListSplitImage = new System.Windows.Forms.ImageList(this.components);
            this.listShowSplitImage = new System.Windows.Forms.ListView();
            this.butSaveSplitImage = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.HeightUp = new System.Windows.Forms.NumericUpDown();
            this.HeightLow = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.WidthUp = new System.Windows.Forms.NumericUpDown();
            this.WidthLow = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.HeightDividWidthLow = new System.Windows.Forms.NumericUpDown();
            this.HeightDividWidthUp = new System.Windows.Forms.NumericUpDown();
            this.tabShowDiffImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeightUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WidthUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WidthLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightDividWidthLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightDividWidthUp)).BeginInit();
            this.SuspendLayout();
            // 
            // listInputImage
            // 
            this.listInputImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listInputImage.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listInputImage.Location = new System.Drawing.Point(0, 3);
            this.listInputImage.Name = "listInputImage";
            this.listInputImage.Size = new System.Drawing.Size(225, 512);
            this.listInputImage.TabIndex = 0;
            this.listInputImage.UseCompatibleStateImageBehavior = false;
            this.listInputImage.View = System.Windows.Forms.View.List;
            this.listInputImage.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 640;
            // 
            // butInputImage
            // 
            this.butInputImage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.butInputImage.Location = new System.Drawing.Point(0, 517);
            this.butInputImage.Name = "butInputImage";
            this.butInputImage.Size = new System.Drawing.Size(212, 32);
            this.butInputImage.TabIndex = 1;
            this.butInputImage.Text = "打开图片文件夹";
            this.butInputImage.UseVisualStyleBackColor = true;
            this.butInputImage.Click += new System.EventHandler(this.InputImage);
            // 
            // tabShowDiffImage
            // 
            this.tabShowDiffImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabShowDiffImage.Controls.Add(this.tabPage12);
            this.tabShowDiffImage.Location = new System.Drawing.Point(3, 3);
            this.tabShowDiffImage.Name = "tabShowDiffImage";
            this.tabShowDiffImage.SelectedIndex = 0;
            this.tabShowDiffImage.Size = new System.Drawing.Size(701, 312);
            this.tabShowDiffImage.TabIndex = 2;
            // 
            // tabPage12
            // 
            this.tabPage12.Location = new System.Drawing.Point(4, 25);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Size = new System.Drawing.Size(693, 283);
            this.tabPage12.TabIndex = 0;
            this.tabPage12.Text = "原图";
            this.tabPage12.UseVisualStyleBackColor = true;
            // 
            // imgListSplitImage
            // 
            this.imgListSplitImage.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imgListSplitImage.ImageSize = new System.Drawing.Size(48, 24);
            this.imgListSplitImage.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // listShowSplitImage
            // 
            this.listShowSplitImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.listShowSplitImage.LargeImageList = this.imgListSplitImage;
            this.listShowSplitImage.Location = new System.Drawing.Point(0, 0);
            this.listShowSplitImage.Name = "listShowSplitImage";
            this.listShowSplitImage.Size = new System.Drawing.Size(712, 166);
            this.listShowSplitImage.TabIndex = 4;
            this.listShowSplitImage.UseCompatibleStateImageBehavior = false;
            // 
            // butSaveSplitImage
            // 
            this.butSaveSplitImage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.butSaveSplitImage.Location = new System.Drawing.Point(0, 549);
            this.butSaveSplitImage.Name = "butSaveSplitImage";
            this.butSaveSplitImage.Size = new System.Drawing.Size(212, 32);
            this.butSaveSplitImage.TabIndex = 5;
            this.butSaveSplitImage.Text = "批量处理";
            this.butSaveSplitImage.UseVisualStyleBackColor = true;
            this.butSaveSplitImage.Click += new System.EventHandler(this.butSaveSplitImage_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listInputImage);
            this.splitContainer1.Panel1.Controls.Add(this.butInputImage);
            this.splitContainer1.Panel1.Controls.Add(this.butSaveSplitImage);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(946, 585);
            this.splitContainer1.SplitterDistance = 216;
            this.splitContainer1.TabIndex = 7;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabShowDiffImage);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Panel2.Controls.Add(this.listShowSplitImage);
            this.splitContainer2.Size = new System.Drawing.Size(716, 575);
            this.splitContainer2.SplitterDistance = 320;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.HeightUp);
            this.groupBox1.Controls.Add(this.HeightLow);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.WidthUp);
            this.groupBox1.Controls.Add(this.WidthLow);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.HeightDividWidthLow);
            this.groupBox1.Controls.Add(this.HeightDividWidthUp);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(0, 172);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(712, 274);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "调整处理过程中的参数";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(404, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 17;
            this.label6.Text = "高最高值";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(129, 127);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 15);
            this.label7.TabIndex = 16;
            this.label7.Text = "高最低值";
            // 
            // HeightUp
            // 
            this.HeightUp.Location = new System.Drawing.Point(521, 117);
            this.HeightUp.Maximum = new decimal(new int[] {
            960,
            0,
            0,
            0});
            this.HeightUp.Name = "HeightUp";
            this.HeightUp.Size = new System.Drawing.Size(120, 25);
            this.HeightUp.TabIndex = 15;
            this.HeightUp.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.HeightUp.ValueChanged += new System.EventHandler(this.HeightUp_ValueChanged);
            // 
            // HeightLow
            // 
            this.HeightLow.Location = new System.Drawing.Point(247, 117);
            this.HeightLow.Maximum = new decimal(new int[] {
            960,
            0,
            0,
            0});
            this.HeightLow.Name = "HeightLow";
            this.HeightLow.Size = new System.Drawing.Size(120, 25);
            this.HeightLow.TabIndex = 14;
            this.HeightLow.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.HeightLow.ValueChanged += new System.EventHandler(this.HeightLow_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(404, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 13;
            this.label5.Text = "宽最高值";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(129, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 12;
            this.label4.Text = "宽最低值";
            // 
            // WidthUp
            // 
            this.WidthUp.Location = new System.Drawing.Point(521, 73);
            this.WidthUp.Maximum = new decimal(new int[] {
            1280,
            0,
            0,
            0});
            this.WidthUp.Name = "WidthUp";
            this.WidthUp.Size = new System.Drawing.Size(120, 25);
            this.WidthUp.TabIndex = 11;
            this.WidthUp.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.WidthUp.ValueChanged += new System.EventHandler(this.WidthUp_ValueChanged);
            // 
            // WidthLow
            // 
            this.WidthLow.Location = new System.Drawing.Point(247, 73);
            this.WidthLow.Maximum = new decimal(new int[] {
            1280,
            0,
            0,
            0});
            this.WidthLow.Name = "WidthLow";
            this.WidthLow.Size = new System.Drawing.Size(120, 25);
            this.WidthLow.TabIndex = 10;
            this.WidthLow.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.WidthLow.ValueChanged += new System.EventHandler(this.WidthLow_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(404, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "高宽比最高值";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(129, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "高宽比最低值";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "外接矩形：";
            // 
            // HeightDividWidthLow
            // 
            this.HeightDividWidthLow.DecimalPlaces = 2;
            this.HeightDividWidthLow.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.HeightDividWidthLow.Location = new System.Drawing.Point(247, 25);
            this.HeightDividWidthLow.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.HeightDividWidthLow.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.HeightDividWidthLow.Name = "HeightDividWidthLow";
            this.HeightDividWidthLow.Size = new System.Drawing.Size(120, 25);
            this.HeightDividWidthLow.TabIndex = 5;
            this.HeightDividWidthLow.Value = new decimal(new int[] {
            15,
            0,
            0,
            131072});
            this.HeightDividWidthLow.ValueChanged += new System.EventHandler(this.HeightDividWidthLow_ValueChanged);
            // 
            // HeightDividWidthUp
            // 
            this.HeightDividWidthUp.DecimalPlaces = 2;
            this.HeightDividWidthUp.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.HeightDividWidthUp.Location = new System.Drawing.Point(521, 24);
            this.HeightDividWidthUp.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            this.HeightDividWidthUp.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.HeightDividWidthUp.Name = "HeightDividWidthUp";
            this.HeightDividWidthUp.Size = new System.Drawing.Size(120, 25);
            this.HeightDividWidthUp.TabIndex = 6;
            this.HeightDividWidthUp.Value = new decimal(new int[] {
            7,
            0,
            0,
            65536});
            this.HeightDividWidthUp.ValueChanged += new System.EventHandler(this.HeightDividWidthUp_ValueChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 609);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "LicensePlateRecognition";
            this.tabShowDiffImage.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeightUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WidthUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WidthLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightDividWidthLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightDividWidthUp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listInputImage;
        private System.Windows.Forms.Button butInputImage;
        private System.Windows.Forms.TabControl tabShowDiffImage;
        private System.Windows.Forms.FolderBrowserDialog inputImageFolder;
        private System.Windows.Forms.ImageList imgListSplitImage;
        private System.Windows.Forms.ListView listShowSplitImage;
        private System.Windows.Forms.Button butSaveSplitImage;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TabPage tabPage12;
        private System.Windows.Forms.NumericUpDown HeightDividWidthLow;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown HeightDividWidthUp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown HeightUp;
        private System.Windows.Forms.NumericUpDown HeightLow;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown WidthUp;
        private System.Windows.Forms.NumericUpDown WidthLow;
    }
}

