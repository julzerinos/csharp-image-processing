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
                ToolStripMenuItem subsubItem = new ToolStripMenuItem("Add layer");
                subItem.DropDownItems.Add(subsubItem);
                subsubItem.Click += AddMultipleLayer;
                filtersToolStripMenuItem.DropDownItems.Add(subItem);
            }

            FltPictureBox.ContextMenuStrip = contextMenuStrip1;
        }

        Dictionary<string, int> multipleFilterCount = new Dictionary<string, int>();

        private void AddMultipleLayer(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = sender as ToolStripMenuItem;
            ToolStripMenuItem tsParent = ts.OwnerItem as ToolStripMenuItem;

            if (!multipleFilterCount.ContainsKey(tsParent.Text))
                multipleFilterCount.Add(tsParent.Text, 1);
            else
                multipleFilterCount[tsParent.Text] += 1;

            ToolStripMenuItem tsLayer = new ToolStripMenuItem(tsParent.Text);
            tsLayer.CheckedChanged += StateChange;
            tsLayer.CheckOnClick = true;
            tsParent.DropDownItems.Add(tsLayer);
        }

        private void StateChange(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            CacheManager.UpdateFilterState(sender.ToString(), tsmi.Checked);

            Debug.Print(
                "Cache for state [ " + String.Join(" -> ", CacheManager.filterState.ToArray())
                + " ] is " + (CacheManager.GetBitmapForFilterState() == null ? "null" : "not null")
                );

            UpdateCacheIfEmpty();

            FltPictureBox.Image = CacheManager.GetBitmapForFilterState();
        }

        public void UpdateCacheIfEmpty()
        {
            if (CacheManager.GetBitmapForFilterState() == null)
                CacheManager.SetBitmapForFilterState(
                    FilterManager.RecreateFilterStateFromState(
                        CacheManager.GetOriginalImage(), CacheManager.filterState
                        )
                    );
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
            SaveImageFileMenu.Enabled = true;
            customKernelToolStripMenuItem.Enabled = true;

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

        private void customKernelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //KernelEditor kernelEditor = new KernelEditor(this);
            //if (kernelEditor.ShowDialog() == DialogResult.Cancel) return;

            //CacheManager.ResetCache((Bitmap)OrgPictureBox.Image);
            //UpdateCacheIfEmpty();

            //FltPictureBox.Image = CacheManager.GetBitmapForFilterState();
        }

        public void OpenKernelEditor(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = sender as ToolStripMenuItem;
            KernelEditor kernelEditor = new KernelEditor(this, ts.OwnerItem.Text);
            if (kernelEditor.ShowDialog() == DialogResult.Cancel) return;

            CacheManager.ResetCache((Bitmap)OrgPictureBox.Image);
            UpdateCacheIfEmpty();

            FltPictureBox.Image = CacheManager.GetBitmapForFilterState();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(FltPictureBox.Image);
        }

        static public int customKernelCounter = 0;

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = "Custom Kernel " + ++customKernelCounter;
            ToolStripMenuItem ts = new ToolStripMenuItem(name);

            ToolStripMenuItem tsEdit = new ToolStripMenuItem("Edit");

            tsEdit.Click += OpenKernelEditor;

            ts.CheckOnClick = true;
            ts.CheckedChanged += StateChange;

            ts.DropDownItems.Add(tsEdit);

            Kernel.customKernels.Add(name, new CustomKernel(new double[3, 3] { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } }, 1, 0, Point.Empty));

            FilterManager.UpdateFilterMapping(name);

            filtersToolStripMenuItem.DropDownItems.Add(ts);
        }

        private void editFilterConstantsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConstantsEditor constantsEditor = new ConstantsEditor();
            if (constantsEditor.ShowDialog() == DialogResult.Cancel) return;

            CacheManager.ResetCache((Bitmap)OrgPictureBox.Image);
            UpdateCacheIfEmpty();

            FltPictureBox.Image = CacheManager.GetBitmapForFilterState();
        }
    }
}
