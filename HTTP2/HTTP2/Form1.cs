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

namespace HTTP2
{
    public partial class Form1 : Form
    {
        private string _logFilePath = "output.log";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            using (StringWriter stringWriter = new StringWriter())
            {
               
                TextWriter originalConsoleOut = Console.Out;

                Console.SetOut(stringWriter);

                try
                {
                    
                    SimulateConsoleOutput();

                   
                    string output = stringWriter.ToString();

                  
                    textBox1.Text = output;

                  
                    WriteToFile(output);
                }
                finally
                {
                
                    Console.SetOut(originalConsoleOut);
                }
            }
        }

        private void SimulateConsoleOutput()
        {
            Console.WriteLine("Первая строка");
            Console.WriteLine("Вторая строка");
            Console.WriteLine($"Текущее время: {DateTime.Now}");
            Console.WriteLine($"Случайное число: {new Random().Next(100)}");
        }

        private void WriteToFile(string output)
        {
            try
            {
                File.AppendAllText(_logFilePath, output);
                MessageBox.Show($"Данные сохранены в файл: {_logFilePath}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения в файл: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

