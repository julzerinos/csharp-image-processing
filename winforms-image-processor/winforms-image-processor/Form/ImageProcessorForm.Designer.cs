namespace winforms_image_processor
{
    partial class ImageProcessorForm
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
            this.components = new System.ComponentModel.Container();
            this.TableControl = new System.Windows.Forms.TableLayoutPanel();
            this.OrgPictureBox = new System.Windows.Forms.PictureBox();
            this.FltPictureBox = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenImageFileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveImageFileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.filtersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customKernelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editFilterConstantsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TableControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OrgPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FltPictureBox)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TableControl
            // 
            this.TableControl.ColumnCount = 1;
            this.TableControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TableControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.TableControl.Controls.Add(this.OrgPictureBox, 1, 0);
            this.TableControl.Controls.Add(this.FltPictureBox, 1, 1);
            this.TableControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableControl.Location = new System.Drawing.Point(0, 24);
            this.TableControl.Name = "TableControl";
            this.TableControl.RowCount = 2;
            this.TableControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableControl.Size = new System.Drawing.Size(678, 437);
            this.TableControl.TabIndex = 0;
            // 
            // OrgPictureBox
            // 
            this.OrgPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OrgPictureBox.Location = new System.Drawing.Point(3, 3);
            this.OrgPictureBox.Name = "OrgPictureBox";
            this.OrgPictureBox.Size = new System.Drawing.Size(672, 212);
            this.OrgPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.OrgPictureBox.TabIndex = 0;
            this.OrgPictureBox.TabStop = false;
            // 
            // FltPictureBox
            // 
            this.FltPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FltPictureBox.Location = new System.Drawing.Point(3, 221);
            this.FltPictureBox.Name = "FltPictureBox";
            this.FltPictureBox.Size = new System.Drawing.Size(672, 213);
            this.FltPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.FltPictureBox.TabIndex = 1;
            this.FltPictureBox.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.filtersToolStripMenuItem,
            this.customKernelToolStripMenuItem,
            this.editFilterConstantsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(678, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenImageFileMenu,
            this.SaveImageFileMenu});
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(37, 20);
            this.FileMenu.Text = "File";
            // 
            // OpenImageFileMenu
            // 
            this.OpenImageFileMenu.Name = "OpenImageFileMenu";
            this.OpenImageFileMenu.Size = new System.Drawing.Size(176, 22);
            this.OpenImageFileMenu.Text = "Open Image";
            this.OpenImageFileMenu.Click += new System.EventHandler(this.OpenImageFileMenu_Click);
            // 
            // SaveImageFileMenu
            // 
            this.SaveImageFileMenu.Enabled = false;
            this.SaveImageFileMenu.Name = "SaveImageFileMenu";
            this.SaveImageFileMenu.Size = new System.Drawing.Size(176, 22);
            this.SaveImageFileMenu.Text = "Save Filtered Image";
            this.SaveImageFileMenu.Click += new System.EventHandler(this.SaveImageFileMenu_Click);
            // 
            // filtersToolStripMenuItem
            // 
            this.filtersToolStripMenuItem.Enabled = false;
            this.filtersToolStripMenuItem.Name = "filtersToolStripMenuItem";
            this.filtersToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.filtersToolStripMenuItem.Text = "Filters";
            // 
            // customKernelToolStripMenuItem
            // 
            this.customKernelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem});
            this.customKernelToolStripMenuItem.Enabled = false;
            this.customKernelToolStripMenuItem.Name = "customKernelToolStripMenuItem";
            this.customKernelToolStripMenuItem.Size = new System.Drawing.Size(97, 20);
            this.customKernelToolStripMenuItem.Text = "Custom Kernel";
            this.customKernelToolStripMenuItem.Click += new System.EventHandler(this.customKernelToolStripMenuItem_Click);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(103, 26);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // editFilterConstantsToolStripMenuItem
            // 
            this.editFilterConstantsToolStripMenuItem.Name = "editFilterConstantsToolStripMenuItem";
            this.editFilterConstantsToolStripMenuItem.Size = new System.Drawing.Size(124, 20);
            this.editFilterConstantsToolStripMenuItem.Text = "Edit Filter Constants";
            this.editFilterConstantsToolStripMenuItem.Click += new System.EventHandler(this.editFilterConstantsToolStripMenuItem_Click);
            // 
            // ImageProcessorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 461);
            this.Controls.Add(this.TableControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(1000, 1000);
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "ImageProcessorForm";
            this.Text = "Image Processor";
            this.TableControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OrgPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FltPictureBox)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TableControl;
        private System.Windows.Forms.PictureBox OrgPictureBox;
        private System.Windows.Forms.PictureBox FltPictureBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem OpenImageFileMenu;
        private System.Windows.Forms.ToolStripMenuItem SaveImageFileMenu;
        private System.Windows.Forms.ToolStripMenuItem filtersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customKernelToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editFilterConstantsToolStripMenuItem;
    }
}

