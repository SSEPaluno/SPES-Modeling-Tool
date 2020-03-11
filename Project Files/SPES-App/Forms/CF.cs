using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPES_App.Forms
{
    public partial class ContextFunction : Form
    {
        public int value = 0;
        public ContextFunction()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ContextFunction_Load(object sender, EventArgs e)
        {
            
        }

        private void LCF_CheckedChanged(object sender, EventArgs e)
        {
            value = 3;
        }

        private void CCF_CheckedChanged(object sender, EventArgs e)
        {
            value = 2;
        }

        private void PCF_CheckedChanged(object sender, EventArgs e)
        {
            value = 1;
        }
    }
}
