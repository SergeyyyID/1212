using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTTP
{
    public partial class Form1 : Form
    {
        private string indexHtml;
        private string rootPath = @"C:\1212";
        
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void LoadHtml()
        {
            try
            {
                string filePath = Path.Combine(rootPath, "index.html");
                indexHtml = File.ReadAllText(filePath);

                // Вывод в текстовое поле, для проверки
                richTextBox1.Text = indexHtml;

                // Эмуляция запросов браузера
                EmulateRequest("styles.css");
                EmulateRequest("image.jpg");
                EmulateRequest("script.js");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void EmulateRequest(string fileName)
        {
            string filePath = Path.Combine(rootPath, fileName);
            if (File.Exists(filePath))
            {
                richTextBox2.AppendText($"Запрос: {fileName}  (найден)\n");
            }
            else
            {
                richTextBox2.AppendText($"Запрос: {fileName}  (не найден)\n");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadHtml();
        }
    }
}