using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpielNaoKinect.Kinect
{
    public partial class WindowAngles : Form
    {
        public WindowAngles()
        {
            InitializeComponent();
        }

        public void ZeigeDaten(float p)
        {
            XElbowRight.Text = p.ToString();
        }

    }
}
