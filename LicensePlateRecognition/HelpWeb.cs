using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LicensePlateRecognition
{
    public partial class HelpWeb : Form
    {

        Sunisoft.IrisSkin.SkinEngine skinEngine1;

        public HelpWeb()
        {
            InitializeComponent();

            //C#皮肤文件加载
            this.Text = "studio";
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.skinEngine1.SkinFile = Application.StartupPath + "//OmegaSkin.ssk";
        }


    }
}
