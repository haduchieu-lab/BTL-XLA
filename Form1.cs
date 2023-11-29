using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;


namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage;
        public Form1()
        {
            InitializeComponent();
        }


        private void SelectImageButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    originalImage = new Bitmap(openFileDialog.FileName);
                    pictureBox1.Image = originalImage;
                    button2.Enabled = true;
                }
            }
        }



        private void ConvertImageButton1_Click(object sender, EventArgs e)
        {
            if (originalImage != null)
            {
                Bitmap negativeImage = ConvertToNegative(originalImage);
                // Bitmap binaryImage = ConvertToBinary(originalImage);
                // Bitmap contrastImage = AdjustContrast(originalImage,1.5);
                //Bitmap power = ApplyPowerLawTransformation((originalImage, 1);
                pictureBox1.Image = negativeImage;
            }
        }
        private void ConvertImageButton2_Click(object sender, EventArgs e)
        {
            if (originalImage != null)
            {
                // Bitmap negativeImage = ConvertToNegative(originalImage);
                Bitmap binaryImage = ConvertToBinary(originalImage);
                // Bitmap contrastImage = AdjustContrast(originalImage,1.5);
                //Bitmap power = ApplyPowerLawTransformation((originalImage, 1);
                pictureBox1.Image = binaryImage;
            }
        }
        private void ConvertImageButton3_Click(object sender, EventArgs e)
        {
            if (originalImage != null)
            {
                // Bitmap negativeImage = ConvertToNegative(originalImage);
                // Bitmap binaryImage = ConvertToBinary(originalImage);
                Bitmap contrastImage = AdjustContrast(originalImage, 1.5);
                //Bitmap power = ApplyPowerLawTransformation((originalImage, 1);
                pictureBox1.Image = contrastImage;
            }
        }
        private void ConvertImageButton4_Click(object sender, EventArgs e)
        {
            if (originalImage != null)
            {
                //Bitmap negativeImage = ConvertToNegative(originalImage);
                // Bitmap binaryImage = ConvertToBinary(originalImage);
                // Bitmap contrastImage = AdjustContrast(originalImage,1.5);
                Bitmap power = ApplyPowerLawTransformation(originalImage, 1);
                pictureBox1.Image = power;
            }
        }
        private void ConvertImageButton5_Click(object sender, EventArgs e)
        {
            if (originalImage != null)
            {
                //Bitmap negativeImage = ConvertToNegative(originalImage);
                // Bitmap binaryImage = ConvertToBinary(originalImage);
                // Bitmap contrastImage = AdjustContrast(originalImage,1.5);
                //Bitmap power = ApplyPowerLawTransformation(originalImage, 1);
                Bitmap loga = ApplyPowerLawTransformation(originalImage, 2);
                pictureBox1.Image = loga;
            }
        }
        private void ConvertImageButton6_Click(object sender, EventArgs e)
        {
            if (originalImage != null)
            {
                //Bitmap negativeImage = ConvertToNegative(originalImage);
                // Bitmap binaryImage = ConvertToBinary(originalImage);
                // Bitmap contrastImage = AdjustContrast(originalImage,1.5);
                //Bitmap power = ApplyPowerLawTransformation(originalImage, 1);
                Bitmap loga = ApplyLogarithmicTransformation(originalImage, 2);
                pictureBox1.Image = loga;
            }
        }
        private void ConvertImageButton7_Click(object sender, EventArgs e)
        {
            if (originalImage != null)
            {
                //Bitmap negativeImage = ConvertToNegative(originalImage);
                // Bitmap binaryImage = ConvertToBinary(originalImage);
                // Bitmap contrastImage = AdjustContrast(originalImage,1.5);
                //Bitmap power = ApplyPowerLawTransformation(originalImage, 1);
                Bitmap xam = ApplyHistogramEqualization(originalImage);
                pictureBox1.Image = xam;
            }
        }

        private Bitmap ConvertToNegative(Bitmap originalImage)
        {
            // Tạo một bản sao của ảnh gốc
            Bitmap negativeImage = new Bitmap(originalImage.Width, originalImage.Height);

            // Tạo ma trận màu đảo ngược
            float[][] colorMatrixElements = {
                 new float[] {-1, 0, 0, 0, 0},
                  new float[] {0, -1, 0, 0, 0},
                  new float[] {0, 0, -1, 0, 0},
                  new float[] {0, 0, 0, 1, 0},
                  new float[] {1, 1, 1, 0, 1}
            };

            using (Graphics graphics = Graphics.FromImage(negativeImage))
            using (ImageAttributes imageAttributes = new ImageAttributes())
            {
                // Thiết lập ma trận màu đảo ngược
                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
                imageAttributes.SetColorMatrix(colorMatrix);

                // Vẽ ảnh gốc lên ảnh âm bản
                graphics.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                    0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel, imageAttributes);
            }

            return negativeImage;
        }


        public static Bitmap ConvertToBinary(Bitmap image)
        {
            using (Graphics gr = Graphics.FromImage(image)) // SourceImage is a Bitmap object
            {
                var gray_matrix = new float[][] {
        new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
        new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
        new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
        new float[] { 0,   0,   0,   1, 0 },
        new float[] { 0,   0,   0,   0, 1 }
          };

                var ia = new System.Drawing.Imaging.ImageAttributes();
                ia.SetColorMatrix(new System.Drawing.Imaging.ColorMatrix(gray_matrix));
                ia.SetThreshold((float)0.7); // Change this threshold as needed
                var rc = new Rectangle(0, 0, image.Width, image.Height);
                gr.DrawImage(image, rc, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, ia);
                //pictureBox1.Image = image;
            }
            return image;
        }
        static int AdjustPixelValue(int pixelValue, double contrastValue)
        {
            double adjustedValue = ((pixelValue / 255.0 - 0.5) * contrastValue + 0.5) * 255;
            adjustedValue = Math.Max(0, Math.Min(255, adjustedValue));
            return (int)adjustedValue;
        }
        static Bitmap AdjustContrast(Bitmap image, double contrastValue)
        {
            Bitmap adjustedImage = new Bitmap(image.Width, image.Height);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);

                    int red = AdjustPixelValue(pixelColor.R, contrastValue);
                    int green = AdjustPixelValue(pixelColor.G, contrastValue);
                    int blue = AdjustPixelValue(pixelColor.B, contrastValue);

                    Color adjustedColor = Color.FromArgb(red, green, blue);
                    adjustedImage.SetPixel(x, y, adjustedColor);
                }
            }

            return adjustedImage;
        }
        public static Bitmap ApplyPowerLawTransformation(Bitmap image, float gamma)
        {
            int width = image.Width;
            int height = image.Height;

            Bitmap transformedImage = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color originalColor = image.GetPixel(x, y);

                    // Áp dụng chuyển đổi hàm mũ
                    int r = (int)(255 * Math.Pow(originalColor.R / 255.0f, gamma));
                    int g = (int)(255 * Math.Pow(originalColor.G / 255.0f, gamma));
                    int b = (int)(255 * Math.Pow(originalColor.B / 255.0f, gamma));

                    Color transformedColor = Color.FromArgb(r, g, b);
                    transformedImage.SetPixel(x, y, transformedColor);
                }
            }

            return transformedImage;
        }

        public static Bitmap ApplyLogarithmicTransformation(Bitmap image, float c)
        {
            int width = image.Width;
            int height = image.Height;

            Bitmap transformedImage = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color originalColor = image.GetPixel(x, y);

                    // Áp dụng chuyển đổi hàm logarith
                    int r = (int)(c * Math.Log(1 + originalColor.R));
                    int g = (int)(c * Math.Log(1 + originalColor.G));
                    int b = (int)(c * Math.Log(1 + originalColor.B));

                    Color transformedColor = Color.FromArgb(r, g, b);
                    transformedImage.SetPixel(x, y, transformedColor);
                }
            }

            return transformedImage;
        }

        public static Bitmap ApplyHistogramEqualization(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;

            Bitmap transformedImage = new Bitmap(width, height);

            int[] histogramR = new int[256];
            int[] histogramG = new int[256];
            int[] histogramB = new int[256];

            // Tính toán lược đồ màu
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color originalColor = image.GetPixel(x, y);
                    histogramR[originalColor.R]++;
                    histogramG[originalColor.G]++;
                    histogramB[originalColor.B]++;
                }
            }

            int totalPixels = width * height;
            float[] cumulativeHistogramR = new float[256];
            float[] cumulativeHistogramG = new float[256];
            float[] cumulativeHistogramB = new float[256];

            // Tính toán lược đồ tích lũy
            cumulativeHistogramR[0] = histogramR[0] / (float)totalPixels;
            cumulativeHistogramG[0] = histogramG[0] / (float)totalPixels;
            cumulativeHistogramB[0] = histogramB[0] / (float)totalPixels;

            for (int i = 1; i < 256; i++)
            {
                cumulativeHistogramR[i] = cumulativeHistogramR[i - 1] + histogramR[i] / (float)totalPixels;
                cumulativeHistogramG[i] = cumulativeHistogramG[i - 1] + histogramG[i] / (float)totalPixels;
                cumulativeHistogramB[i] = cumulativeHistogramB[i - 1] + histogramB[i] / (float)totalPixels;
            }

            // Áp dụng cân bằng lược đồ xám
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color originalColor = image.GetPixel(x, y);

                    int r = (int)(255 * cumulativeHistogramR[originalColor.R]);
                    int g = (int)(255 * cumulativeHistogramG[originalColor.G]);
                    int b = (int)(255 * cumulativeHistogramB[originalColor.B]);

                    Color transformedColor = Color.FromArgb(r, g, b);
                    transformedImage.SetPixel(x, y, transformedColor);
                }
            }

            return transformedImage;
        }
    }


    /* public static class Program
     {
         [STAThread]
         static void Main()
         {
             Application.EnableVisualStyles();
             Application.SetCompatibleTextRenderingDefault(false);
             Application.Run(new MainForm());
         }
     }*/

}
