using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winforms_image_processor
{
    public partial class ImageProcessorForm : Form
    {
        public ImageProcessorForm()
        {
            InitializeComponent();

            foreach (var filter in FilterManager.filterMapping)
            {
                ToolStripMenuItem subItem = new ToolStripMenuItem(filter.Key);
                subItem.CheckedChanged += StateChange;
                subItem.CheckOnClick = true;
                filtersToolStripMenuItem.DropDownItems.Add(subItem);
            }
        }

        //public void UpdateToolStripCheckedChanged(ToolStripMenuItem tsmi)
        ////Recursion for toolstrip: https://stackoverflow.com/questions/33766276/cannot-get-all-the-submenu-items-in-winform-in-c-sharp
        //{
        //    if (tsmi.DropDownItems.Count == 0)
        //    {
        //        tsmi.CheckedChanged += StateChange;
        //        return;
        //    }

        //    foreach (ToolStripMenuItem innerTsmi in tsmi.DropDownItems)
        //    {
        //        UpdateToolStripCheckedChanged(innerTsmi);
        //    }
        //}

        private void StateChange(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            CacheManager.UpdateFilterState(sender.ToString(), tsmi.Checked);

            Debug.Print(
                "Cache for state [ " + String.Join(" -> ", CacheManager.filterState.ToArray())
                + " ] is " + (CacheManager.GetBitmapForFilterState() == null ? "null" : "not null")
                );

            if (CacheManager.GetBitmapForFilterState() == null)
                CacheManager.SetBitmapForFilterState(
                    FilterManager.RecreateFilterStateFromState(
                        CacheManager.GetOriginalImage(), CacheManager.filterState
                        )
                    );

            FltPictureBox.Image = CacheManager.GetBitmapForFilterState();
        }

        private void OpenImageFileMenu_Click(object sender, EventArgs e)
        // File Dialog:         https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.openfiledialog?view=netframework-4.8
        // Bitmap deep copy:    https://stackoverflow.com/questions/16316451/how-can-i-open-a-bitmap-file-change-it-and-then-save-it 
        // PictureBox Bitmap:   https://stackoverflow.com/questions/743549/how-to-put-image-in-a-picture-box-from-bitmap
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\Downloads";
                openFileDialog.Filter = "JPEG Image|*.jpg|PNG Image|*.png";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    filePath = openFileDialog.FileName;
                else
                    return;
            }

            OrgPictureBox.Image = new Bitmap(filePath);
            FltPictureBox.Image = new Bitmap(filePath);

            filtersToolStripMenuItem.Enabled = true;

            CacheManager.InitializeWithOriginal((Bitmap)FltPictureBox.Image);
        }

        private void SaveImageFileMenu_Click(object sender, EventArgs e)
        // SaveFileControl:     https://docs.microsoft.com/en-us/dotnet/framework/winforms/controls/how-to-save-files-using-the-savefiledialog-component
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPEG Image|*.jpg|PNG Image|*.png";
            saveFileDialog1.Title = "Save the filtered image";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                Bitmap filteredImage = (Bitmap)FltPictureBox.Image;
                filteredImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }


    }
}
