using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListBox_and_Colors
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void lst_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void черныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.txt.BackColor = System.Drawing.Color.Black;
        }

        private void красныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.txt.BackColor = System.Drawing.Color.Red;
        }

        private void синийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.txt.BackColor = System.Drawing.Color.Blue;
        }

        private void зеленыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.txt.BackColor = System.Drawing.Color.Green;
        }

        private void желтыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.txt.BackColor = System.Drawing.Color.Yellow;
        }
    }
}
