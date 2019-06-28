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
            this.tabShowDiffImage = new System.Windows.Forms.TabControl();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.inputImageFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.imgListSplitImage = new System.Windows.Forms.ImageList(this.components);
            this.listShowSplitImage = new System.Windows.Forms.ListView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button7 = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBoxForPlateParameter = new System.Windows.Forms.GroupBox();
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
            this.HeightDividWidthLow = new System.Windows.Forms.NumericUpDown();
            this.HeightDividWidthUp = new System.Windows.Forms.NumericUpDown();
            this.groupBoxForPlateLabel = new System.Windows.Forms.GroupBox();
            this.butSaveAllPlate = new System.Windows.Forms.Button();
            this.butDrop = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.butSavePlateCategory = new System.Windows.Forms.Button();
            this.comboBoxPlate = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBoxForChar = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.butSaveAllChar = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.butSaveChar = new System.Windows.Forms.Button();
            this.comboBoxChar = new System.Windows.Forms.ComboBox();
            this.车牌定位ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.butOpenImage = new System.Windows.Forms.ToolStripMenuItem();
            this.批量处理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.字符分割ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.butOpenPlate = new System.Windows.Forms.ToolStripMenuItem();
            this.批量处理ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.车牌训练ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.字符训练ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tabShowDiffImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBoxForPlateParameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeightUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WidthUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WidthLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightDividWidthLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightDividWidthUp)).BeginInit();
            this.groupBoxForPlateLabel.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBoxForChar.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listInputImage
            // 
            this.listInputImage.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.listInputImage.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listInputImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listInputImage.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listInputImage.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.listInputImage.Location = new System.Drawing.Point(0, 0);
            this.listInputImage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listInputImage.Name = "listInputImage";
            this.listInputImage.Size = new System.Drawing.Size(247, 658);
            this.listInputImage.TabIndex = 0;
            this.listInputImage.UseCompatibleStateImageBehavior = false;
            this.listInputImage.View = System.Windows.Forms.View.List;
            this.listInputImage.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 640;
            // 
            // tabShowDiffImage
            // 
            this.tabShowDiffImage.Controls.Add(this.tabPage12);
            this.tabShowDiffImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabShowDiffImage.Location = new System.Drawing.Point(0, 0);
            this.tabShowDiffImage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabShowDiffImage.Name = "tabShowDiffImage";
            this.tabShowDiffImage.SelectedIndex = 0;
            this.tabShowDiffImage.Size = new System.Drawing.Size(839, 173);
            this.tabShowDiffImage.TabIndex = 2;
            // 
            // tabPage12
            // 
            this.tabPage12.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tabPage12.Location = new System.Drawing.Point(4, 25);
            this.tabPage12.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Size = new System.Drawing.Size(831, 144);
            this.tabPage12.TabIndex = 0;
            this.tabPage12.Text = "原图";
            // 
            // imgListSplitImage
            // 
            this.imgListSplitImage.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imgListSplitImage.ImageSize = new System.Drawing.Size(16, 32);
            this.imgListSplitImage.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // listShowSplitImage
            // 
            this.listShowSplitImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.listShowSplitImage.LargeImageList = this.imgListSplitImage;
            this.listShowSplitImage.Location = new System.Drawing.Point(0, 0);
            this.listShowSplitImage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listShowSplitImage.Name = "listShowSplitImage";
            this.listShowSplitImage.Size = new System.Drawing.Size(839, 166);
            this.listShowSplitImage.TabIndex = 4;
            this.listShowSplitImage.UseCompatibleStateImageBehavior = false;
            this.listShowSplitImage.SelectedIndexChanged += new System.EventHandler(this.listShowSplitImage_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button7);
            this.splitContainer1.Panel1.Controls.Add(this.listInputImage);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1107, 662);
            this.splitContainer1.SplitterDistance = 251;
            this.splitContainer1.TabIndex = 7;
            // 
            // button7
            // 
            this.button7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button7.Location = new System.Drawing.Point(0, 635);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(247, 23);
            this.button7.TabIndex = 1;
            this.button7.Text = "button7";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Location = new System.Drawing.Point(3, 2);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabShowDiffImage);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer2.Panel2.Controls.Add(this.listShowSplitImage);
            this.splitContainer2.Size = new System.Drawing.Size(843, 652);
            this.splitContainer2.SplitterDistance = 177;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 166);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(839, 463);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBoxForPlateParameter);
            this.tabPage1.Controls.Add(this.groupBoxForPlateLabel);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(831, 434);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "车牌处理";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBoxForPlateParameter
            // 
            this.groupBoxForPlateParameter.Controls.Add(this.label6);
            this.groupBoxForPlateParameter.Controls.Add(this.label7);
            this.groupBoxForPlateParameter.Controls.Add(this.HeightUp);
            this.groupBoxForPlateParameter.Controls.Add(this.HeightLow);
            this.groupBoxForPlateParameter.Controls.Add(this.label5);
            this.groupBoxForPlateParameter.Controls.Add(this.label4);
            this.groupBoxForPlateParameter.Controls.Add(this.WidthUp);
            this.groupBoxForPlateParameter.Controls.Add(this.WidthLow);
            this.groupBoxForPlateParameter.Controls.Add(this.label3);
            this.groupBoxForPlateParameter.Controls.Add(this.label2);
            this.groupBoxForPlateParameter.Controls.Add(this.HeightDividWidthLow);
            this.groupBoxForPlateParameter.Controls.Add(this.HeightDividWidthUp);
            this.groupBoxForPlateParameter.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxForPlateParameter.Location = new System.Drawing.Point(3, 128);
            this.groupBoxForPlateParameter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxForPlateParameter.Name = "groupBoxForPlateParameter";
            this.groupBoxForPlateParameter.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxForPlateParameter.Size = new System.Drawing.Size(825, 180);
            this.groupBoxForPlateParameter.TabIndex = 7;
            this.groupBoxForPlateParameter.TabStop = false;
            this.groupBoxForPlateParameter.Text = "外接矩形参数";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(396, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 17;
            this.label6.Text = "高最高值";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(87, 144);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 15);
            this.label7.TabIndex = 16;
            this.label7.Text = "高最低值";
            // 
            // HeightUp
            // 
            this.HeightUp.Location = new System.Drawing.Point(513, 134);
            this.HeightUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.HeightLow.Location = new System.Drawing.Point(205, 134);
            this.HeightLow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.label5.Location = new System.Drawing.Point(396, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 13;
            this.label5.Text = "宽最高值";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(87, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 12;
            this.label4.Text = "宽最低值";
            // 
            // WidthUp
            // 
            this.WidthUp.Location = new System.Drawing.Point(513, 88);
            this.WidthUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.WidthLow.Location = new System.Drawing.Point(205, 88);
            this.WidthLow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.label3.Location = new System.Drawing.Point(396, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "高宽比最高值";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "高宽比最低值";
            // 
            // HeightDividWidthLow
            // 
            this.HeightDividWidthLow.DecimalPlaces = 2;
            this.HeightDividWidthLow.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.HeightDividWidthLow.Location = new System.Drawing.Point(205, 41);
            this.HeightDividWidthLow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.HeightDividWidthUp.Location = new System.Drawing.Point(513, 40);
            this.HeightDividWidthUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            // groupBoxForPlateLabel
            // 
            this.groupBoxForPlateLabel.Controls.Add(this.butSaveAllPlate);
            this.groupBoxForPlateLabel.Controls.Add(this.butDrop);
            this.groupBoxForPlateLabel.Controls.Add(this.label1);
            this.groupBoxForPlateLabel.Controls.Add(this.butSavePlateCategory);
            this.groupBoxForPlateLabel.Controls.Add(this.comboBoxPlate);
            this.groupBoxForPlateLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxForPlateLabel.Location = new System.Drawing.Point(3, 3);
            this.groupBoxForPlateLabel.Name = "groupBoxForPlateLabel";
            this.groupBoxForPlateLabel.Size = new System.Drawing.Size(825, 125);
            this.groupBoxForPlateLabel.TabIndex = 8;
            this.groupBoxForPlateLabel.TabStop = false;
            this.groupBoxForPlateLabel.Text = "打标签";
            // 
            // butSaveAllPlate
            // 
            this.butSaveAllPlate.Location = new System.Drawing.Point(338, 81);
            this.butSaveAllPlate.Name = "butSaveAllPlate";
            this.butSaveAllPlate.Size = new System.Drawing.Size(75, 23);
            this.butSaveAllPlate.TabIndex = 28;
            this.butSaveAllPlate.Text = "保存全部";
            this.butSaveAllPlate.UseVisualStyleBackColor = true;
            this.butSaveAllPlate.Click += new System.EventHandler(this.butAutoSavePlateCategory);
            // 
            // butDrop
            // 
            this.butDrop.Location = new System.Drawing.Point(482, 82);
            this.butDrop.Name = "butDrop";
            this.butDrop.Size = new System.Drawing.Size(75, 23);
            this.butDrop.TabIndex = 25;
            this.butDrop.Text = "丢弃";
            this.butDrop.UseVisualStyleBackColor = true;
            this.butDrop.Click += new System.EventHandler(this.butDrop_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(90, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 21;
            this.label1.Text = "添加标签";
            // 
            // butSavePlateCategory
            // 
            this.butSavePlateCategory.Location = new System.Drawing.Point(205, 82);
            this.butSavePlateCategory.Name = "butSavePlateCategory";
            this.butSavePlateCategory.Size = new System.Drawing.Size(83, 23);
            this.butSavePlateCategory.TabIndex = 20;
            this.butSavePlateCategory.Text = "保存";
            this.butSavePlateCategory.UseVisualStyleBackColor = true;
            this.butSavePlateCategory.Click += new System.EventHandler(this.butSavePlateCategory_Click);
            // 
            // comboBoxPlate
            // 
            this.comboBoxPlate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlate.FormattingEnabled = true;
            this.comboBoxPlate.Location = new System.Drawing.Point(204, 36);
            this.comboBoxPlate.Name = "comboBoxPlate";
            this.comboBoxPlate.Size = new System.Drawing.Size(153, 23);
            this.comboBoxPlate.TabIndex = 19;
            this.comboBoxPlate.Tag = "";
            this.comboBoxPlate.SelectedIndexChanged += new System.EventHandler(this.comboBoxPlate_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBoxForChar);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(831, 434);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "字符处理";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBoxForChar
            // 
            this.groupBoxForChar.Controls.Add(this.button1);
            this.groupBoxForChar.Controls.Add(this.butSaveAllChar);
            this.groupBoxForChar.Controls.Add(this.label10);
            this.groupBoxForChar.Controls.Add(this.butSaveChar);
            this.groupBoxForChar.Controls.Add(this.comboBoxChar);
            this.groupBoxForChar.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxForChar.Location = new System.Drawing.Point(3, 3);
            this.groupBoxForChar.Name = "groupBoxForChar";
            this.groupBoxForChar.Size = new System.Drawing.Size(825, 135);
            this.groupBoxForChar.TabIndex = 10;
            this.groupBoxForChar.TabStop = false;
            this.groupBoxForChar.Text = "打标签";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(493, 89);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 26;
            this.button1.Text = "丢弃";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.butDrop_Click);
            // 
            // butSaveAllChar
            // 
            this.butSaveAllChar.Location = new System.Drawing.Point(350, 89);
            this.butSaveAllChar.Name = "butSaveAllChar";
            this.butSaveAllChar.Size = new System.Drawing.Size(75, 23);
            this.butSaveAllChar.TabIndex = 25;
            this.butSaveAllChar.Text = "保存全部";
            this.butSaveAllChar.UseVisualStyleBackColor = true;
            this.butSaveAllChar.Click += new System.EventHandler(this.butAutoSaveCharCategory);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(90, 43);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 15);
            this.label10.TabIndex = 21;
            this.label10.Text = "添加标签";
            // 
            // butSaveChar
            // 
            this.butSaveChar.Location = new System.Drawing.Point(204, 89);
            this.butSaveChar.Name = "butSaveChar";
            this.butSaveChar.Size = new System.Drawing.Size(75, 23);
            this.butSaveChar.TabIndex = 20;
            this.butSaveChar.Text = "保存";
            this.butSaveChar.UseVisualStyleBackColor = true;
            this.butSaveChar.Click += new System.EventHandler(this.butSaveCharCategory_Click);
            // 
            // comboBoxChar
            // 
            this.comboBoxChar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxChar.FormattingEnabled = true;
            this.comboBoxChar.Location = new System.Drawing.Point(204, 36);
            this.comboBoxChar.Name = "comboBoxChar";
            this.comboBoxChar.Size = new System.Drawing.Size(150, 23);
            this.comboBoxChar.TabIndex = 19;
            this.comboBoxChar.Tag = "";
            this.comboBoxChar.SelectedIndexChanged += new System.EventHandler(this.comboBoxChar_SelectedIndexChanged);
            // 
            // 车牌定位ToolStripMenuItem
            // 
            this.车牌定位ToolStripMenuItem.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.车牌定位ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.butOpenImage,
            this.批量处理ToolStripMenuItem});
            this.车牌定位ToolStripMenuItem.Name = "车牌定位ToolStripMenuItem";
            this.车牌定位ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.车牌定位ToolStripMenuItem.Text = "车牌定位";
            // 
            // butOpenImage
            // 
            this.butOpenImage.Name = "butOpenImage";
            this.butOpenImage.Size = new System.Drawing.Size(189, 26);
            this.butOpenImage.Text = "打开图片文件夹";
            this.butOpenImage.Click += new System.EventHandler(this.OpenImageFolder);
            // 
            // 批量处理ToolStripMenuItem
            // 
            this.批量处理ToolStripMenuItem.Name = "批量处理ToolStripMenuItem";
            this.批量处理ToolStripMenuItem.Size = new System.Drawing.Size(189, 26);
            this.批量处理ToolStripMenuItem.Text = "批量处理";
            this.批量处理ToolStripMenuItem.Click += new System.EventHandler(this.AutoProcessImage);
            // 
            // 字符分割ToolStripMenuItem
            // 
            this.字符分割ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.butOpenPlate,
            this.批量处理ToolStripMenuItem1});
            this.字符分割ToolStripMenuItem.Name = "字符分割ToolStripMenuItem";
            this.字符分割ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.字符分割ToolStripMenuItem.Text = "字符分割";
            // 
            // butOpenPlate
            // 
            this.butOpenPlate.Name = "butOpenPlate";
            this.butOpenPlate.Size = new System.Drawing.Size(216, 26);
            this.butOpenPlate.Text = "打开图片文件夹";
            this.butOpenPlate.Click += new System.EventHandler(this.butOpenCharFolder);
            // 
            // 批量处理ToolStripMenuItem1
            // 
            this.批量处理ToolStripMenuItem1.Name = "批量处理ToolStripMenuItem1";
            this.批量处理ToolStripMenuItem1.Size = new System.Drawing.Size(216, 26);
            this.批量处理ToolStripMenuItem1.Text = "批量处理";
            this.批量处理ToolStripMenuItem1.Click += new System.EventHandler(this.AutoProcessChar);
            // 
            // 车牌训练ToolStripMenuItem
            // 
            this.车牌训练ToolStripMenuItem.Name = "车牌训练ToolStripMenuItem";
            this.车牌训练ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.车牌训练ToolStripMenuItem.Text = "车牌训练";
            this.车牌训练ToolStripMenuItem.Click += new System.EventHandler(this.TrainPlate);
            // 
            // 字符训练ToolStripMenuItem
            // 
            this.字符训练ToolStripMenuItem.Name = "字符训练ToolStripMenuItem";
            this.字符训练ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.字符训练ToolStripMenuItem.Text = "字符训练";
            this.字符训练ToolStripMenuItem.Click += new System.EventHandler(this.TrainChar);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.帮助ToolStripMenuItem.Text = "设置";
            this.帮助ToolStripMenuItem.Click += new System.EventHandler(this.OpenUserSetting);
            // 
            // 帮助ToolStripMenuItem1
            // 
            this.帮助ToolStripMenuItem1.Name = "帮助ToolStripMenuItem1";
            this.帮助ToolStripMenuItem1.Size = new System.Drawing.Size(51, 24);
            this.帮助ToolStripMenuItem1.Text = "帮助";
            this.帮助ToolStripMenuItem1.Click += new System.EventHandler(this.OpenHelpForm);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.车牌定位ToolStripMenuItem,
            this.字符分割ToolStripMenuItem,
            this.车牌训练ToolStripMenuItem,
            this.字符训练ToolStripMenuItem,
            this.帮助ToolStripMenuItem,
            this.帮助ToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1107, 28);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1107, 690);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "LicensePlateRecognition";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tabShowDiffImage.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBoxForPlateParameter.ResumeLayout(false);
            this.groupBoxForPlateParameter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeightUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WidthUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WidthLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightDividWidthLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightDividWidthUp)).EndInit();
            this.groupBoxForPlateLabel.ResumeLayout(false);
            this.groupBoxForPlateLabel.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBoxForChar.ResumeLayout(false);
            this.groupBoxForChar.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listInputImage;
        private System.Windows.Forms.TabControl tabShowDiffImage;
        private System.Windows.Forms.FolderBrowserDialog inputImageFolder;
        private System.Windows.Forms.ImageList imgListSplitImage;
        private System.Windows.Forms.ListView listShowSplitImage;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TabPage tabPage12;
        private System.Windows.Forms.NumericUpDown HeightDividWidthLow;
        private System.Windows.Forms.GroupBox groupBoxForPlateParameter;
        private System.Windows.Forms.NumericUpDown HeightDividWidthUp;
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
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBoxForPlateLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button butSavePlateCategory;
        private System.Windows.Forms.ComboBox comboBoxPlate;
        private System.Windows.Forms.Button butDrop;
        private System.Windows.Forms.GroupBox groupBoxForChar;
        private System.Windows.Forms.Button butSaveAllChar;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button butSaveChar;
        private System.Windows.Forms.ComboBox comboBoxChar;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button butSaveAllPlate;
        private System.Windows.Forms.ToolStripMenuItem 车牌定位ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem butOpenImage;
        private System.Windows.Forms.ToolStripMenuItem 批量处理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 字符分割ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem butOpenPlate;
        private System.Windows.Forms.ToolStripMenuItem 批量处理ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 车牌训练ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 字符训练ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button button1;
    }
}

