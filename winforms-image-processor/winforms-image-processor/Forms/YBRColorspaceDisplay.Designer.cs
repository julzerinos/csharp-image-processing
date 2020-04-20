namespace winforms_image_processor
{
    partial class YBRColorspaceDisplay
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.YImage = new System.Windows.Forms.PictureBox();
            this.CbImage = new System.Windows.Forms.PictureBox();
            this.CrImage = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.YImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CbImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CrImage)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.YImage, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.CbImage, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.CrImage, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(284, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // YImage
            // 
            this.YImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.YImage.Location = new System.Drawing.Point(3, 3);
            this.YImage.Name = "YImage";
            this.YImage.Size = new System.Drawing.Size(278, 143);
            this.YImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.YImage.TabIndex = 0;
            this.YImage.TabStop = false;
            // 
            // CbImage
            // 
            this.CbImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CbImage.Location = new System.Drawing.Point(3, 152);
            this.CbImage.Name = "CbImage";
            this.CbImage.Size = new System.Drawing.Size(278, 143);
            this.CbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CbImage.TabIndex = 1;
            this.CbImage.TabStop = false;
            // 
            // CrImage
            // 
            this.CrImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CrImage.Location = new System.Drawing.Point(3, 301);
            this.CrImage.Name = "CrImage";
            this.CrImage.Size = new System.Drawing.Size(278, 146);
            this.CrImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CrImage.TabIndex = 2;
            this.CrImage.TabStop = false;
            // 
            // YBRColorspaceDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "YBRColorspaceDisplay";
            this.Text = "YBRColorspaceDisplay";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.YBRColorspaceDisplay_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.YImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CbImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CrImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox YImage;
        private System.Windows.Forms.PictureBox CbImage;
        private System.Windows.Forms.PictureBox CrImage;
    }
}