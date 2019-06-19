namespace LicensePlateRecognition
{
    partial class MyWindow
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
            this.fileList = new System.Windows.Forms.ListBox();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.deleteFileButton = new System.Windows.Forms.Button();
            this.openFIleButton = new System.Windows.Forms.Button();
            this.viewPanel = new System.Windows.Forms.Panel();
            this.viewTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.leftPanel.SuspendLayout();
            this.viewPanel.SuspendLayout();
            this.viewTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileList
            // 
            this.fileList.AllowDrop = true;
            this.fileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileList.FormattingEnabled = true;
            this.fileList.ItemHeight = 15;
            this.fileList.Items.AddRange(new object[] {
            "test1",
            "test2",
            "test3",
            "test4"});
            this.fileList.Location = new System.Drawing.Point(0, 24);
            this.fileList.Name = "fileList";
            this.fileList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.fileList.Size = new System.Drawing.Size(290, 455);
            this.fileList.TabIndex = 0;
            this.fileList.SelectedIndexChanged += new System.EventHandler(this.fileList_SelectedIndexChanged);
            this.fileList.DragDrop += new System.Windows.Forms.DragEventHandler(this.fileList_DragDrop);
            this.fileList.DragEnter += new System.Windows.Forms.DragEventHandler(this.fileList_DragEnter);
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.fileList);
            this.leftPanel.Controls.Add(this.deleteFileButton);
            this.leftPanel.Controls.Add(this.openFIleButton);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 0);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(290, 502);
            this.leftPanel.TabIndex = 1;
            // 
            // deleteFileButton
            // 
            this.deleteFileButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.deleteFileButton.Location = new System.Drawing.Point(0, 479);
            this.deleteFileButton.Name = "deleteFileButton";
            this.deleteFileButton.Size = new System.Drawing.Size(290, 23);
            this.deleteFileButton.TabIndex = 2;
            this.deleteFileButton.Text = "清除";
            this.deleteFileButton.UseVisualStyleBackColor = true;
            this.deleteFileButton.Click += new System.EventHandler(this.deleteFileButton_Click);
            // 
            // openFIleButton
            // 
            this.openFIleButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.openFIleButton.Location = new System.Drawing.Point(0, 0);
            this.openFIleButton.Name = "openFIleButton";
            this.openFIleButton.Size = new System.Drawing.Size(290, 24);
            this.openFIleButton.TabIndex = 1;
            this.openFIleButton.Text = "打开";
            this.openFIleButton.UseVisualStyleBackColor = true;
            this.openFIleButton.Click += new System.EventHandler(this.openFIleButton_Click);
            // 
            // viewPanel
            // 
            this.viewPanel.Controls.Add(this.viewTabControl);
            this.viewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewPanel.Location = new System.Drawing.Point(290, 0);
            this.viewPanel.Name = "viewPanel";
            this.viewPanel.Size = new System.Drawing.Size(692, 502);
            this.viewPanel.TabIndex = 2;
            // 
            // viewTabControl
            // 
            this.viewTabControl.Controls.Add(this.tabPage1);
            this.viewTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewTabControl.Location = new System.Drawing.Point(0, 0);
            this.viewTabControl.Name = "viewTabControl";
            this.viewTabControl.SelectedIndex = 0;
            this.viewTabControl.Size = new System.Drawing.Size(692, 502);
            this.viewTabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(684, 473);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "测试";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // MyWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 502);
            this.Controls.Add(this.viewPanel);
            this.Controls.Add(this.leftPanel);
            this.Name = "MyWindow";
            this.Text = "MyWindow";
            this.Load += new System.EventHandler(this.MyWindow_Load);
            this.leftPanel.ResumeLayout(false);
            this.viewPanel.ResumeLayout(false);
            this.viewTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox fileList;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Button openFIleButton;
        private System.Windows.Forms.Panel viewPanel;
        private System.Windows.Forms.Button deleteFileButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TabControl viewTabControl;
        private System.Windows.Forms.TabPage tabPage1;
    }
}