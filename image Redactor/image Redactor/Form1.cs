using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace image_Redactor
{
    public partial class Form1 : Form
    {
        private Bitmap _bitmap;
        private Graphics _graphics;
        private Point _lastPoint;
        private bool _isDrawing;

        private Random _random = new Random();

        public Form1()
        {
            InitializeComponent();
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphics = Graphics.FromImage(_bitmap);
            _graphics.SmoothingMode = SmoothingMode.AntiAlias;
            _graphics.Clear(Color.White);

            pictureBox1.Image = _bitmap;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения (*.bmp, *.jpg, *.jpeg, *.png)|*.bmp;*.jpg;*.jpeg;*.png|Все файлы (*.*)|*.*";
            openFileDialog.Title = "Открыть изображение";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var tempBitmap = new Bitmap(openFileDialog.FileName))
                    {
                        _bitmap = new Bitmap(tempBitmap, pictureBox1.Width, pictureBox1.Height); 
                        _graphics = Graphics.FromImage(_bitmap);
                        _graphics.SmoothingMode = SmoothingMode.AntiAlias; 
                        pictureBox1.Image = _bitmap;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при открытии изображения: {ex.Message}");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG (*.png)|*.png|JPEG (*.jpg, *.jpeg)|*.jpg;*.jpeg|BMP (*.bmp)|*.bmp|Все файлы (*.*)|*.*";
            saveFileDialog.Title = "Сохранить изображение";
            saveFileDialog.DefaultExt = "png"; 
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ImageFormat format;
                    switch (Path.GetExtension(saveFileDialog.FileName).ToLower())
                    {
                        case ".png":
                            format = ImageFormat.Png;
                            break;
                        case ".jpg":
                        case ".jpeg":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            format = ImageFormat.Bmp;
                            break;
                        default:
                            format = ImageFormat.Png;
                            break;
                    }

                    _bitmap.Save(saveFileDialog.FileName, format);
                    MessageBox.Show("Изображение сохранено.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении изображения: {ex.Message}");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            _isDrawing = true;
            _lastPoint = e.Location;
        }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {

                using (Pen pen = new Pen(Color.Black, 2))
                {
                    _graphics.DrawLine(pen, _lastPoint, e.Location); 
                }
                _lastPoint = e.Location; 
                pictureBox1.Refresh(); 
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _isDrawing = false;
        }

        private void buttonAddRandomPoints_Click(object sender, EventArgs e)
        {
            AddRandomPoints(1000);
            pictureBox1.Refresh();
        }
        private void AddRandomPoints(int numberOfPoints)
        {
            for (int i = 0; i < numberOfPoints; i++)
            {

                int x = _random.Next(0, pictureBox1.Width);
                int y = _random.Next(0, pictureBox1.Height);


                Color randomColor = Color.FromArgb(
                 _random.Next(256),
                  _random.Next(256),
                  _random.Next(256)
                 );


                using (Brush brush = new SolidBrush(randomColor))
                {
                    _graphics.FillRectangle(brush, x, y, 1, 1);
                }

            }
        }
    }
}
