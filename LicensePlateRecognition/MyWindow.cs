using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace LicensePlateRecognition
{
    public partial class MyWindow : Form
    {
        public MyWindow()
        {
            InitializeComponent();
        }
        
        //some functions
        private int currentTabCount;
        private void AddTab(string text, Mat image)
        {
            //get page
            TabPage page = null;
            if (viewTabControl.TabPages.Count > currentTabCount)
            {
                page = viewTabControl.TabPages[currentTabCount];
            }
            else
            {
                page = new TabPage();
                viewTabControl.TabPages.Add(page);
            }
            page.Text = text;

            //get picture
            PictureBox pic = null;
            if (page.Controls.Count == 0)
            {
                pic = new PictureBox();
                page.Controls.Add(pic);
                pic.Dock = DockStyle.Fill;
                pic.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else pic = (PictureBox)page.Controls[0];
            pic.Image = image.ToBitmap();

            currentTabCount++;
        }
        private void ProcessAndShowMat(Bitmap image)
        {
            //resize here?


            currentTabCount = 0;


            AddTab("原图", image.ToMat());
            
            //to HSV
            Mat matHSV = new Mat();
            Cv2.CvtColor(image.ToMat(), matHSV, ColorConversionCodes.RGB2HSV);
            AddTab("HSV", matHSV);

            //split
            Cv2.Split(matHSV, out Mat[] matHSVs);
            AddTab("H", matHSVs[0]);
            AddTab("S", matHSVs[1]);
            AddTab("V", matHSVs[2]);

            //eqV
            Mat matVeq = new Mat();
            Cv2.EqualizeHist(matHSVs[2], matVeq);
            AddTab("Veq", matVeq);

            //hsveq
            matHSVs[2] = matVeq;
            Mat matHsveq = new Mat();
            Cv2.Merge(matHSVs, matHsveq);
            AddTab("HSVeq", matHsveq);

            //inrange
            Scalar low = new Scalar(15, 95, 95);
            Scalar upper = new Scalar(40, 255, 255);
            Mat matInrange = new Mat();
            Cv2.InRange(matHsveq, low, upper, matInrange);
            AddTab("InRange", matInrange);

            //operate
            var element = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(7, 3));
            Mat matDilate = new Mat();
            Cv2.Dilate(matInrange, matDilate, element);
            AddTab("Dilate", matDilate);

            Mat matErode = new Mat();
            Cv2.Erode(matDilate, matErode, element);
            AddTab("Erode", matErode);


            //Contour
            Cv2.FindContours(matErode, out OpenCvSharp.Point[][] contours, out HierarchyIndex[] hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
            Mat matContours = image.ToMat();
            Cv2.DrawContours(matContours, contours, -1, new Scalar(255, 0, 255), 2);
            AddTab("Contours", matContours);

            //rectanglize
            Mat matRects = image.ToMat();
            List<Rect> rects = new List<Rect>();
            foreach (OpenCvSharp.Point[] c in contours)
            {
                Rect rect = Cv2.BoundingRect(c);
                rects.Add(rect);
                Cv2.Rectangle(matRects, rect, new Scalar(255, 0, 255), 2);
            }

            AddTab("Rects", matRects);

        }
        private void OpenFile(string[] files)
        {
            foreach (string f in files)
            {
                if (
                    //supported file types
                    f.EndsWith(".jpg") ||
                    f.EndsWith(".png") ||
                    f.EndsWith(".bmp")


                    && !fileList.Items.Contains(f)) fileList.Items.Add(f);
            }
        }

        //window life
        private void MyWindow_Load(object sender, EventArgs e)
        {
            fileList.Items.Clear();
            //openFileDialog1.InitialDirectory =
        }


        //events

        private void openFIleButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                OpenFile(System.IO.Directory.GetFiles(folderBrowserDialog1.SelectedPath));
            }
        }

        private void fileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fileList.SelectedItems.Count != 1) return;
            
            ProcessAndShowMat(new Bitmap(fileList.SelectedItem.ToString()));
        }

        private void deleteFileButton_Click(object sender, EventArgs e)
        {
            var indices = fileList.SelectedIndices;
            if (indices.Count == 0)
            {
                fileList.Items.Clear();
                return;
            }
            for (int i = indices.Count - 1; i >= 0; i--)
            {
                fileList.Items.RemoveAt(indices[i]);
            }
        }

        private void fileList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void fileList_DragDrop(object sender, DragEventArgs e)
        {
            var fs = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (fs.Length == 1 && !fs[0].Contains(".") && System.IO.Directory.Exists(fs[0]))
            {
                OpenFile(System.IO.Directory.GetFiles(fs[0]));
            }
            else
                OpenFile(fs);
        }
    }
}
