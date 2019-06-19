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
            this.tabShowDiffImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
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
            this.tabShowDiffImage.Size = new System.Drawing.Size(701, 372);
            this.tabShowDiffImage.TabIndex = 2;
            // 
            // tabPage12
            // 
            this.tabPage12.Location = new System.Drawing.Point(4, 25);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Size = new System.Drawing.Size(693, 343);
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
            this.splitContainer2.Panel2.Controls.Add(this.listShowSplitImage);
            this.splitContainer2.Size = new System.Drawing.Size(716, 575);
            this.splitContainer2.SplitterDistance = 380;
            this.splitContainer2.TabIndex = 0;
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
    }
}

