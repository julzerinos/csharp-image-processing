using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winforms_image_processor
{
    public partial class YBRColorspaceDisplay : Form
    {
        private ImageProcessorForm imageProcessorForm;
        private Bitmap currentFilterImage;

        public YBRColorspaceDisplay(Form callerForm, Bitmap currentFltImage)
        {
            currentFilterImage = CacheManager.DeepCopy(currentFltImage);
            imageProcessorForm = callerForm as ImageProcessorForm;

            InitializeComponent();

            Height = callerForm.Height;

            setUpImages();
        }

        public void setUpImages()
        {
            setUpYImage();
            setUpCbImage();
            setUpCrImage();
        }

        public void updateImages(Bitmap currFltImage)
        {
            currentFilterImage = currFltImage;

            setUpImages();
        }

        void setUpYImage()
        {
            Bitmap YImageBmp = CacheManager.DeepCopy<Bitmap>(currentFilterImage);

            YImageBmp = YImageBmp.ApplyFilter(FilterManager.YGrayscale);

            YImage.Image = YImageBmp;
        }

        void setUpCbImage()
        {
            Bitmap CbImageBmp = CacheManager.DeepCopy<Bitmap>(currentFilterImage);

            CbImageBmp = CbImageBmp.ApplyFilter(FilterManager.CbInterpolate);

            CbImage.Image = CbImageBmp;
        }

        void setUpCrImage()
        {
            Bitmap CrImageBmp = CacheManager.DeepCopy<Bitmap>(currentFilterImage);

            CrImageBmp = CrImageBmp.ApplyFilter(FilterManager.CrInterpolate);

            CrImage.Image = CrImageBmp;
        }

        private void YBRColorspaceDisplay_FormClosed(object sender, FormClosedEventArgs e)
        {
            imageProcessorForm.yBRColorspaceDisplay = null;
        }
    }
}
