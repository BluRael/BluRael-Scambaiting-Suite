using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysKey
{
    public partial class SyskeyMain : Form
    {
        public SyskeyMain()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Environment.Exit(0); //Close program
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0); //Close program
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }
    }
}
