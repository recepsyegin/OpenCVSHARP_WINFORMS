using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.XImgProc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace openSHARK
{
    public partial class Form1 : Form
    {

        // KAMERA AÇMA İÇİN DEĞİŞKENLER
        private VideoCapture capture;
        private bool isCameraRunning = false;



        public Form1()
        {
            InitializeComponent();
            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (isCameraRunning)
            {
                // Kamera zaten çalışıyorsa durdur
                isCameraRunning = false;
                capture.Release(); // Kamerayı serbest bırak
                button1.Text = "Kamerayı Aç";
            }
            else
            {
                // Kamera çalışmıyorsa başlat
                capture = new VideoCapture(0); // 0, varsayılan kamera
                isCameraRunning = true;
                button1.Text = "Kamerayı Kapat";
                

                // Kameradan gelen görüntüyü sürekli güncellemek için bir döngü başlat
                await Task.Run(() =>
                {
                    while (isCameraRunning && capture.IsOpened())
                    {
                        using (var frame = new Mat())
                        {
                            capture.Read(frame); // Kameradan bir kare al
                            if (!frame.Empty())
                            {
                                // PictureBox'ta görüntülemek için formu güncelle
                                pictureBox1.Image = BitmapConverter.ToBitmap(frame);
                            }
                        }
                    }
                });
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if(capture != null && capture.IsOpened())
            {
                // Kenar algılama için yeni bir döngü başlat
                Task.Run(() =>
                {
                    while (isCameraRunning && capture.IsOpened())
                    {
                        using (var frame = new Mat())
                        using (var edges = new Mat())
                        {
                            capture.Read(frame); // Kameradan bir kare al

                            if(!frame.Empty())
                            {

                                // Kenar algılama işlemini uygula
                                Cv2.Canny(frame, edges, 100, 200);

                                // Orijinal Kareyi göster (pictureBox1)
                                pictureBox1.Image = BitmapConverter.ToBitmap(frame);

                                // Kenar algılanmış kareyi göster (pictureBox2)
                                pictureBox2.Image = BitmapConverter.ToBitmap(edges);


                                

                            }
                        }
                    }
                });
            }
        }
        

        // Homogeneus Blur
        private void button3_Click(object sender, EventArgs e)
        {
            
            if (capture != null && capture.IsOpened())
            {
                isCameraRunning = true;

                Task.Run(() =>
                {
                    while (isCameraRunning && capture.IsOpened())
                    {
                        using (var frame = new Mat())
                        using (var smoothed = new Mat())
                        {
                            capture.Read(frame);

                            if (!frame.Empty())
                            {
                                OpenCvSharp.Size size = new OpenCvSharp.Size(15, 15);
                                Cv2.Blur(frame, smoothed, size);

                                // Form ana iş parçacığına göndermek için Invoke kullanılır
                                Invoke(new Action(() =>
                                {
                                    pictureBox3.Image?.Dispose();
                                    pictureBox3.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(smoothed);
                                }));
                            }
                        }
                    }
                });
            }
        }




        // Canny Edge Detection Picture Box
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }


        // Smooth Images
        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
       

        // Gaussian Blur 
        private void button4_Click(object sender, EventArgs e)
        {
           
            
            if(capture != null && capture.IsOpened())
            {
                isCameraRunning = true;
                Task.Run(() =>
                {
                while (isCameraRunning && capture.IsOpened())
                {
                    using (var frame = new Mat())
                    using (var gaussianBlurFrame = new Mat())
                    {
                        capture.Read(frame);
                        if (!frame.Empty())
                        {
                            OpenCvSharp.Size kernelSize = new OpenCvSharp.Size(15, 15);
                            Cv2.GaussianBlur(frame, gaussianBlurFrame, kernelSize, 0, 0);

                            Invoke(new Action(() =>
                                {
                                pictureBox4.Image?.Dispose();
                                pictureBox4.Image =
                                OpenCvSharp.Extensions.BitmapConverter.ToBitmap(gaussianBlurFrame);
                            }));
                            
                            }

                        }
                    }
                });
            }
        }


        // Median Blur Button
        private void button5_Click(object sender, EventArgs e)
        {
            if(capture!=null && capture.IsOpened())
            {
                //isCameraRunning = true;
                Task.Run(() =>
                {
                    while(isCameraRunning && capture.IsOpened())
                    {
                        using (var frame = new Mat())
                        using(var medianBlurFrame = new Mat())
                        {
                            capture.Read(frame);
                            if(!frame.Empty())
                            {
                      
                                Cv2.MedianBlur(frame, medianBlurFrame, 21); // must be odd size 1 3 5 7 ... 21 23 .. buraya trackbar ile deger degistirme eklenecek
                                Invoke(new Action(() =>
                                {
                                    pictureBox5.Image?.Dispose();
                                    pictureBox5.Image =
                                    OpenCvSharp.Extensions.BitmapConverter.ToBitmap(medianBlurFrame);
                                }));
                            }
                        }
                    }
                });
            }
        }


        // Bilateral Blur Button
        private void button6_Click(object sender, EventArgs e)
        {
            

            if(capture != null && capture.IsOpened())
            {
                //isCameraRunning = true;
                Task.Run(() =>
                {
                    while (isCameraRunning && capture.IsOpened())
                    {
                        using (var frame = new Mat())
                        using (var bilateralBlurFrame = new Mat())
                        {
                            capture.Read(frame);
                            if(!frame.Empty())
                            {
                                Cv2.BilateralFilter(frame, bilateralBlurFrame, 30, 30, 30);
                                Invoke(new Action(() =>
                                {
                                    pictureBox6.Image?.Dispose();
                                    pictureBox6.Image =
                                    OpenCvSharp.Extensions.BitmapConverter.ToBitmap(bilateralBlurFrame);
                                }));

                            }
                        }
                    }
                });
            }
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {

        }
        // Gaussian Blur Picture Box
        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }
        // Median Blur Picture Box
        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        // Bilateral Blur Picture Box
        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }
    }
}
