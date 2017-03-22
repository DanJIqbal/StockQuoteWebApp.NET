using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Quotes
{
    public partial class PictureBox : Form
    {
        public PictureBox(string url)
        {
            
            InitializeComponent();
            pictureBox1.Load(url);
        }

        public void changeChartURLofPictureBox(string url)
        {
            pictureBox1.Load(url);
        }
    }
}
