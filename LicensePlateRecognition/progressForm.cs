using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LicensePlateRecognition
{
    public partial class progressForm : Form
    {
        public progressForm()
        {
            InitializeComponent();
        }


        public void Addprogess()
        {
            progressBar1.Value++;
        }

    }
}
