using DevExpress.XtraEditors;
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

namespace DeviceDataCatcher.Views
{
    public partial class HomeView : Field4Control
    {
        public HomeView()
        {
            InitializeComponent();
            if (File.Exists($"{WorkPath}\\Images\\Logo.png"))
            {
                using (FileStream file = File.OpenRead($"{WorkPath}\\Images\\Logo.png"))
                {
                    pictureEdit1.Image = Image.FromStream(file);
                }
            }
        }
    }
}
